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

namespace Bookmark_Manager_Client.ViewModel
{
    public class CategoryViewModelEdit : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private Category parentCategory { get; set; }
        public Category ParentCategory 
        {
            get
            {
                if (parentCategory == null)
                {
                    parentCategory = mainViewModel.GetParentCategory(Category);
                    //OnPropertyChanged();
                }
                return parentCategory;
            }
            set
            {
                parentCategory = value;
                OnPropertyChanged();
            }
        }

        private Category category;
        public Category Category 
        {
            get => category; 
            set
            {
                category = value;
                OnPropertyChanged();
            }
        }
        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel 
        {
            get => mainViewModel;
            set
            {
                mainViewModel = value;
                category = MainViewModel.SelectedCategory;
                CategoryName = category.Name;
                category.PermissionUsers.ToList().ForEach(x => PermittedUsers.Add(x));
                OnPropertyChanged();
            }
        }


        private string categoryName;
        public string CategoryName
        {
            get => categoryName;
            set
            {
                categoryName = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<User> permittedUsers = new ObservableCollection<User>();
        public ObservableCollection<User> PermittedUsers
        {
            get => permittedUsers;
            set
            {
                permittedUsers = value;
                OnPropertyChanged();
            }
        }
        public void AddPermittedUser(User user)
        {
            if(PermittedUsers.IndexOf(user) == -1)
            {
                PermittedUsers.Add(user);
            }
        }
        public bool UpdateCategory()
        {
            var category = new Category
            {
                ID = MainViewModel.SelectedCategory.ID,
                Name = categoryName,
                Bookmarks = MainViewModel.SelectedCategory.Bookmarks,
                ChildCategories = MainViewModel.SelectedCategory.ChildCategories,
                OwnerID = MainViewModel.SelectedCategory.OwnerID,
                ParentID = MainViewModel.SelectedCategory.ParentID,
                PermissionUsers = PermittedUsers,
                Shared = (PermittedUsers.Count > 1) ? true : false
            };
            if(ObjectRepository.DataProvider.PutCategory(category))
            {

                MainViewModel.Categories.ReplaceCategory(MainViewModel.SelectedCategory, category);
                return true;
            }
            return false;
            //ObjectRepository.DataProvider.PutCategory()
        }
        public CategoryViewModelEdit()
        {
            
        }
    }
}
