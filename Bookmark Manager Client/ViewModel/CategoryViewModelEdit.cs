using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Bookmark_Manager_Client.Utils;
using Windows.UI.Xaml.Media.Animation;
using System.Threading;
using System.Windows.Data;
using Bookmark_Manager_Client.Commands;
using System.Windows.Media;

namespace Bookmark_Manager_Client.ViewModel
{
    public class CategoryViewModelEdit : CategoryViewModelBase
    {

        object permissionLock = new object();

        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get => mainViewModel;
            set
            {
                mainViewModel = value;
                var category = MainViewModel.SelectedCategory;
                CategoryName = category.Name;
                CategoryDescription = category.Description;
                IconName = category.IconName;

                Icons = MainViewModel.Icons;
                selectIcon();
                IconsView = CollectionViewSource.GetDefaultView(Icons);
                IconsView.Filter = iconFilter;

                addPermittedUserToList(category.ID);

                if(category.ParentID == 0)
                    IsTopCategory = true;
                else
                    IsTopCategory = false;

                OnPropertyChanged();
            }
        }



        private bool isTopCategory;
        public bool IsTopCategory { get => isTopCategory; private set { isTopCategory = value; OnPropertyChanged(); } }


        public RemoveUserFromListCommand RemoveUserFromListCommand { get; set; }

        public CategoryViewModelEdit()
        {
            ObjectRepository.LogEvent.Clear();
            RemoveUserFromListCommand = new RemoveUserFromListCommand(PermittedUsers);
            BindingOperations.EnableCollectionSynchronization(PermittedUsers, permissionLock);
        }
        
        
        private void addPermittedUserToList(uint categoryId)
        {
            Task.Run(async () =>
            {
                var users = await ObjectRepository.DataProvider.GetPermittedUsersAsync(categoryId);
                foreach (var user in users)
                {
                    lock (permissionLock)
                        PermittedUsers.Add(user);
                }
            });
            
        }

        public async Task<bool> UpdateCategoryAsync()
        {
            if (CategoryName == "")
                return false;

            var category = MainViewModel.SelectedCategory;
            category.Name = CategoryName;
            category.Description = CategoryDescription;
            category.IconName = Icon.IconTitle;

            if (!await ObjectRepository.DataProvider.ChangeCategoryAsync(category))
                return false;

            if (category.ParentID == 0)
                if (!await ObjectRepository.DataProvider.ChangePermissionsAsync(PermittedUsers, category))
                    return false;
                else
                {
                    if(PermittedUsers.Count > 1) category.SetSharedState(true);
                    else category.SetSharedState(false);
                }

            MainViewModel.SetDefaultView();
            return true;
        }

        public void Exit() => MainViewModel.SetDefaultView();
    }
}
