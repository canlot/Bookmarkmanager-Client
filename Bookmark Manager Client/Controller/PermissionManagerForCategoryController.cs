using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookmark_Manager_Client.Model;

namespace Bookmark_Manager_Client.Controller
{
    class PermissionManagerForCategoryController : PermissionManagerForController
    {
        CategoryController categoryController;

        public PermissionManagerForCategoryController(CategoryController controller) : base(controller)
        {
            this.categoryController = controller;
            loadUp();
        }
        protected override void getAllPermittedUsers()
        {
            if (categoryController.ParentCategory != null)
                foreach (var user in categoryController.ParentCategory.PermissionUsers)
                    categoryController.PermittedUsers.Add(user);
            else if (categoryController.Category != null)
                foreach (var user in categoryController.Category.PermissionUsers)
                    categoryController.PermittedUsers.Add(user);
            else
                categoryController.PermittedUsers.Add(RestCommunicator.GetInstance().CurrentUser);
        }
        protected override void SaveAddedPermissions(ICollection<User> addedPermissions)
        {
            if (RestCommunicator.GetInstance().PostPermission(addedPermissions, categoryController.Category.ID))
            {
                categoryController.Category.AddPermissionUser(addedPermissions);
            }
        }
        protected override void SaveRemovedPermissions(ICollection<User> removedPermissions)
        {
            if (RestCommunicator.GetInstance().RemovePermission(removedPermissions, categoryController.Category.ID))
            {
                categoryController.Category.RemovePermissionUser(removedPermissions);
            }
        }
    }

}
