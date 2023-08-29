using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.Configurators
{
    public static class GlobalConfigurator
    {
        public static string GlobalConfigurationPath = ".";
        private static string AppConfigurationFilePath
        {
            get => GlobalConfigurationPath + "\\" + "config.ini";
        }

        public static AppConfiguration AppConfiguration
        {
            get;
            private set;
        }


        public static void Initialize()
        {
            AppConfiguration = new AppConfiguration(AppConfigurationFilePath);
        }
        public static bool LoadConfig()
        {
            return AppConfiguration.LoadConfig();
        }
    }
}
