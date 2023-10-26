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

        private void ToggleSwitch_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            CategoryViewModelNew categoryViewModel = (CategoryViewModelNew)this.DataContext;
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    categoryViewModel.ParentCategory = null;
                }
                else
                {
                    
                }
            }
        }
    }
}
