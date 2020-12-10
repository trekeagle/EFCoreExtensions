using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTreker.Utils
{
    /// <summary>
    /// 读取配置文件需安装的包如下
    /// NuGet: Install-Package Microsoft.Extensions.Configuration.Json
    /// </summary>
    public class ConfigManager
    {
        #region 单例模式
        private static ConfigManager instance = new ConfigManager();
        private ConfigManager() { }
        public static ConfigManager GetInstance(string appsettingsJsonFilePath = null)
        {
            if (string.IsNullOrEmpty(appsettingsJsonFilePath))
            {
                appsettingsJsonFilePath = "appsettings.json";
            }

            #region 读取json配置文件
            instance.Configuration = new ConfigurationBuilder()
                 .Add(new JsonConfigurationSource
                 {
                     Path = appsettingsJsonFilePath,
                     ReloadOnChange = true //ReloadOnChange = true 指示appsettings.json被修改后会重新加载
                }).Build();

            #endregion

            #region 另一读取json配置文件方式

            //Configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json").Build();

            #endregion
            return instance;
        }
        #endregion
        public IConfiguration Configuration { get; set; }



    }
}
