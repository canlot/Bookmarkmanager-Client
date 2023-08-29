using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Bookmark_Manager_Client.Model;

namespace Bookmark_Manager_Client.Controller
{
    public class ModelViewController
    {
        private ObservableCollection<Category> categories;
        public ObservableCollection<Category> Categories
        {
            get => categories;
            private set => categories = value;

        }
        //public ObservableCollection<Bookmark> Bookmarks { get; set; }

        public ModelViewController()
        {
            Categories = new ObservableCollection<Category>();
            Categories.CollectionChanged += OnNewCategoryItems;
        }
        public void getCategories()
        {
            var cat = RestConnectionHandler.GetInstance().GetCategories();
            if(cat != null)
                foreach (var item in cat)
                    categories.Add(item);
        }

        private void OnNewCategoryItems(object sender, NotifyCollectionChangedEventArgs e)
        {
            
            if(e.NewItems != null)
            {
                foreach (Category item in e.NewItems)
                {
                    item.GetChildCategories();
                }
            }

        }
        public void DeleteCategory(Category category)
        {
            if(RestConnectionHandler.GetInstance().DeleteCategory(category))
            {
                RemoveCategory(category, categories);
            }
        }
        public void RemoveCategory(Category category, IList<Category> categories)
        {
            foreach(var item in categories)
            {
                if (category == item)
                {
                    categories.Remove(category);
                    return;
                }
                else
                {
                    RemoveCategory(category, item.ChildCategories);
                }

            }
        }
    }
}
