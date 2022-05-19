using Box.V2;
using Box.V2.Config;
using Box.V2.Exceptions;
using Box.V2.JWTAuth;
using Box.V2.Models;
using Box.V2.Utility;
using BoxApiSample.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoxApiSample.Api
{
    /// <summary>
    /// BOX API を使うクライアントクラス
    /// </summary>
    public class BoxApi
    {
        #region フィールド・プロパティ
        /// <summary>
        ///  ロガー
        /// </summary>
        Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// BOX クライアント
        /// </summary>
        private BoxClient _client = null;

        /// <summary>
        /// BOX 認証用 JSON ファイルのパス
        /// </summary>
        public string ConfigJsonFile {get;set;}

        /// <summary>
        /// 再試行間隔(ms)
        /// </summary>
        public int RetryInterval { get; set; }

        /// <summary>
        /// 再試行回数
        /// </summary>
        public int RetryCount { get; set; }
        #endregion

        #region 定数
        /// <summary>
        /// フォルダを一度に取得する件数の上限
        /// </summary>
        private const int GET_FOLDER_ITEM_LIMIT = 10000;

        /// <summary>
        /// 分割アップロードを行う閾値（バイト）
        /// 20000000 以上にしないとダメです（※ 約20MB）
        /// </summary>
        private const int SINGLE_UPLOAD_MODE_LIMIT = 20000000; // 52428800;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BoxApi()
        {
            RetryInterval = 1000;
            RetryCount = 3;
        }
        #endregion

        #region 外部向け
        /// <summary>
        /// 接続します
        /// </summary>
        /// <returns></returns>
        /// <remarks>接続に失敗した場合は</remarks>
        public async Task Connect()
        {
            logger.Info("BOX API の認証を行います");
            var jsonConfig = File.ReadAllText(ConfigJsonFile, Encoding.UTF8);
            var config = BoxConfigBuilder.CreateFromJsonString(jsonConfig).Build();
            var session = new BoxJWTAuth(config);
            var adminToken = await session.AdminTokenAsync(); //valid for 60 minutes so should be cached and re-used
            _client = session.AdminClient(adminToken);
            logger.Info("  -> BOX API の認証 OK でした");
        }

        /// <summary>
        /// 指定したフォルダの下にフォルダを作成します（深い階層でも可）
        /// </summary>
        /// <param name="id">フォルダID</param>
        /// <param name="targetFolders">作成対象のフォルダ（各 Id は空で良い）</param>
        /// <returns>フォルダ情報のリスト（深いほど後ろに積まれる）</returns>
        public async Task<List<BoxFolderItem>> CreateFolders(string id, List<BoxFolderItem> targetFolders)
        {
            var parentId = id;
            for (int i = 0; i < targetFolders.Count; i++)
            {
                var targetFolder = targetFolders[i];
                var folderParams = new BoxFolderRequest()
                {
                    Name = targetFolder.Name,
                    Parent = new BoxRequestEntity()
                    {
                        Id = parentId,
                    }
                };

                // 作成
                // (フォルダがなかったときだけ作る）
                // 10000 決め打ちなのはダメですが、そんな数いかないような設計にしましょう
                var folders = await GetFolderItemsAsyncWithRetry(parentId, GET_FOLDER_ITEM_LIMIT);
                var folder = folders.Entries.FirstOrDefault(f => f.Name == targetFolder.Name);
                if (folder == null)
                {
                    logger.Info("フォルダがなかったので作成します[{0}]", targetFolder.Name);
                    folder = await CreateFolderAsyncWithRetry(folderParams);
                    logger.Info("  -> フォルダがなかったので作成しました[{0}, ID: {1}]", targetFolder.Name, folder.Id);
                }
                else
                {
                    logger.Info("フォルダがあったので作成しません[{0}, ID: {1}]", targetFolder.Name, folder.Id);
                }

                // 取得した ID を保持
                targetFolder.Id = folder.Id;
                // 作成したフォルダを親とする
                parentId = folder.Id;
            }

            return targetFolders;
        }


        /// <summary>
        /// 指定したローカルフォルダをアップロードします
        /// </summary>
        /// <param name="id">アップロード先フォルダID</param>
        /// <param name="localFolder">ローカルフォルダパス</param>
        public async Task UploadFolder(string id, string localFolder)
        {
            // ローカルのフォルダ名を取得
            // 後ろに \ が入っていることを考慮しています
            var dirName = (new DirectoryInfo(localFolder)).Name;

            // フォルダを作成します
            var folderReq = new List<BoxFolderItem>()
            {
                new BoxFolderItem
                {
                    Name = dirName
                }
            };
            var folderRes = await CreateFolders(id, folderReq);
            var targetId = folderReq[0].Id;

            // フォルダの一覧を取得します
            var folders = Directory.GetDirectories(localFolder);
            foreach (var folder in folders)
            {
                // 再帰
                await UploadFolder(targetId, folder);
            }

            // ファイルの一覧を取得します
            var files = Directory.GetFiles(localFolder);
            foreach (var file in files)
            {
                var fileReq = new BoxFileRequest
                {
                    Name = Path.GetFileName(file),
                    Parent = new BoxFolderRequest { Id = targetId }
                };
                logger.Info("ファイルをアップロードします[{0}]", fileReq.Name);
                var fileRes = await UploadFileAsyncWithRetry(fileReq, file);
                logger.Info("  -> ファイルをアップロードしました[{0}, ID: {1}]", fileReq.Name, fileRes.Id);
            }
        }
        #endregion

        #region API ラッパー
        /// <summary>
        /// 指定したフォルダの一覧を取得します（リトライ機能付き）
        /// </summary>
        /// <param name="id">フォルダID</param>
        /// <param name="limit">上限</param>
        /// <returns>フォルダの一覧</returns>
        public async Task<BoxCollection<BoxItem>> GetFolderItemsAsyncWithRetry(string id, int limit)
        {
            BoxCollection<BoxItem> ret = null;
            Exception last_e = null;

            for (int i = 0; i <= RetryCount; i++)
            {
                var needReconnect = false;

                // メイン処理
                try
                {
                    ret = await _client.FoldersManager.GetFolderItemsAsync(id, GET_FOLDER_ITEM_LIMIT);
                    last_e = null;
                    break;
                }
                catch (ThreadAbortException ae)
                {
                    throw ae;
                }
                catch (BoxSessionInvalidatedException)
                {
                    needReconnect = true;
                }
                catch (Exception e)
                {
                    last_e = e;
                }

                // 再接続が必要なとき
                if (needReconnect && i < RetryCount)
                {
                    try
                    {
                        logger.Warn("セッションが切れたので再接続します。");
                        await Connect();
                    }
                    catch (ThreadAbortException ae)
                    {
                        throw ae;
                    }
                    catch (Exception e)
                    {
                        // 再接続に失敗
                        last_e = e;
                    }
                }
                
                if (i < RetryCount)
                {
                    logger.Warn("フォルダ一覧の取得をリトライします。{0}回目。", i + 1);
                    Thread.Sleep(RetryInterval);
                }
            }

            if (last_e != null)
            {
                throw last_e;
            }

            return ret;
        }

        /// <summary>
        /// 指定したフォルダを作成します（リトライ機能付き）
        /// </summary>
        /// <param name="req">リクエスト</param>
        /// <returns>フォルダ情報</returns>
        public async Task<BoxFolder> CreateFolderAsyncWithRetry(BoxFolderRequest req)
        {
            BoxFolder ret = null;
            Exception last_e = null;

            for (int i = 0; i <= RetryCount; i++)
            {
                var needReconnect = false;

                // メイン処理
                try
                {
                    ret = await _client.FoldersManager.CreateAsync(req);
                    last_e = null;
                    break;
                }
                catch (ThreadAbortException ae)
                {
                    throw ae;
                }
                catch (BoxSessionInvalidatedException)
                {
                    needReconnect = true;
                }
                catch (Exception e)
                {
                    last_e = e;
                }

                // 再接続が必要なとき
                if (needReconnect && i < RetryCount)
                {
                    try
                    {
                        logger.Warn("セッションが切れたので再接続します。");
                        await Connect();
                    }
                    catch (ThreadAbortException ae)
                    {
                        throw ae;
                    }
                    catch (Exception e)
                    {
                        // 再接続に失敗
                        last_e = e;
                    }
                }

                if (i < RetryCount)
                {
                    logger.Warn("フォルダ作成をリトライします。{0}回目。", i + 1);
                    Thread.Sleep(RetryInterval);
                }
            }

            if (last_e != null)
            {
                throw last_e;
            }

            return ret;
        }

        /// <summary>
        /// 指定したファイルをアップロードします（リトライ機能付き）
        /// </summary>
        /// <param name="req">リクエスト</param>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>フォルダ情報</returns>
        public async Task<BoxFile> UploadFileAsyncWithRetry(BoxFileRequest req, string filePath)
        {
            BoxFile ret = null;
            Exception last_e = null;

            for (int i = 0; i <= RetryCount; i++)
            {
                var needReconnect = false;

                // メイン処理
                try
                {
                    var fi = new FileInfo(filePath);
                    var filesize = fi.Length;
                    using (var file = File.OpenRead(filePath))
                    {
                        // 50MB(=52428800Byte)を超えるかどうかで呼び出しを変えています
                        if (filesize >= SINGLE_UPLOAD_MODE_LIMIT)
                        {
                            var progress = new Progress<BoxProgress>(val => { logger.Info("{0}をアップロード中・・・ {1}%", req.Name, val.progress); });
                            ret = await _client.FilesManager.UploadUsingSessionAsync(file, req.Name, req.Parent.Id, null, progress);
                        }
                        else
                        {
                            ret = await _client.FilesManager.UploadAsync(req, file);
                        }
                    }
                    last_e = null;
                    break;
                }
                catch (ThreadAbortException ae)
                {
                    throw ae;
                }
                catch (BoxSessionInvalidatedException)
                {
                    needReconnect = true;
                }
                catch (Exception e)
                {
                    last_e = e;
                }

                // 再接続が必要なとき
                if (needReconnect && i < RetryCount)
                {
                    try
                    {
                        logger.Warn("セッションが切れたので再接続します。");
                        await Connect();
                    }
                    catch (ThreadAbortException ae)
                    {
                        throw ae;
                    }
                    catch (Exception e)
                    {
                        // 再接続に失敗
                        last_e = e;
                    }
                }

                if (i < RetryCount)
                {
                    logger.Warn("ファイルアップロードをリトライします。{0}回目。", i + 1);
                    Thread.Sleep(RetryInterval);
                }
            }

            if (last_e != null)
            {
                throw last_e;
            }

            return ret;
        }
        #endregion
    }
}
