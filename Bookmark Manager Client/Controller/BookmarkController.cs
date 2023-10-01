using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Bookmark_Manager_Client.Model;
using Bookmark_Manager_Client.Controller;
namespace Bookmark_Manager_Client.Controller
{
    class BookmarkController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string url;
        public string Url
        {
            get => url;
            set
            {
                url = value;
                OnPropertyChanged(Url);
            }
        }
        private string categoryName;
        public string CategoryName
        {
            get => categoryName;
            set
            {
                categoryName = value;
                OnPropertyChanged(CategoryName);
            }
        }
        private Category category;

        private Bookmark bookmark;
        public BookmarkController(Category category)
        {
            this.category = category;
            CategoryName = category.Name;
        }
        public BookmarkController(Category category, Bookmark bookmark = null) : this(category)
        {
            this.bookmark = bookmark;
            Url = bookmark.Url;
        }
        public bool CreateNewBookmark()
        {
            if (Url == "")
                return false;
            bookmark = new Bookmark() { CategoryID = category.ID, Url = Url };

            if (!RestCommunicator.GetInstance().PostBookmark(bookmark))
                return false;

            category.Bookmarks.Add(bookmark);
            return true;
        }

        internal void ModifyBookmark()
        {
            throw new NotImplementedException();
        }
    }
}
