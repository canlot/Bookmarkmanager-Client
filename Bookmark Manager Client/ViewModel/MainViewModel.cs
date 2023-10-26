using System;
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
        private ChangeUserControlCommand changeUserControlCommand = new ChangeUserControlCommand();
        public ChangeUserControlCommand ChangeUserControlCommand 
        {
            get => changeUserControlCommand; 
            set
            {
                changeUserControlCommand = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() 
        {
            Categories = ObjectRepository.DataProvider.GetAllCategories();
        }
        public void GetBookmarks(Category category)
        {
            Bookmarks = ObjectRepository.DataProvider.GetBookmarks(category.ID);
        }
    }
}
