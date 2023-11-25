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
    public class MainViewModelController
    {
        

        public MainViewModelController()
        {
            //Categories = new ObservableCollection<Category>();
            //Categories.CollectionChanged += OnNewCategoryItems;
        }
        public IEnumerable<Category> getCategories()
        {
            return ObjectRepository.DataProvider.GetCategories();
            
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

        public void RemoveCategory(Category category)
        {
            ObjectRepository.DataProvider.DeleteCategory(category.ID);
        }
    }
}
