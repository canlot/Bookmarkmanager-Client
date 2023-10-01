using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Bookmark_Manager_Client.Controller;
using Bookmark_Manager_Client.DataProvider;

namespace Bookmark_Manager_Client.Model
{
    public class Category : INotifyPropertyChanged
    {
        private uint id;
        public uint ID 
        {
            get => id;
            set
            {
                id = value;
                OnProperyChanged("ID");
            }
        }
        private uint ownerID;
        public uint OwnerID
        {
            get => ownerID;
            set
            {
                ownerID = value;
                OnProperyChanged("OwnerID");
            }
        }
        private uint parentID;
        public uint ParentID
        {
            get => parentID;
            set
            {
                parentID = value;
                OnProperyChanged("ParentID");
            }
        }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnProperyChanged("Name");
            }
        }
        private bool shared;
        public bool Shared
        {
            get => shared;
            set
            {
                shared = Shared;
                OnProperyChanged("Shared");
            }
        }
        private ObservableCollection<Category> childCategories;
        public ObservableCollection<Category> ChildCategories
        {
            get => childCategories;
            set
            {
                childCategories = value;
                OnProperyChanged("ChildCategories");
            }
        }
        public Category()
        {
            childCategories = new ObservableCollection<Category>();
            bookmarks = new ObservableCollection<Bookmark>();
            permissionUsers = new ObservableCollection<User>();
            childCategories.CollectionChanged += OnNewChildCategoryItems;
        }
        private ObservableCollection<Bookmark> bookmarks;
        public ObservableCollection<Bookmark> Bookmarks
        {
            get => bookmarks;
            set => bookmarks = value;
        }
        private ObservableCollection<User> permissionUsers;
        public ObservableCollection<User> PermissionUsers
        {
            get => permissionUsers;
            set => permissionUsers = value;
        }
        public void GetChildCategories()
        {
            if (childCategories.Count > 0)
                return;
            var cat = ObjectRepository.DataProvider.GetCategories(ID);
            foreach (var item in cat)
                childCategories.Add(item);
        }
        public void GetBookmarks()
        {
            if (bookmarks.Count > 0)
                return;
            var bm = ObjectRepository.DataProvider.GetBookmarks(ID);
            if(bm != null)
                foreach (var item in bm)
                    bookmarks.Add(item);
        }
        public void GetPermissionUsers()
        {
            var users = ObjectRepository.DataProvider.GetPermittedUsers(ID);
            permissionUsers.Clear();
            if(users != null)
                foreach (var user in users)
                    permissionUsers.Add(user);
        }
        public void AddPermissionUser(ICollection<User> users)
        {
            foreach(var user in users)
            {
                var exists = false;
                foreach (var ouser in permissionUsers)
                {
                    if (user == ouser)
                        exists = true;
                }
                if(!exists)
                    permissionUsers.Add(user);
            }
                

        }
        public void RemovePermissionUser(ICollection<User> users)
        {
            foreach(var user in users)
            {
                PermissionUsers.Remove(user);
            }
        }
        private void OnNewChildCategoryItems(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Hallo");
            if (e.NewItems != null)
            {
                foreach (Category item in e.NewItems)
                {
                    item.GetChildCategories();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnProperyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
