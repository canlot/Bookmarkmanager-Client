﻿using System;
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

namespace Bookmark_Manager_Client
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainViewModelController controller = new MainViewModelController();
        public ApplicationSettings settings = new ApplicationSettings();
        public MainWindow()
        {
            InitializeComponent();
            //WindowLogin windowLogin = new WindowLogin(settings);
            //windowLogin.ShowDialog();
            settings.Username = "admin";
            settings.Password = "admin";
            settings.Url = "http://localhost:8080/apiv1";
            RestCommunicator.GetInstance().UserName = settings.Username;
            RestCommunicator.GetInstance().Password = settings.Password;
            RestCommunicator.GetInstance().SetUpConnection();
            
            controller.getCategories();
        }

        private void treeViewCategory_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Category category = (Category)treeViewCategory.SelectedItem;
            category.GetBookmarks();
            listBoxBookmarks.ItemsSource = category.Bookmarks;

            category.GetPermissionUsers();
            //listBoxFullAccessUser.ItemsSource = category.PermissionUsers;
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            /*
            if(treeViewCategory.SelectedItem == null)
            {
                var categoryWindow = new CategoryWindow(controller.Categories, CategoryWindow.WindowMode.CreateNewCategory);
                categoryWindow.Owner = this;
                categoryWindow.ShowDialog();
            }
            else
            {
                if (((Category)treeViewCategory.SelectedItem).OwnerID == RestConnectionHandler.GetInstance().CurrentUser.ID)
                {
                    var categoryWindow = new CategoryWindow(controller.Categories, CategoryWindow.WindowMode.CreateNewCategoryWithParent, (Category)treeViewCategory.SelectedItem);
                    categoryWindow.Owner = this;
                    categoryWindow.ShowDialog();
                }
                else
                {
                    var categoryWindow = new CategoryWindow(controller.Categories, CategoryWindow.WindowMode.CreateNewCategory);
                    categoryWindow.Owner = this;
                    categoryWindow.ShowDialog();
                }
                
            }
            */
            
        }
        private void ButtonEditCategory_Click(object sender, RoutedEventArgs e)
        {
            /*
            if(treeViewCategory.SelectedItem != null)
            {
                if (((Category)treeViewCategory.SelectedItem).OwnerID == RestConnectionHandler.GetInstance().CurrentUser.ID)
                {
                    var categegoryWindow = new CategoryWindow(controller.Categories, CategoryWindow.WindowMode.ModifyExistingCategory, (Category)treeViewCategory.SelectedItem);
                    categegoryWindow.Owner = this;
                    categegoryWindow.ShowDialog();
                }
                else
                    MessageBox.Show("Sie sind kein Besitzer der Kategorie");
            }
            */
        }
        private void ButtonRemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            if(treeViewCategory.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Damit werden alle Unterkategorien und dazugehörigen Lesezeichen gelöscht, bist du sicher?", "Wirklich löschen?", 
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if(result == MessageBoxResult.Yes)
                {
                    //controller.DeleteCategory((Category)treeViewCategory.SelectedItem);
                }

            }
        }
        private void ButtonAddBookmark_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewCategory.SelectedItem == null || (((Category)treeViewCategory.SelectedItem).OwnerID != ObjectRepository.DataProvider.CurrentUser.ID))
                return;
            BookmarkWindow bookmarkWindow = new BookmarkWindow(BookmarkWindow.WindowMode.CreateBookmark, (Category)treeViewCategory.SelectedItem);
            bookmarkWindow.Owner = this;
            bookmarkWindow.Show();
        }
        private void ButtonEditBookmark_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewCategory.SelectedItem == null || listBoxBookmarks.SelectedItem == null || (((Category)treeViewCategory.SelectedItem).OwnerID != ObjectRepository.DataProvider.CurrentUser.ID))
                return;
            BookmarkWindow bookmarkWindow = new BookmarkWindow(BookmarkWindow.WindowMode.ModifyBookmark, (Category)treeViewCategory.SelectedItem,
                (Bookmark)listBoxBookmarks.SelectedItem);

            bookmarkWindow.Owner = this;
            bookmarkWindow.Show();
        }
        private void ButtonDeleteBookmark_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

public class ApplicationSettings
{
    public string Url;
    public string Username;
    public string Password;

}