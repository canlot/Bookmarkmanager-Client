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
    /// Interaktionslogik für UserUserControlAdd.xaml
    /// </summary>
    public partial class UserUserControlAdd : ContentDialog
    {
        public UserUserControlAdd(SettingsViewModel vm)
        {
            UserViewModelAdd userViewModelAdd = new UserViewModelAdd(vm);
            userViewModelAdd.SettingsViewModel = vm;
            this.DataContext = userViewModelAdd;
            InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            (this.DataContext as UserViewModelAdd).Password = PasswordBox.Password;
            await (this.DataContext as UserViewModelAdd).Save();
        }
    }
}
