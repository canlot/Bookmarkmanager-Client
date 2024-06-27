using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Bookmark_Manager_Client.ViewModel
{
    public class LoginWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string Email { get; set; }
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
            Email = ObjectRepository.AppConfiguration.Email;
            //Password = ObjectRepository.AppConfiguration.Password;

#if (DEBUG == false)
            try
            {
                PasswordVault vault = new Windows.Security.Credentials.PasswordVault();
                PasswordCredential credential = vault.Retrieve("Bookmarkmanager", ObjectRepository.AppConfiguration.Email);
                Password = credential.Password;
            }
            catch (Exception ex) { }
#endif
        }


        public void SaveSettings()
        {
            ObjectRepository.AppConfiguration.Host = Host;
            ObjectRepository.AppConfiguration.Port = Convert.ToInt32(Port);
            ObjectRepository.AppConfiguration.Email = Email;
            ObjectRepository.AppConfiguration.Password = Password;
            ObjectRepository.AppConfiguration.SaveConfig();


#if (DEBUG == false)
            PasswordVault vault = new Windows.Security.Credentials.PasswordVault();
            PasswordCredential credential = new PasswordCredential("Bookmarkmanager", ObjectRepository.AppConfiguration.Email, ObjectRepository.AppConfiguration.Password);
            vault.Add(credential);
#endif
        }
    }
}
