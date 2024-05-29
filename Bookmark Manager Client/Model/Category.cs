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
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace Bookmark_Manager_Client.Model
{
    public class Category : INotifyPropertyChanged
    {
        public object ChildCategorylock = new object();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private uint id;
        public uint ID { get => id; set { id = value; OnPropertyChanged(); } }

        private uint ownerID;
        public uint OwnerID { get => ownerID; set { ownerID = value; OnPropertyChanged(); } }

        private uint parentID;
        public uint ParentID { get => parentID; set { parentID = value; OnPropertyChanged(); } }

        private string name;
        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        private string description;
        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        private bool shared;
        public bool Shared { get => shared; set { shared = value; OnPropertyChanged(); } }

        private bool isExpanded;
        public bool IsExpanded 
        { 
            get => isExpanded;
            set
            {
                isExpanded = value;
                OnPropertyChanged();
                if(value == true)
                {
                    getChildCategoriesForThisCategory();
                }
                
            }

        }

        private ObservableCollection<Category> childCategories;
        public ObservableCollection<Category> ChildCategories { get => childCategories; set { childCategories = value; OnPropertyChanged(); } }
        public Category()
        {
            childCategories = new ObservableCollection<Category>();
            BindingOperations.EnableCollectionSynchronization(ChildCategories, ChildCategorylock);
            //childCategories.CollectionChanged += OnNewChildCategoryItems;
        }
        private void getChildCategoriesForThisCategory()
        {
            Task.Run(async () =>
            {
                foreach (var category in ChildCategories)
                {
                    category.ChildCategories = new ObservableCollection<Category>();
                    foreach (var cat in await ObjectRepository.DataProvider.GetCategoriesAsync(category.ID))
                    {
                        category.ChildCategories.Add(cat);
                    }
                }
            });
            
        }
        public async Task GetChildCategoriesAsync()
        {
            if (childCategories.Count > 0)
                return;
            var cat = await ObjectRepository.DataProvider.GetCategoriesAsync();
            foreach (var item in cat)
                childCategories.Add(item);
        }

        private async Task OnNewChildCategoryItemsAsync(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Hallo");
            if (e.NewItems != null)
            {
                foreach (Category item in e.NewItems)
                {
                    await item.GetChildCategoriesAsync();
                }
            }
        }
        

    }
}
