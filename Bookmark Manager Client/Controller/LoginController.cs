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
            get => ObjectRepository.AppConfiguration.UserName;
            set => ObjectRepository.AppConfiguration.UserName = value;
        }
        public string Host
        {
            get => ObjectRepository.AppConfiguration.Host;
            set => ObjectRepository.AppConfiguration.Host = value;
        }
        public string Port
        {
            get => Convert.ToString(ObjectRepository.AppConfiguration.Port);
            set => ObjectRepository.AppConfiguration.Port = int.Parse(value);
        }
        public string Password
        {
            get => ObjectRepository.AppConfiguration.Password;
            set => ObjectRepository.AppConfiguration.Password = Password;
        }

        public LoginController()
        {

        }
    }
}
