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
using Bookmark_Manager_Client.Controller;
using Bookmark_Manager_Client.Model;

namespace Bookmark_Manager_Client
{
    /// <summary>
    /// Interaktionslogik für BookmarkWindow.xaml
    /// </summary>
    public partial class BookmarkWindow : Window
    {
        public enum WindowMode
        {
            CreateBookmark,
            ModifyBookmark
        }
        WindowMode windowMode;
        BookmarkController controller;
        public BookmarkWindow(WindowMode windowMode, Category category, Bookmark bookmark = null)
        {
            InitializeComponent();
            this.windowMode = windowMode;
            switch(windowMode)
            {
                case WindowMode.CreateBookmark:
                    controller = new BookmarkController(category);
                    break;
                case WindowMode.ModifyBookmark:
                    controller = new BookmarkController(category, bookmark);
                    break;
            }
            this.DataContext = controller;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            switch(windowMode)
            {
                case WindowMode.CreateBookmark:
                    if (!controller.CreateNewBookmark())
                        return;
                    break;
                case WindowMode.ModifyBookmark:
                    controller.ModifyBookmark();
                    break;
            }
            this.Close();
        }

        private void ButtonAbort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
