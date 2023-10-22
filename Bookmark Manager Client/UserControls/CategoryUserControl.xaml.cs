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
    public partial class CategoryUserControl : UserControl
    {
        private bool isNewCategory;
        public CategoryViewModel CategoryViewModel { get; set; }
        public CategoryUserControl(bool isNewCategory)
        {
            InitializeComponent();
            
            MainViewModel mainViewModel = (MainViewModel)this.DataContext;
            
            if(isNewCategory) 
            {
                //CategoryViewModel categoryViewModel = this.Resources["CategoryViewModel"] as CategoryViewModel;
                CategoryViewModel = new CategoryViewModel(true);
                CategoryViewModel.ParentCategory = mainViewModel.SelectedCategory;
            }
            else
            {
                CategoryViewModel = new CategoryViewModel(false, mainViewModel.SelectedCategory);
                
            }
            this.Resources["CategoryViewModel"] = CategoryViewModel;
        }

        private void ToggleSwitch_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            CategoryViewModel categoryViewModel = this.Resources["CategoryViewModel"] as CategoryViewModel;
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
