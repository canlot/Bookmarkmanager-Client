using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string host;
        public string Host { get => host; set { host = value; OnPropertyChanged(); } }

        private string port;
        public string Port { get => port; set { host = value; OnPropertyChanged(); } }

        public string UserName { get; set; }

        public string Password { get; set; }



    }
}
