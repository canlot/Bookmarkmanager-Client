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
        private ObservableCollection<Bookmark> bookmarks = new ObservableCollection<Bookmark>();
        
        public FakeDataProvider() 
        {
            categories.Add(new Category()
            {
                ID = 1,
                Name = "IT",
                OwnerID = 1,
                ParentID = 0,
                ChildCategories = new ObservableCollection<Category>()
                {
                    new Category() 
                    {
                        ID = 2,
                        Name = "Programmieren",
                        OwnerID = 1,
                        ParentID = 1,
                        ChildCategories = new ObservableCollection<Category>()
                        {
                            new Category()
                            {
                                ID = 4,
                                Name = "C#",
                                OwnerID = 1,
                                ParentID = 2,
                            },
                            new Category()
                            {
                                ID = 5,
                                Name = "Go",
                                OwnerID = 1,
                                ParentID = 2,
                            }
                            
                        }
                    },
                    new Category() 
                    {
                        ID = 3,
                        Name = "Administration",
                        OwnerID = 1,
                        ParentID = 1,
                    }
                }
            });
            bookmarks.Add(new Bookmark()
            {
                ID = 1,
                CategoryID = 2,
                Url = "www.mok-test.com"
            });
            bookmarks.Add(new Bookmark()
            {
                ID = 2,
                CategoryID = 4,
                Url = "https://learn.microsoft.com/de-de/dotnet/desktop/wpf/data/?view=netdesktop-7.0",
            });
        }

        public User CurrentUser => throw new NotImplementedException();

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

        public IEnumerable<Bookmark> GetBookmarks(uint id)
        {
            foreach(var bookmark in bookmarks)
            {
                if (bookmark.CategoryID == id)
                    yield return bookmark;
            }
        }

        public ObservableCollection<Category> GetCategories(uint id)
        {
            return categories;
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
            
        }
    }
}
