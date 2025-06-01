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
    public abstract class CategoryViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected IconElement icon;
        public IconElement Icon { get { return icon; } set { icon = value; OnPropertyChanged(); } }

        protected string iconName;
        public string IconName { get { return iconName; } set { iconName = value; OnPropertyChanged(); } }


        protected ObservableCollection<IconElement> icons = new ObservableCollection<IconElement>();
        public ObservableCollection<IconElement> Icons { get { return icons; } set { icons = value; OnPropertyChanged(); } }

        private ObservableCollection<User> permittedUsers = new ObservableCollection<User>();
        public ObservableCollection<User> PermittedUsers { get => permittedUsers; set { permittedUsers = value; OnPropertyChanged(); } }

        private string categoryName;
        public string CategoryName { get => categoryName; set { categoryName = value; OnPropertyChanged(); } }


        private string categoryDescription;
        public string CategoryDescription { get => categoryDescription; set { categoryDescription = value; OnPropertyChanged(); } }

        public ICollectionView IconsView { get; set; }

        private string searchIconString;
        public string SearchIconString { get { return searchIconString; } set { searchIconString = value; IconsView.Refresh(); OnPropertyChanged(); } }

        protected bool iconFilter(object element)
        {
            if (element is null)
                return false;
            if (string.IsNullOrEmpty(SearchIconString))
                return true;
            if (element is IconElement)
            {
                var icon = (IconElement)element;
                if (icon.IconTitle.ToLower().Contains(SearchIconString.ToLower()))
                    return true;
                return false;
            }
            return false;
        }

        protected void selectIcon()
        {
            string name;

            if (string.IsNullOrEmpty(IconName)) name = "message_circle_question_mark";
            else name = IconName;

            foreach (var icon in icons)
            {
                if (icon != null && icon.IconTitle == name)
                {
                    Icon = icon;
                }
            }
        }
        public void AddPermittedUser(User user)
        {
            if (PermittedUsers.IndexOf(user) == -1)
            {
                PermittedUsers.Add(user);
            }
        }
    }
}
