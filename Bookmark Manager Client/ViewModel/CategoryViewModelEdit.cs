using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Bookmark_Manager_Client.Utils;
using Windows.UI.Xaml.Media.Animation;

namespace Bookmark_Manager_Client.ViewModel
{
    public class CategoryViewModelEdit : INotifyPropertyChanged
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
                var category = MainViewModel.SelectedCategory;
                CategoryName = category.Name;
                categoryDescription = category.Description;
                addPermittedUserToList(category.ID);

                if(category.ParentID == 0)
                    IsTopCategory = true;
                else
                    IsTopCategory = false;

                OnPropertyChanged();
            }
        }

        private bool isTopCategory;
        public bool IsTopCategory { get => isTopCategory; private set { isTopCategory = value; OnPropertyChanged(); } }

        private string categoryName;
        public string CategoryName { get => categoryName; set { categoryName = value; OnPropertyChanged(); } }

        private string categoryDescription;
        public string CategoryDescription { get => categoryDescription; set { categoryDescription = value; OnPropertyChanged(); } }
        
        

        private ObservableCollection<User> permittedUsers = new ObservableCollection<User>();
        public ObservableCollection<User> PermittedUsers { get => permittedUsers; set { permittedUsers = value; OnPropertyChanged(); } }
        
        public CategoryViewModelEdit()
        {
            
        }
        
        public void AddPermittedUser(User user)
        {
            if (PermittedUsers.IndexOf(user) == -1)
            {
                PermittedUsers.Add(user);
            }
        }
        private void addPermittedUserToList(uint categoryId)
        {
            var users = ObjectRepository.DataProvider.GetPermittedUsers(categoryId);
            foreach (var user in users)
                permittedUsers.Add(user);
        }

        public bool UpdateCategory()
        {
            if (CategoryName == "")
                return false;

            var category = MainViewModel.SelectedCategory;
            category.Name = CategoryName;
            category.Description = CategoryDescription;

            if (!ObjectRepository.DataProvider.PutCategory(category))
                return false;

            if (category.ParentID == 0)
                if (!ObjectRepository.DataProvider.ChangePermissions(PermittedUsers, category.ID))
                    return false;

            MainViewModel.SetDefaultView();
            return true;
        }

        public void Exit() => MainViewModel.SetDefaultView();
    }
}
