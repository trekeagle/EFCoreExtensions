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
        public static IConfiguration Configuration { get; set; }

        static ConfigManager()
        {


            #region 读取json配置文件
            Configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource
                {
                    Path = "appsettings.json",
                    ReloadOnChange = true //ReloadOnChange = true 指示appsettings.json被修改后会重新加载
                }).Build();

            #endregion

            #region 另一读取json配置文件方式

            //Configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json").Build();

            #endregion
        }

    }
}
