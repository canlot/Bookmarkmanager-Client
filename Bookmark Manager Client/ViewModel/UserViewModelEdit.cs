using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.ViewModel
{
    public class UserViewModelEdit : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private SettingsViewModel settingsViewModel;
        public SettingsViewModel SettingsViewModel
        {
            get => settingsViewModel;
            set
            {
                settingsViewModel = value;
            }
        }
        public UserViewModelEdit(SettingsViewModel vm)
        {
            settingsViewModel = vm;
            Email = vm.SelectedUser.Email;
            Username = vm.SelectedUser.Name;
            Administrator = vm.SelectedUser.Administrator;
        }
        private string email;
        public string Email { get => email; set { email = value; OnPropertyChanged(); } }
        private string username;
        public string Username { get => username; set { username = value; OnPropertyChanged(); } }
        private bool isAdmin;
        public bool Administrator { get => isAdmin; set { isAdmin = value; OnPropertyChanged(); } }
        private string password = "";
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        public async Task Save()
        {
            var user = new User() { Administrator = Administrator, Email = Email, Name = Username };
            await SettingsViewModel.SaveUser(user, Password);
        }
    }
}
