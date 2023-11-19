﻿using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.ViewModel
{
    public class CategoryViewModelNew : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private Category parentCategory { get; set; }
        public Category ParentCategory 
        {
            get => parentCategory;
            set
            {
                parentCategory = value;
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
        private bool isTopCategory;
        public bool IsTopCategory
        {
            get => isTopCategory;
            set
            {
                isTopCategory = value;
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
                parentCategory = MainViewModel.SelectedCategory;
                if (MainViewModel.SelectedCategory == null) IsTopCategory = true;
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
        public CategoryViewModelNew()
        {
            var user = ObjectRepository.DataProvider.CurrentUser;
            PermittedUsers.Add(user);
        }
        public void AddPermittedUser(User user)
        {
            if (PermittedUsers.IndexOf(user) == -1)
            {
                PermittedUsers.Add(user);
            }
        }
        public bool SaveCategory()
        {
            if(CategoryName == "") return false;

            var category = new Category()
            {
                Name = CategoryName,
                ParentID = 0,
                OwnerID = ObjectRepository.DataProvider.CurrentUser.ID,
                Shared = (PermittedUsers.Count > 1) ? true : false
            };

            if(!isTopCategory && ParentCategory == null) return false;

            if (!IsTopCategory) category.ParentID = ParentCategory.ID;

            try
            {
                ObjectRepository.DataProvider.PostCategory(category); // no need to set the id, because category will be set in dataprovider and it is the same object

                if(category.ParentID == 0)
                    ObjectRepository.DataProvider.PostPermission(PermittedUsers, category.ID);

                if(category.ParentID != 0)
                    ParentCategory.ChildCategories.Add(category);
                else
                    MainViewModel.Categories.Add(category);

                MainViewModel.SetDefaultView();

            }
            catch (Exception ex) { return false; }

            return true;

        }

        public void Exit() => MainViewModel.SetDefaultView();
    }
}
