using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmark_Manager_Client.Utils;

namespace Bookmark_Manager_Client.DataProvider
{
    public class FakeDataProvider : IDataProvider
    {
        private List<Category> categories = new List<Category>();
        private List<Bookmark> bookmarks = new List<Bookmark>();

        private Dictionary<uint, List<User>> categoryUserAssignment = new Dictionary<uint, List<User>>();

        private List<User> users = new List<User>();

        private User currentUser;

        public FakeDataProvider() 
        {
            var adminUser = new User { Name = "Admin", ID = 1, Administrator =  true };
            var jakobUser = new User { Name = "Jakob", ID = 2, Administrator = false };
            var danielUser = new User { Name = "Daniel", ID = 3, Administrator = false };
            var bobUser = new User { Name = "Bob", ID=4, Administrator = false };

            currentUser = adminUser;

            users.Add(adminUser);
            users.Add(jakobUser);
            users.Add(danielUser);
            users.Add(bobUser);

            categories.Add(new Category()
            {
                ID = 1,
                Name = "IT",
                OwnerID = 1,
                ParentID = 0,
            });
            categoryUserAssignment.Add(1, new List<User>() { adminUser});

            categories.Add(new Category()
            {
                ID = 2,
                Name = "Programmieren",
                OwnerID = 1,
                ParentID = 1,
            });
            categoryUserAssignment.Add(2, new List<User>() { adminUser });

            categories.Add(new Category()
            {
                ID = 3,
                Name = "Administration",
                OwnerID = 1,
                ParentID = 1,
            });
            categoryUserAssignment.Add(3, new List<User>() { adminUser });

            categories.Add(new Category()
            {
                ID = 4,
                Name = "C#",
                OwnerID = 1,
                ParentID = 2,
            });
            categoryUserAssignment.Add(4, new List<User>() { adminUser });

            categories.Add(new Category()
            {
                ID = 5,
                Name = "Go",
                OwnerID = 1,
                ParentID = 2,
            });
            categoryUserAssignment.Add(5, new List<User>() { adminUser });

            categories.Add(new Category()
            {
                ID = 6,
                Name = "Elektronik",
                OwnerID = 1,
                ParentID = 0,
                Shared = false,
            });
            categoryUserAssignment.Add(6, new List<User>() { adminUser });

            categories.Add(new Category()
            {
                ID = 7,
                Name = "3D Zeugs",
                OwnerID = 1,
                ParentID = 0,
                Shared = true,
            });
            categoryUserAssignment.Add(7, new List<User>() { adminUser, jakobUser });

            categories.Add(new Category()
            {
                ID = 8,
                Name = "Drucker",
                OwnerID = 1,
                ParentID = 7,
                Shared = true,
            });
            categoryUserAssignment.Add(8, new List<User>() { adminUser, jakobUser });


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

        public User CurrentUser => currentUser;

        public bool DeleteBookmark(Bookmark bookmark)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public IList<Category> GetCategories(uint parentCategoryId = 0)
        {
            var list = new List<Category>();
            foreach (var category in categories) 
            {
                if(category.ParentID == parentCategoryId)
                    list.Add(category);
            }
            return list;
        }

        public IList<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public IList<Bookmark> GetBookmarks(uint id)
        {
            var list = new List<Bookmark>();
            foreach(var bookmark in bookmarks)
            {
                if (bookmark.CategoryID == id)
                    list.Add(bookmark);
            }
            return list;
        }

        public IList<User> GetPermittedUsers(uint categoryId)
        {
            foreach(var categoryIdPair in categoryUserAssignment)
            {
                if(categoryIdPair.Key == categoryId)
                    return categoryIdPair.Value;
            }
            return null;
        }

        public bool PostBookmark(Bookmark bookmark)
        {
            throw new NotImplementedException();
        }

        public bool PostCategory(Category category)
        {
            if(category == null) return false;
            if(category.OwnerID != currentUser.ID) return false;
            if(category.ParentID != 0)
                if(getParentCategoryOwner(category).ID != category.OwnerID) return false;
                
            categories.Add(category);
            categoryUserAssignment.Add(category.ID, new List<User>() { currentUser });
            return true;
        }

        public bool PostPermission(ICollection<User> users, uint categoryId)
        {
            var category = categories.Single(x => x.ID == categoryId);
            if (category.OwnerID != currentUser.ID) return false;
            if (category.ParentID != 0) return false;
            

            if(!addNewUsersToCategory(users, category))
                return false;
            if(!removeUsersFromCategory(users, category)) 
                return false;

            return true;

        }

        private bool addNewUsersToCategory(ICollection<User> incomingUsers,  Category category)
        {
            var categoryPermissionList = categoryUserAssignment[category.ID];

            foreach (var incomingUser in incomingUsers)
            {
                bool userExists = false;
                foreach (var existingUser in categoryPermissionList)
                {
                    if (existingUser.ID == incomingUser.ID)
                    { userExists = true; break; }
                }
                if (!userExists)
                {
                    addPermissionsInChildRecursive(category, incomingUser);
                }

            }
            return true;
        }
        private void addPermissionsInChildRecursive(Category category, User user)
        {
            var categoryPermissionList = categoryUserAssignment[category.ID];
            categoryPermissionList.Add(user);
            category.Shared = true;

            List<Category> childCategories = categories.Where(x => x.ParentID == category.ID).ToList();
            foreach (var childCategory in childCategories)
            {
                addPermissionsInChildRecursive(childCategory, user);
            }
        }
        private bool removeUsersFromCategory(ICollection<User> incomingUsers, Category category)
        {
            var categoryPermissionList = categoryUserAssignment[category.ID];
            foreach(var existingUser in categoryPermissionList)
            {
                bool userExistsInIncomingUsers = false;
                foreach(var incomingUser in incomingUsers)
                {
                    if(incomingUser.ID == existingUser.ID)
                    {
                        userExistsInIncomingUsers = true;
                        break;
                    }
                }
                if(!userExistsInIncomingUsers)
                {
                    if(existingUser.ID == category.OwnerID) //cannot delete permission from owner
                        return false;
                    removePermissionsInChildRecursive(category, existingUser);
                }
            }
            return true;
        }
        private void removePermissionsInChildRecursive(Category category, User user)
        {
            var categoryPermissionList = categoryUserAssignment[category.ID];
            categoryPermissionList.Remove(user);
            if (categoryPermissionList.Count < 2)
                category.Shared = false;

            List<Category> childCategories = categories.Where(x => x.ParentID == category.ID).ToList();
            foreach (var childCategory in childCategories)
            {
                removePermissionsInChildRecursive(childCategory, user);
            }
        }

        private User getParentCategoryOwner(Category category)
        {
            var parentCategory = categories.Single(x => x.ID == category.ParentID);
            return users.Single(x => x.ID == parentCategory.OwnerID);
        }

        public bool PutBookmark(Bookmark bookmark)
        {
            throw new NotImplementedException();
        }

        public bool PutCategory(Category category)
        {
            Category cat = categories.Single(x => x.ID == category.ID);
            if(category.OwnerID != currentUser.ID) return false;
            if(category.OwnerID != cat.OwnerID) return false;
            if(category.ParentID != cat.ParentID) return false;
            if(cat == null) return false;

            cat = category;
            return true;
        }


        public bool RemovePermission(ICollection<User> users, uint id)
        {
            throw new NotImplementedException();
        }

        public IList<User> SearchUser(string username)
        {
            var foundUsers = new ObservableCollection<User>();
            foreach(var user in users)
            {
                if(user.Name.ToLower().Contains(username.ToLower()))
                    foundUsers.Add(user);
            }
            return foundUsers;
        }

        public void SetUpConnection()
        {
            
        }
    }
}
