using Bookmark_Manager_Client.Model;
using Bookmark_Manager_Client.Utils;
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
    /// Interaktionslogik für SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        public SettingsUserControl()
        {
            InitializeComponent();
        }

        private void ButtonAbort_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (SettingsViewModel)this.DataContext;
            viewModel.Exit();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserUserControlAdd(this.DataContext as SettingsViewModel);
            await dialog?.ShowAsync();
        }

        private async void editUserButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserUserControlEdit(this.DataContext as SettingsViewModel);
            await dialog?.ShowAsync();
        }

        private async void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)UsersListBox.SelectedItem;
            if (user == null) return;
            var vm = this.DataContext as SettingsViewModel;
            var popup = new UserUserControlDelete(user.Name);
            var result = await popup.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await vm.DeleteUserAsync();
            }
        }

        private async void TabControl_Initialized(object sender, EventArgs e)
        {
            var vm = this.DataContext as SettingsViewModel;
            await vm.GetAllUsers();
        }
    }
}
