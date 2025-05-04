using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.ObjectModel;
using RestSharp.Serializers.NewtonsoftJson;
using Bookmark_Manager_Client.Controller;
using Bookmark_Manager_Client.Model;
using Bookmark_Manager_Client.ViewModel;
using ModernWpf.Controls;
using Bookmark_Manager_Client.UserControls;
using Bookmark_Manager_Client.Utils;
using Windows.Storage.FileProperties;

namespace Bookmark_Manager_Client
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            LoginWindow windowLogin = new LoginWindow();
            windowLogin.ShowDialog();

            InitializeComponent();
        }

        private async void treeViewCategory_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Category category = (Category)treeViewCategory.SelectedItem;
            MainViewModel vm = (MainViewModel)this.DataContext;
            if(category != null)
            {
                await vm.GetBookmarksAsync(category);
                vm.SelectedCategory = category;
            }
            
        }
      


        private void listBoxBookmarks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            
        }

        private void OpenInBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            Bookmark bookmark = (Bookmark)listBoxBookmarks.SelectedItem;
            AppBarButton button = (AppBarButton)sender;
            
            if (bookmark == null) return;
            System.Diagnostics.Process.Start(bookmark.Url);
        }

        private async void CategoryDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Category category = (Category)treeViewCategory.SelectedItem;
            if (category == null) return;
            var vm = this.DataContext as MainViewModel;
            var popup = new CategoryUserControlDelete(category.Name);
            var result = await popup.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if(await ObjectRepository.DataProvider.DeleteCategoryAsync(category.ID))
                {
                    vm.SelectedCategory = null;
                    vm.Categories.DeleteCategoryWithChildCategories(category);
                }
            }
        }

        private async void BookmarkDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Bookmark bookmark = (Bookmark)listBoxBookmarks.SelectedItem;
            if(bookmark == null) return;
            var vm = this.DataContext as MainViewModel;
            var popup = new BookmarkUserControlDelete(bookmark.Title);
            var result = await popup.ShowAsync();
            if(result == ContentDialogResult.Primary) 
            {
                if(await ObjectRepository.DataProvider.DeleteBookmarkAsync(bookmark))
                {
                    vm.SelectedBookmark = null;
                    vm.Bookmarks.Remove(bookmark);
                }
            }
        }

        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var vm = this.DataContext as MainViewModel;
            await vm.SearchForCategoriesAsync(sender.Text);
            await vm.SearchForBookmarksAsync(sender.Text);

        }

        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if(sender.Text.Length == 0)
            {
                var vm = this.DataContext as MainViewModel;
                await vm.GetTopCategoriesWithChildAsync();
            }
        }

        private void CutBookmarksButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            
            vm.SelectedBookmarksCopySource.Clear();
            foreach(var bookmark in listBoxBookmarks.SelectedItems)
            {
                vm.SelectedBookmarksCopySource.Add((Bookmark) bookmark);
            }

            vm.CategoryCopySource = treeViewCategory.SelectedItem as Category;

        }

        private async void InsertBookmarksButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            vm.CategoryCopyDestination = (Category)(sender as MenuItem).DataContext;
            await vm.MoveBookmarksAsync();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            await vm.GetTopCategoriesWithChildAsync();
        }

    }
}
