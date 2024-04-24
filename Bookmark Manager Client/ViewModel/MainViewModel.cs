using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Bookmark_Manager_Client.Commands;
using Bookmark_Manager_Client.Controller;
using Bookmark_Manager_Client.Model;
using Bookmark_Manager_Client.UserControls;
using Bookmark_Manager_Client.Utils;

namespace Bookmark_Manager_Client.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        object categorylock = new object ();
        object bookmarklock = new object ();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


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
        private UserControl detailsUserControl;
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
                ChangeUserControlCommand.Execute("BrowserUserControl");
                OnPropertyChanged();
            }
        }
        private List<Bookmark> selectedBookmarksCopySource = new List<Bookmark>();
        public List<Bookmark> SelectedBookmarksCopySource
        {
            get { return selectedBookmarksCopySource; }
            set { selectedBookmarksCopySource = value;}
        }
        private Category categoryCopySource;
        public Category CategoryCopySource
        {
            get { return categoryCopySource; }
            set { categoryCopySource = value;}
        }
        private Category categoryCopyDestination;
        public Category CategoryCopyDestination
        {
            get { return categoryCopyDestination; }
            set { categoryCopyDestination = value;}
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

        public async Task GetTopCategoriesWithChildAsync()
        {
            lock(categorylock) Categories.Clear();
            lock(bookmarklock) Bookmarks.Clear();
            var categoryList = await ObjectRepository.DataProvider.GetCategoriesAsync();
            if (categoryList == null) return;
            foreach (var category in categoryList)
            {
                lock(categorylock) Categories.Add(category);
                await addChildCategoriesToCategoryAsync(category);
            }
        }
        
        private async Task addChildCategoriesToCategoryAsync(Category parentCategory)
        {
            var categoryList = await ObjectRepository.DataProvider.GetCategoriesAsync(parentCategory.ID);
            if (categoryList == null) return;
            foreach (var category in categoryList)
            {
                if(!parentCategory.ChildCategories.Contains(category))
                    lock(categorylock)
                        parentCategory.ChildCategories.Add(category);
            }
        }
        public async Task AddCategoriesToChildAsync(Category parentCategory)
        {
            foreach(var category in parentCategory.ChildCategories)
            {
                lock(categorylock)
                    category.ChildCategories = new ObservableCollection<Category>();
                await addChildCategoriesToCategoryAsync(category);
            }
        }
        public void SetDefaultView()
        {
            ChangeUserControlCommand.Execute("Browser");
        }
        public MainViewModel() 
        {
            BindingOperations.EnableCollectionSynchronization(Categories, categorylock);
            BindingOperations.EnableCollectionSynchronization(Bookmarks, bookmarklock);
            changeUserControlCommand = new ChangeUserControlCommand(this);
            Task.Run(async () => await GetTopCategoriesWithChildAsync());
            
        }
        public async Task GetBookmarksAsync(Category category)
        {
            lock(bookmarklock) Bookmarks.Clear();
            foreach(var bookmark in await ObjectRepository.DataProvider.GetBookmarksAsync(category.ID))
            {
                lock(bookmarklock) Bookmarks.Add(bookmark);
            }
        }
        public async Task SearchForCategoriesAsync(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return;
            lock(categorylock) Categories.Clear();
            var categoryList = await ObjectRepository.DataProvider.SearchCategoriesAsync(searchString);
            if (categoryList == null) return;
            foreach(var category in categoryList)
                lock(categorylock) Categories.Add(category);
        }
        public async Task SearchForBookmarksAsync(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return;
            lock(bookmarklock) Bookmarks.Clear();
            var bookmarkList = await ObjectRepository.DataProvider.SearchBookmarksAsync(searchString);
            if (bookmarkList == null) return;
            foreach (var bookmark in bookmarkList)
                lock(bookmarklock) Bookmarks.Add(bookmark);
        }
        public async Task MoveBookmarksAsync()
        {
            try
            {
                if (SelectedBookmarksCopySource.Count == 0) return;
                if (CategoryCopySource == null) return;
                if (CategoryCopyDestination == null) return;
                if (CategoryCopySource == CategoryCopyDestination) return;

                await ObjectRepository.DataProvider.MoveBookmarksAsync(CategoryCopySource, CategoryCopyDestination, SelectedBookmarksCopySource);
                
                if (CategoryCopySource == SelectedCategory)
                {
                    foreach(var bookmark in SelectedBookmarksCopySource)
                    {
                        Bookmarks.Remove(bookmark);
                    }
                }
                if (CategoryCopyDestination == SelectedCategory)
                {
                    foreach(var bookmark in SelectedBookmarksCopySource)
                    {
                        Bookmarks.Add(bookmark);
                    }
                }
            }
            catch (Exception ex) 
            {

            }
        }
    }
}
