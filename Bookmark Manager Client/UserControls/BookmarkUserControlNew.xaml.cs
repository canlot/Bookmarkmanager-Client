using Bookmark_Manager_Client.ViewModel;
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
    /// Interaktionslogik für BookmarkUserControl.xaml
    /// </summary>
    public partial class BookmarkUserControlNew : UserControl
    {
        public BookmarkUserControlNew()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as BookmarkViewModelNew;
            vm.Exit();
        }

        private async void SaveCategory_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as BookmarkViewModelNew;
            await vm.SaveBookmarkAsync();
        }
    }
}
