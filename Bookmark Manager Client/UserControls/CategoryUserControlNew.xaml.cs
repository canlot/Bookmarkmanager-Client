using Bookmark_Manager_Client.Model;
using Bookmark_Manager_Client.ViewModel;
using ModernWpf.Controls;
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

namespace Bookmark_Manager_Client.UserControls
{
    /// <summary>
    /// Interaktionslogik für CategoryAddUserControl.xaml
    /// </summary>
    public partial class CategoryUserControlNew : UserControl
    {
        public CategoryUserControlNew()
        {
            InitializeComponent();

        }


        private void UserSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is User)
            {
                //User selected an item, take an action
                var vm = this.DataContext as CategoryViewModelNew;
                vm.AddPermittedUser(args.ChosenSuggestion as User);
            }
            else if (!string.IsNullOrEmpty(args.QueryText))
            {

            }
        }

        private void UserSearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is User user)
            {
                sender.Text = user.ToString();
            }
        }

        private void UserSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var users = ObjectRepository.DataProvider.SearchUser(sender.Text);

            if (users.Count > 0)
                sender.ItemsSource = users;
            else
                sender.ItemsSource = new string[] { "No results found" };
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (CategoryViewModelNew)this.DataContext;
            viewModel.SaveCategory();
        }

        private void ButtonAbort_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (CategoryViewModelNew)this.DataContext;
            viewModel.Exit();
        }
    }
}
