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
            var adminUser = new User { Name = "Admin", ID = 1, Administrator =  true };
            var user = new User { Name = "Jakob", ID = 2, Administrator = false };


            categories.Add(new Category()
            {
                ID = 1,
                Name = "IT",
                OwnerID = 1,
                ParentID = 0,
                PermissionUsers = new ObservableCollection<User>() { adminUser },
                ChildCategories = new ObservableCollection<Category>()
                {
                    new Category()
                    {
                        ID = 2,
                        Name = "Programmieren",
                        OwnerID = 1,
                        ParentID = 1,
                        PermissionUsers = new ObservableCollection<User>() { adminUser },
                        ChildCategories = new ObservableCollection<Category>()
                        {
                            new Category()
                            {
                                ID = 4,
                                Name = "C#",
                                OwnerID = 1,
                                ParentID = 2,
                                PermissionUsers = new ObservableCollection<User>() { adminUser },
                            },
                            new Category()
                            {
                                ID = 5,
                                Name = "Go",
                                OwnerID = 1,
                                ParentID = 2,
                                PermissionUsers = new ObservableCollection<User>() { adminUser },
                            }
                        }
                    },
                    new Category()
                    {
                        ID = 3,
                        Name = "Administration",
                        OwnerID = 1,
                        ParentID = 1,
                        PermissionUsers = new ObservableCollection<User>() { adminUser },
                    }
                }
            }) ;

            categories.Add(new Category()
            {
                ID = 6,
                Name = "Elektronik",
                OwnerID = 1,
                ParentID = 0,
                Shared = false,
                PermissionUsers = new ObservableCollection<User>() { adminUser }
            });

            categories.Add(new Category()
            {
                ID = 7,
                Name = "3D Zeugs",
                OwnerID = 1,
                ParentID = 0,
                Shared = true,
                PermissionUsers = new ObservableCollection<User>() { adminUser, user },
                ChildCategories = new ObservableCollection<Category>()
                {
                    new Category()
                    {
                        ID = 8,
                        Name = "Drucker",
                        OwnerID = 1,
                        ParentID = 1,
                        Shared = true,
                        PermissionUsers = new ObservableCollection<User>() { adminUser, user }
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
            bookmarks.Add(new Bookmark()
            {
                ID = 3,
                CategoryID = 3,
                Url = "https://administrator.de",
            });
            bookmarks.Add(new Bookmark()
            {
                ID = 4,
                CategoryID = 8,
                Url = "https://eu.store.bambulab.com/de/products/p1s?variant=47016782397788&gclid=EAIaIQobChMIh4eGoOmGggMV4ItoCR2zOADhEAYYCCABEgJdjvD_BwE"
            });
            bookmarks.Add(new Bookmark()
            {
                ID = 5,
                CategoryID = 8,
                Url = "https://eu.store.bambulab.com/de/products/x1-carbon-combo?utm_campaign=sag_organic&utm_content=sag_organic&utm_medium=product_sync&utm_source=google&variant=47035962851676&gclid=EAIaIQobChMIh4eGoOmGggMV4ItoCR2zOADhEAYYBCABEgIjOfD_BwE"
            });
            bookmarks.Add(new Bookmark()
            {
                ID = 6,
                CategoryID = 8,
                Url = "https://store.creality.com/de/products/ender-3-v2-3d-printer?utm_source=googleshopping&gclid=EAIaIQobChMI5P6Q2eqGggMVow8GAB1JmwGqEAYYASABEgKScvD_BwE"
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

        public ObservableCollection<Category> GetAllCategories()
        {
            return categories;
        }

        public ObservableCollection<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Bookmark> GetBookmarks(uint id)
        {
            ObservableCollection<Bookmark> requestedBookmarks = new ObservableCollection<Bookmark>();
            foreach(var bookmark in bookmarks)
            {
                if (bookmark.CategoryID == id)
                    requestedBookmarks.Add(bookmark);
            }
            return requestedBookmarks;
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
