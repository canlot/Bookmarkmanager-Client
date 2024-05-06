using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
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
        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get => mainViewModel;
            set
            {
                mainViewModel = value;
                getAllUsers();
            }
        }
        private string host;
        public string Host { get => host; set { host = value; OnPropertyChanged(); } }

        private string port;
        public string Port { get => port; set { host = value; OnPropertyChanged(); } }

        private string userName;
        public string UserName { get => userName; set { userName = value; OnPropertyChanged(); } }

        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }

        private User selectedUser;
        public User SelectedUser { get => selectedUser; set { selectedUser = value; OnPropertyChanged(); } }

        private ObservableCollection<User> users = new ObservableCollection<User>();
        public ObservableCollection<User> Users { get => users; set { users = value; OnPropertyChanged(); } }

        private void getAllUsers()
        {
            Task.Run(async () =>
            {
                var retUsers = await ObjectRepository.DataProvider.GetAllUsersAsync();
                foreach (var user in retUsers)
                {
                    Users.Add(user);
                }
            });
        }

        public async Task AddUser(User user, string userPassword)
        {
            Users.Add(user);
        }
        public async Task SaveUser(User user, string userPassword)
        {
            if(SelectedUser.Name != user.Name)
                SelectedUser.Name = user.Name;
            if(SelectedUser.Email != user.Email)
                SelectedUser.Email = user.Email;
            if(SelectedUser.Administrator != user.Administrator)
                SelectedUser.Administrator = user.Administrator;
        }

        public SettingsViewModel() 
        {
            
        }

        public void Exit() => MainViewModel.SetDefaultView();
    }
}
