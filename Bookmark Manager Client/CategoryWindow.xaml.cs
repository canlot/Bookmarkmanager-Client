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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Bookmark_Manager_Client.Model;
using Bookmark_Manager_Client.Controller;

namespace Bookmark_Manager_Client
{
    /// <summary>
    /// Interaktionslogik für CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        public enum WindowMode
        {
            CreateNewCategory,
            CreateNewCategoryWithParent,
            ModifyExistingCategory
        }
        WindowMode windowMode;
        CategoryController controller;
        public CategoryWindow(ObservableCollection<Category> categories,  WindowMode mode, Category category = null)
        {
            InitializeComponent();
            windowMode = mode;
            
            switch(windowMode)
            {
                case WindowMode.CreateNewCategory:
                    controller = new CategoryController(categories);
                    break;
                case WindowMode.CreateNewCategoryWithParent:
                    controller = new CategoryController(categories, category);
                    break;
                case WindowMode.ModifyExistingCategory:
                    controller = new CategoryController(categories, null, category);
                    break;
            }
            this.DataContext = controller;
            ListViewAllUsers.ItemsSource = controller.AllUsers;
            ListViewPermittedUsers.ItemsSource = controller.PermittedUsers;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            switch(windowMode)
            {
                case WindowMode.CreateNewCategory:
                    controller.CreateNewCategory();
                    break;
                case WindowMode.CreateNewCategoryWithParent:
                    controller.CreateNewCategory();
                    break;
                case WindowMode.ModifyExistingCategory:
                    controller.ModifyCategory();
                    break;
                    
            }
            this.Close();
        }

        private void ButtonAbort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonParentCategoryDelete_Click(object sender, RoutedEventArgs e)
        {
            controller.ParentCategory = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(windowMode == WindowMode.CreateNewCategoryWithParent)
            {

            }
        }

        private void ButtonAddPermission_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewAllUsers.SelectedValue != null)
                controller.AddUserToPermittedUsers((User)ListViewAllUsers.SelectedItem);
        }

        private void ButtonRemovePermission_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewPermittedUsers.SelectedValue != null)
                controller.RemoveUserFromPermittedUsers((User)ListViewPermittedUsers.SelectedItem);
        }
    }
}
