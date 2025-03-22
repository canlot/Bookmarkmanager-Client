using Bookmark_Manager_Client.Configurators;
using Bookmark_Manager_Client.DataProvider;
using Bookmark_Manager_Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client
{
    public static class ObjectRepository
    {
        private static bool dataProviderAlreadySet = false;
        private static IDataProvider dataProvider;
        public static IDataProvider DataProvider 
        {
            get =>  dataProvider;
            set
            {
                if (dataProviderAlreadySet) return;
                else dataProvider = value;
                dataProviderAlreadySet = true;
            }
        }

        private static bool appConfigurationAlreadySet = false;

        private static AppConfiguration appConfiguration;

        public static AppConfiguration AppConfiguration
        {
            get => appConfiguration;
            set 
            {
                if (appConfigurationAlreadySet) return;
                else appConfiguration = value;
                appConfigurationAlreadySet = true;
            }
        }
        
        public static EventDispatcher EventDispatcher { get; } = new EventDispatcher();

        static ObjectRepository()
        {
            DataProvider = new RestDataProvider();
            AppConfiguration = new AppConfiguration();
        }
        
    }
}
