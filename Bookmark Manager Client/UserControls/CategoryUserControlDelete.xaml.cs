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
using ModernWpf.Controls;

namespace Bookmark_Manager_Client.UserControls
{
    /// <summary>
    /// Interaktionslogik für CategoryUserControlDelete.xaml
    /// </summary>
    public partial class CategoryUserControlDelete : ContentDialog
    {
        public string CategoryName { get; set; }
        public CategoryUserControlDelete(string categoryName)
        {
            CategoryName = categoryName;
            InitializeComponent();
        }
    }
}
