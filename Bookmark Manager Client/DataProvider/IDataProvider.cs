using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.DataProvider
{
    public interface IDataProvider
    {
        void SetUpConnection();
        User CurrentUser { get; }
        IList<Category> GetCategories(uint id = 0);
        //IEnumerable<Category> GetAllCategories();
        IList<Bookmark> GetBookmarks(uint id);
        IList<User> GetPermittedUsers(uint id);
        IList<User> GetAllUsers();
        IList<User> SearchUser(string username);
        bool PostCategory(Category category);
        bool PutCategory(Category category);
        bool DeleteCategory(Category category);
        bool PostBookmark(Bookmark bookmark);
        bool PutBookmark(Bookmark bookmark);
        bool DeleteBookmark(Bookmark bookmark);
        bool PostPermission(ICollection<User> users, uint id);
        bool RemovePermission(ICollection<User> users, uint id);
    }
}
