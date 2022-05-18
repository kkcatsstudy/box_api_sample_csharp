using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
using Box.V2.Models;
using BoxApiSample.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxApiSample.Api
{
    public class BoxApi
    {
        private BoxClient _client = null;
        public string ConfigJsonFile {get;set;}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BoxApi()
        {
        }

        public async Task Connect()
        {
            var jsonConfig = File.ReadAllText(ConfigJsonFile, Encoding.UTF8);
            var config = BoxConfigBuilder.CreateFromJsonString(jsonConfig).Build();
            var session = new BoxJWTAuth(config);
            var adminToken = await session.AdminTokenAsync(); //valid for 60 minutes so should be cached and re-used
            _client = session.AdminClient(adminToken);
        }

        /// <summary>
        /// 指定したフォルダの下にフォルダを作成します（深い階層でも可）
        /// </summary>
        /// <param name="directoryId">フォルダID</param>
        /// <param name="targetDirectories">作成対象のフォルダ（各 Id は空で良い）</param>
        /// <returns>フォルダ情報のリスト（深いほど後ろに積まれる）</returns>
        public async Task<List<BoxDirectory>> CreateDirectories(string directoryId, List<BoxDirectory> targetDirectories)
        {
            var parentId = directoryId;
            foreach (var d in targetDirectories)
            {
                var folderParams = new BoxFolderRequest()
                {
                    Name = d.Name,
                    Parent = new BoxRequestEntity()
                    {
                        Id = parentId,
                    }
                };

                // 作成
                // (フォルダがなかったときだけ作る）
                // 10000 決め打ちなのはダメですが、そんな数いかないような設計にしましょう
                var folders = await _client.FoldersManager.GetFolderItemsAsync(parentId, 10000);
                var folder = folders.Entries.FirstOrDefault(f => f.Name == d.Name);
                if (folder == null)
                {
                    Console.WriteLine("フォルダがなかったので作成します[{0}]", d.Name);
                    folder = await _client.FoldersManager.CreateAsync(folderParams);
                }
                else
                {
                    Console.WriteLine("フォルダがあったので作成しません[{0}, ID: {1}]", d.Name, folder.Id);
                }

                // 取得した ID を保持
                d.Id = folder.Id;
                // 作成したフォルダを親とする
                parentId = folder.Id;
            }

            return targetDirectories;
        }
    }
}
