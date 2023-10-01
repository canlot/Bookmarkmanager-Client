using Bookmark_Manager_Client.DataProvider;
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
        private static IDataProvider dataProvider { get; set; }
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
        
        
    }
}
