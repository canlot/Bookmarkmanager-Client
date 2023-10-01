using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.Communicator
{
    public interface ICommunicator
    {
        void SetUpConnection();
        ObservableCollection<Category> GetCategories(uint id);
        ObservableCollection<Bookmark> GetBookmarks(uint id);
        ObservableCollection<User> GetPermittedUsers(uint id);
        ObservableCollection<User> GetAllUsers();
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
