using Bookmark_Manager_Client.Commands;
using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Bookmark_Manager_Client.ViewModel
{
    public class CategoryViewModelNew : CategoryViewModelBase
    {

        private Category parentCategory { get; set; }
        public Category ParentCategory { get => parentCategory; set { parentCategory = value; OnPropertyChanged(); } }

        private bool isTopCategory;
        public bool IsTopCategory { get => isTopCategory; set { isTopCategory = value; OnPropertyChanged(); } }


        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel 
        {
            get => mainViewModel;
            set
            {
                mainViewModel = value;
                parentCategory = MainViewModel.SelectedCategory;

                Icons = MainViewModel.Icons;
                selectIcon();
                IconsView = CollectionViewSource.GetDefaultView(Icons);
                IconsView.Filter = iconFilter;

                if (MainViewModel.SelectedCategory == null) IsTopCategory = true;
                OnPropertyChanged();
            }
        }

        

        public RemoveUserFromListCommand RemoveUserFromListCommand { get; set; }

        public CategoryViewModelNew()
        {
            ObjectRepository.LogEvent.Clear();
            RemoveUserFromListCommand = new RemoveUserFromListCommand(PermittedUsers);
            var user = ObjectRepository.DataProvider.CurrentUser;
            PermittedUsers.Add(user);
        }
        
        public async Task<bool> SaveCategoryAsync()
        {
            if(string.IsNullOrEmpty(CategoryName)) return false;

            var category = new Category()
            {
                Name = CategoryName,
                Description = CategoryDescription,
                ParentID = 0,
                OwnerID = ObjectRepository.DataProvider.CurrentUser.ID,
                Shared = (PermittedUsers.Count > 1) ? true : false
            };

            if(!isTopCategory && ParentCategory == null) return false;

            if (!IsTopCategory) category.ParentID = ParentCategory.ID;

            category.IconName = Icon.IconTitle;

            try
            {
                await ObjectRepository.DataProvider.AddCategoryAsync(category); // no need to set the id, because category will be set in dataprovider and it is the same object

                if(category.ParentID == 0)
                    await ObjectRepository.DataProvider.ChangePermissionsAsync(PermittedUsers, category);

                if(category.ParentID != 0)
                    ParentCategory.ChildCategories.Add(category);
                else
                    MainViewModel.Categories.Add(category);

                MainViewModel.SetDefaultView();

            }
            catch (Exception ex) { return false; }

            return true;

        }

        public void Exit() => MainViewModel.SetDefaultView();
    }
}
