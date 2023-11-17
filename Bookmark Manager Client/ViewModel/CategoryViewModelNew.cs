using Bookmark_Manager_Client.Model;
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
        private bool isTopCategory;
        public bool IsTopCategory
        {
            get => isTopCategory;
            private set
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
            
        }
    }
}
