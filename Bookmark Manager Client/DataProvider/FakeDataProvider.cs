using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.DataProvider
{
    public class FakeDataProvider : IDataProvider
    {
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        
        public FakeDataProvider() { }

        public bool DeleteBookmark(Bookmark bookmark)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Bookmark> GetBookmarks(uint id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Category> GetCategories(uint id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<User> GetPermittedUsers(uint id)
        {
            throw new NotImplementedException();
        }

        public bool PostBookmark(Bookmark bookmark)
        {
            throw new NotImplementedException();
        }

        public bool PostCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public bool PostPermission(ICollection<User> users, uint id)
        {
            throw new NotImplementedException();
        }

        public bool PutBookmark(Bookmark bookmark)
        {
            throw new NotImplementedException();
        }

        public bool PutCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public bool RemovePermission(ICollection<User> users, uint id)
        {
            throw new NotImplementedException();
        }

        public void SetUpConnection()
        {
            throw new NotImplementedException();
        }
    }
}
