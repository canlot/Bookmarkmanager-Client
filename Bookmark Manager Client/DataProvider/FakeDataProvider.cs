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

        private uint categoryId = 0;
        private uint bookmarkId = 0;

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


            var itCategory = new Category()
            {
                ID = getCategoryIdAndIncrementByOne(),
                Name = "IT",
                Description = "Hobby zum Beruf",
                OwnerID = adminUser.ID,
                ParentID = 0,
            };

            categories.Add(itCategory);
            categoryUserAssignment.Add(itCategory.ID, new List<User>() { adminUser}); // be aware that multiple categories could have the same name, but it is sufficuent for this case right now

            var programmingCategory = new Category()
            {
                ID = getCategoryIdAndIncrementByOne(),
                Name = "Programmieren",
                OwnerID = itCategory.OwnerID,
                ParentID = itCategory.ID,
            };

            categories.Add(programmingCategory);
            categoryUserAssignment.Add(programmingCategory.ID, new List<User>() { adminUser });

            var administrationCategory = new Category()
            {
                ID = getCategoryIdAndIncrementByOne(),
                Name = "Administration",
                Description = "Alles rund um Administration",
                OwnerID = itCategory.OwnerID,
                ParentID = itCategory.ID,
            };

            categories.Add(administrationCategory);
            categoryUserAssignment.Add(administrationCategory.ID, new List<User>() { adminUser });

            var cSharpCategory = new Category()
            {
                ID = getCategoryIdAndIncrementByOne(),
                Name = "C#",
                Description = "Eine der besten Sprachen die es gibt",
                OwnerID = itCategory.OwnerID,
                ParentID = programmingCategory.ID,
            };

            categories.Add(cSharpCategory);
            categoryUserAssignment.Add(cSharpCategory.ID, new List<User>() { adminUser });

            var goCategory = new Category()
            {
                ID = getCategoryIdAndIncrementByOne(),
                Name = "Go",
                OwnerID = itCategory.OwnerID,
                ParentID = programmingCategory.ID,
            };

            categories.Add(goCategory);
            categoryUserAssignment.Add(goCategory.ID, new List<User>() { adminUser });

            var electronicsCategory = new Category()
            {
                ID = getCategoryIdAndIncrementByOne(),
                Name = "Elektronik",
                OwnerID = adminUser.ID,
                ParentID = 0,
                Shared = false,
            };

            categories.Add(electronicsCategory);
            categoryUserAssignment.Add(electronicsCategory.ID, new List<User>() { adminUser });

            var threeDStuffCategory = new Category()
            {
                ID = getCategoryIdAndIncrementByOne(),
                Name = "3D Zeugs",
                OwnerID = adminUser.ID,
                ParentID = 0,
                Shared = true,
            };

            categories.Add(threeDStuffCategory);
            categoryUserAssignment.Add(threeDStuffCategory.ID, new List<User>() { adminUser, jakobUser });

            var printerCategory = new Category()
            {
                ID = getCategoryIdAndIncrementByOne(),
                Name = "Drucker",
                OwnerID = adminUser.ID,
                ParentID = threeDStuffCategory.ID,
                Shared = threeDStuffCategory.Shared,
            };

            categories.Add(printerCategory);
            categoryUserAssignment.Add(printerCategory.ID, new List<User>() { adminUser, jakobUser });


            bookmarks.Add(new Bookmark()
            {
                ID = getBookmarkIdAndIncrementByOne(),
                CategoryID = programmingCategory.ID,
                Url = "www.mok-test.com"
            });
            bookmarks.Add(new Bookmark()
            {
                ID = getBookmarkIdAndIncrementByOne(),
                CategoryID = cSharpCategory.ID,
                Url = "https://learn.microsoft.com/de-de/dotnet/desktop/wpf/data/?view=netdesktop-7.0",
            });
            bookmarks.Add(new Bookmark()
            {
                ID = getBookmarkIdAndIncrementByOne(),
                CategoryID = administrationCategory.ID,
                Url = "https://administrator.de",
            });
            bookmarks.Add(new Bookmark()
            {
                ID = getBookmarkIdAndIncrementByOne(),
                CategoryID = printerCategory.ID,
                Url = "https://eu.store.bambulab.com/de/products/p1s?variant=47016782397788&gclid=EAIaIQobChMIh4eGoOmGggMV4ItoCR2zOADhEAYYCCABEgJdjvD_BwE"
            });
            bookmarks.Add(new Bookmark()
            {
                ID = getBookmarkIdAndIncrementByOne(),
                CategoryID = printerCategory.ID,
                Url = "https://eu.store.bambulab.com/de/products/x1-carbon-combo?utm_campaign=sag_organic&utm_content=sag_organic&utm_medium=product_sync&utm_source=google&variant=47035962851676&gclid=EAIaIQobChMIh4eGoOmGggMV4ItoCR2zOADhEAYYBCABEgIjOfD_BwE"
            });
            bookmarks.Add(new Bookmark()
            {
                ID = getBookmarkIdAndIncrementByOne(),
                CategoryID = printerCategory.ID,
                Url = "https://store.creality.com/de/products/ender-3-v2-3d-printer?utm_source=googleshopping&gclid=EAIaIQobChMI5P6Q2eqGggMVow8GAB1JmwGqEAYYASABEgKScvD_BwE"
            });
        }

        private uint getCategoryIdAndIncrementByOne() =>  ++categoryId;
        

        private uint getBookmarkIdAndIncrementByOne() => ++bookmarkId;
        

        public User CurrentUser => currentUser;

        public bool DeleteBookmark(Bookmark bookmark)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCategory(uint categoryID)
        {
            var category = categories.Single(x => x.ID == categoryID);
            if(category.OwnerID != CurrentUser.ID) return false;
            if(category.ID == 0) return false;

            if(!deleteChildCategories(category))
                return false;
            return true;
        }



        private bool deleteChildCategories(Category category)
        {
            var childCategories = categories.Where(x => x.ParentID == category.ID).ToList();
            foreach (Category childCategory in childCategories)
            {
                if (!deleteChildCategories(childCategory))
                    return false;
            }
            categoryUserAssignment.Remove(category.ID);
            var cat = categories.Single(x => x.ID == category.ID);
            categories.Remove(cat);
            return true;
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
            if(bookmark.CategoryID == 0) return false;
            if(getCategoryOwnerById(bookmark.CategoryID).ID != CurrentUser.ID) return false;

            bookmarks.Add(bookmark);
            bookmark.ID = getBookmarkIdAndIncrementByOne();

            return true;
        }

        public bool PostCategory(Category category) // should maybe return category id because it will set here, but because it is the same object it does not matter
        {
            if(category == null) throw new Exception("No category");
            if(category.OwnerID == 0) category.OwnerID = currentUser.ID;
            if(category.OwnerID != currentUser.ID) throw new Exception("Category owner is not the same as the loged in user");
            if(category.ParentID != 0) 
                if(getParentCategoryOwner(category).ID != category.OwnerID) throw new Exception("Parent Category has different owner");

            category.ID = getCategoryIdAndIncrementByOne();
            categories.Add(category);
            if(category.ParentID == 0)
                categoryUserAssignment.Add(category.ID, new List<User>() { currentUser });
            else
            {
                var users = new List<User>();
                foreach (var user in categoryUserAssignment[category.ParentID])
                    users.Add(user);

                categoryUserAssignment.Add(category.ID, users);
                if(users.Count > 1)
                    category.Shared = true;
            }
                
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

        private User getCategoryOwnerById(uint categoryID)
        {
            var category = categories.Single(x => x.ID == categoryID);
            return getCategoryOwner(category);
        }

        private User getCategoryOwner(Category category)
        {
            return users.Single(x => x.ID == category.OwnerID);
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
