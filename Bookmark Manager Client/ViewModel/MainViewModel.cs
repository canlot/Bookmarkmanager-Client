﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Bookmark_Manager_Client.Commands;
using Bookmark_Manager_Client.Controller;
using Bookmark_Manager_Client.Model;
using Bookmark_Manager_Client.UserControls;
using Bookmark_Manager_Client.Utils;

namespace Bookmark_Manager_Client.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModelController controller = new MainViewModelController();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private ObservableCollection<User> allUsers;
        public ObservableCollection<User> AllUsers
        {
            get => allUsers;
            set
            {
                allUsers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        public ObservableCollection<Category> Categories 
        { 
            get => categories; 
            set
            {
                categories = value;
                OnPropertyChanged();
            }
        }
        private Category selectedCategory;
        public Category SelectedCategory { get => selectedCategory; set { selectedCategory = value; OnPropertyChanged(); } }

        private ObservableCollection<Bookmark> bookmarks = new ObservableCollection<Bookmark>();
        public ObservableCollection<Bookmark> Bookmarks
        {
            get => bookmarks;
            set
            {
                bookmarks = value;
                OnPropertyChanged();
            }
        }
        private UserControl detailsUserControl = new BrowserUserControl();
        public UserControl DetailsUserControl 
        { 
            get => detailsUserControl; 
            set
            {
                detailsUserControl = value;
                OnPropertyChanged();
            }
        }
        private ChangeUserControlCommand changeUserControlCommand;
        public ChangeUserControlCommand ChangeUserControlCommand 
        {
            get => changeUserControlCommand; 
            set
            {
                changeUserControlCommand = value;
                OnPropertyChanged();
            }
        }
        private Bookmark selectedBookmark;
        public Bookmark SelectedBookmark
        {
            get => selectedBookmark;
            set
            {
                selectedBookmark = value;
                OnPropertyChanged();
            }
        }
        public Category GetParentCategory(Category childCategory)
        {
            foreach(var category in Categories)
            {
                if(category.ChildCategories.Contains(childCategory))
                    return category;
                else
                {
                    var foundCategory = searchParentCategory(category, childCategory);
                    if(foundCategory != null) return foundCategory;
                }
                    
            }
            return null;
        }
        private Category searchParentCategory(Category category, Category childCategory)
        {
            foreach(var cat in category.ChildCategories)
            {
                if (cat.ChildCategories.Contains(childCategory)) return cat;
                else
                {
                    var foundCategory = searchParentCategory(cat, childCategory);
                    if (foundCategory != null) return foundCategory;
                }
            }
            return null;
        }

        public void GetTopCategoriesWithChild()
        {
            categories = new ObservableCollection<Category>(); 
            foreach(var category in ObjectRepository.DataProvider.GetCategories())
            {
                categories.Add(category);
                addChildCategoriesToCategory(category);
            }
        }
        
        private void addChildCategoriesToCategory(Category parentCategory)
        {
            foreach(var category in ObjectRepository.DataProvider.GetCategories(parentCategory.ID))
            {
                parentCategory.ChildCategories.Add(category);
            }
        }
        public void AddCategoriesToChild(Category parentCategory)
        {
            foreach(var category in parentCategory.ChildCategories)
            {
                category.ChildCategories = new ObservableCollection<Category>();
                addChildCategoriesToCategory(category);
            }
        }
        public void SetDefaultView()
        {
            ChangeUserControlCommand.Execute("Browser");
        }
        public MainViewModel() 
        {
            changeUserControlCommand = new ChangeUserControlCommand(this);
            GetTopCategoriesWithChild();
            
        }
        public void GetBookmarks(Category category)
        {
            bookmarks.Clear();
            foreach(var bookmark in ObjectRepository.DataProvider.GetBookmarks(category.ID))
            {
                bookmarks.Add(bookmark);
            }
        }
    }
}
