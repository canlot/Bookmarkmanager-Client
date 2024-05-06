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
    /// Interaktionslogik für UserUserControlEdit.xaml
    /// </summary>
    public partial class UserUserControlEdit : ContentDialog
    {
        public UserUserControlEdit(SettingsViewModel vm)
        {
            UserViewModelEdit userViewModelEdit = new UserViewModelEdit(vm);
            userViewModelEdit.SettingsViewModel = vm;
            this.DataContext = userViewModelEdit;
            InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ModernWpf.Controls.ContentDialog sender, ModernWpf.Controls.ContentDialogButtonClickEventArgs args)
        {
            (this.DataContext as UserViewModelEdit).Password = PasswordBox.Password;
            await(this.DataContext as UserViewModelEdit).Save();
        }
    }
}
