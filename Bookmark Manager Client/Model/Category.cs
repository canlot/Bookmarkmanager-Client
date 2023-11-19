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
                shared = value;
                OnProperyChanged("Shared");
            }
        }
        private bool isExpanded;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                OnProperyChanged("IsExpanded");
                if(value == true)
                {
                    foreach (var category in ChildCategories)
                    {
                        category.ChildCategories = new ObservableCollection<Category>();
                        foreach(var cat in ObjectRepository.DataProvider.GetCategories(category.ID))
                        {
                            category.ChildCategories.Add(cat);
                        }
                    }
                }
                
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
            //childCategories.CollectionChanged += OnNewChildCategoryItems;
        }

        public void GetChildCategories()
        {
            if (childCategories.Count > 0)
                return;
            var cat = ObjectRepository.DataProvider.GetCategories();
            foreach (var item in cat)
                childCategories.Add(item);
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
