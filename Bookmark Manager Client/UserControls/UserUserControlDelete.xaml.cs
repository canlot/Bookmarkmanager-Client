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
    /// Interaktionslogik für UserUserControlDelete.xaml
    /// </summary>
    public partial class UserUserControlDelete : ContentDialog
    {
        public string UserName { get; set; }
        public UserUserControlDelete(string userName)
        {
            InitializeComponent();
            UserName = userName;
        }
    }
}
