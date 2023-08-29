using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmark_Manager_Client.Configurators;

namespace Bookmark_Manager_Client.Controller
{
    class LoginController
    {
        public string Username
        {
            get => GlobalConfigurator.AppConfiguration.UserName;
            set => GlobalConfigurator.AppConfiguration.UserName = value;
        }
        public string Host
        {
            get => GlobalConfigurator.AppConfiguration.Host;
            set => GlobalConfigurator.AppConfiguration.Host = value;
        }
        public string Port
        {
            get => Convert.ToString(GlobalConfigurator.AppConfiguration.Port);
            set => GlobalConfigurator.AppConfiguration.Port = int.Parse(value);
        }
        public string Password
        {
            get => GlobalConfigurator.AppConfiguration.Password;
            set => GlobalConfigurator.AppConfiguration.Password = Password;
        }

        public LoginController()
        {

        }
    }
}
