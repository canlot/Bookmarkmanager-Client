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
        Task<bool> LoginAsync();
        User CurrentUser { get; }
        Task<IList<Category>> GetCategoriesAsync(uint id = 0);

        Task<IList<Bookmark>> GetBookmarksAsync(uint id);
        Task<IList<User>> GetPermittedUsersAsync(uint id);
        Task<IList<User>> GetAllUsersAsync();
        Task<IList<User>> SearchUsersAsync(string username);
        Task<IList<Category>> SearchCategoriesAsync(string searchString);
        Task<IList<Bookmark>> SearchBookmarksAsync(string searchString);
        Task<bool> PostCategoryAsync(Category category);
        Task<bool> PutCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(uint categoryId);
        Task<bool> PostBookmarkAsync(Bookmark bookmark);
        Task<bool> PutBookmarkAsync(Bookmark bookmark);
        Task<bool> DeleteBookmarkAsync(Bookmark bookmark);
        Task <bool> ChangePermissionsAsync(ICollection<User> users, uint id);
    }
}
