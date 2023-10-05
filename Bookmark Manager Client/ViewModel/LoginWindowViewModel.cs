using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.ViewModel
{
    public class LoginWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }


        public LoginWindowViewModel() 
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            Host = ObjectRepository.AppConfiguration.Host;
            Port = Convert.ToString(ObjectRepository.AppConfiguration.Port);
            Username = ObjectRepository.AppConfiguration.UserName;
            Password = ObjectRepository.AppConfiguration.Password;
        }

        public void SaveSettings()
        {
            ObjectRepository.AppConfiguration.Host = Host;
            ObjectRepository.AppConfiguration.Port = Convert.ToInt32(Port);
            ObjectRepository.AppConfiguration.UserName = Username;
            ObjectRepository.AppConfiguration.Password = Password;
            ObjectRepository.AppConfiguration.SaveConfig();
        }
    }
}
