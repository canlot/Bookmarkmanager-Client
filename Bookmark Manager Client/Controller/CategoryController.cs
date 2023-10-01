using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmark_Manager_Client.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Bookmark_Manager_Client.Controller
{
    public class CategoryController : INotifyPropertyChanged, IPermitableObjectController
    {
        //---------------------Properties---------------------------//
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private ObservableCollection<User> allUsers;
        public ObservableCollection<User> AllUsers { get => allUsers;
            set
            {
                allUsers = value;
                OnPropertyChanged("AllUsers");
            }
        }
        private ObservableCollection<User> permittedUsers;
        public ObservableCollection<User> PermittedUsers { get => permittedUsers;
            set
            {
                permittedUsers = value;
                OnPropertyChanged("PermittedUsers");
            }
        }
        private Category parentCategory;
        public Category ParentCategory
        {
            get => parentCategory;
            set
            {
                parentCategory = value;
                if (value != null)
                    ParentCategoryName = value.Name;
                else
                    ParentCategoryName = "";
            }
        }
        private Category category;
        public Category Category { get => category; set => category = value; }
        public ObservableCollection<Category> Categories { get; set; }

        private string categoryName;
        public string CategoryName { get => categoryName;
            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }
        private string parentCategoryName;
        public string ParentCategoryName { get => parentCategoryName;
            set
            {
                parentCategoryName = value;
                OnPropertyChanged("ParentCategoryName");
            }
        }
        //---------------------Properties---------------------------//
        private PermissionManagerForCategoryController permissionManager;
        public CategoryController()
        {
            AllUsers = new ObservableCollection<User>();
            permittedUsers = new ObservableCollection<User>();
        }
        public CategoryController(ObservableCollection<Category> categories, Category parentCategory = null, Category currentCategory = null) : this()
        {
            this.Categories = categories;
            if (parentCategory != null)
            {
                this.parentCategory = parentCategory;
                this.ParentCategoryName = parentCategory.Name;
            }
            else if (currentCategory != null)
            {
                this.category = currentCategory;
                this.CategoryName = this.category.Name;
            }
            permissionManager = new PermissionManagerForCategoryController(this);
        }
        public void AddUserToPermittedUsers(User user) => permissionManager.AddUserToPermittedUsers(user);
        public void RemoveUserFromPermittedUsers(User user) => permissionManager.RemoveUserFromPermittedUsers(user);

        public void ModifyPermissions() => permissionManager.SavePermissions();
        public void CreateNewCategory()
        {
            if (CategoryName == "")
                return;

            if(parentCategory != null)
                category = new Category() { Name = CategoryName, ParentID = parentCategory.ID };
            else
                category = new Category() { Name = CategoryName };

            if (!RestCommunicator.GetInstance().PostCategory(category))
                return;

            if(parentCategory != null)
                parentCategory.ChildCategories.Add(category);
            else
                Categories.Add(category);

            ModifyPermissions();
        }
        public void ModifyCategory()
        {
            if (CategoryName == "")
                return;

            if (CategoryName != category.Name)
            {
                category.Name = CategoryName;

                if (!RestCommunicator.GetInstance().PutCategory(category))
                    return;
            }
            ModifyPermissions();
        }
    }
}
