using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Bookmark_Manager_Client.Model;

namespace Bookmark_Manager_Client.Controller
{
    abstract class PermissionManagerForController
    {
        protected ObservableCollection<User> originalPermittedUsers = new ObservableCollection<User>();
        protected IPermitableObjectController controller;
        public PermissionManagerForController(IPermitableObjectController controller)
        {
            this.controller = controller;
        }
        protected void loadUp()
        {
            getAllPermittedUsers();
            copyOriginalPermittedUsers();
            GetAllUsers();
        }
        private void copyOriginalPermittedUsers()
        {
            if (controller.PermittedUsers == null)
                return;
            foreach(var user in controller.PermittedUsers)
                originalPermittedUsers.Add(user);
        }
        abstract protected void getAllPermittedUsers();
        
        public void GetAllUsers()
        {
            ObservableCollection<User> users = RestCommunicator.GetInstance().GetAllUsers();
            if(users == null)
                return;
            foreach (var user in users)
            {
                bool userExists = false;
                foreach (var ouser in originalPermittedUsers)
                {
                    //Console.WriteLine($"Original User {ouser.Name}, {ouser.ID} == Retrieved User {user.Name}, {user.ID}");
                    if (user == ouser)
                    {
                        userExists = true;
                        break;
                    }
                }
                if (!userExists)
                    controller.AllUsers.Add(user);
            }
        }
        public void AddUserToPermittedUsers(User user)
        {
            controller.PermittedUsers.Add(user);
            controller.AllUsers.Remove(user);
        }
        public void RemoveUserFromPermittedUsers(User user)
        {
            if (user == RestCommunicator.GetInstance().CurrentUser)
                return;
            controller.AllUsers.Add(user);
            controller.PermittedUsers.Remove(user);
        }
        private List<User> GetAddedPermissions()
        {
            List<User> addedPermissions = new List<User>();
            foreach (var user in controller.PermittedUsers)
            {
                bool addedUser = true;
                foreach (var ouser in originalPermittedUsers)
                    if (user == ouser)
                        addedUser = false;
                if (addedUser)
                    addedPermissions.Add(user);
            }
            return addedPermissions;
        }
        private List<User> GetRemovedPermissions()
        {
            List<User> removedPermissions = new List<User>();
            foreach (var ouser in originalPermittedUsers)
            {
                bool removedUser = true;
                foreach (var user in controller.PermittedUsers)
                    if (user == ouser)
                        removedUser = false;
                if (removedUser)
                    removedPermissions.Add(ouser);
            }
            return removedPermissions;
        }
        public void SavePermissions()
        {
            SaveAddedPermissions(GetAddedPermissions());
            SaveRemovedPermissions(GetRemovedPermissions());
        }
        abstract protected void SaveAddedPermissions(ICollection<User> addedPermissions);
        abstract protected void SaveRemovedPermissions(ICollection<User> removedPermissions);
    }
}
