using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
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

        public async Task<List<BoxDirectory>> GetDirectories(string directoryId)
        {
            var items = await _client.FoldersManager.GetFolderItemsAsync(directoryId, 100);
            return items.Entries.Select(item =>
            {
                return new BoxDirectory
                {
                    Id = item.Id,
                    Name = item.Name,
                };
            }).ToList();
        }

        /// <summary>
        /// 指定したフォルダの下にフォルダを作成します（深い階層でも可）
        /// </summary>
        /// <param name="directoryId">フォルダID</param>
        /// <returns>フォルダ情報のリスト（深いほど後ろに積まれる）</returns>
        public async Task<List<BoxDirectory>> CreateDirectories(string directoryId)
        {
            List<BoxDirectory> ret = null;


            return ret;
        }
    }
}
