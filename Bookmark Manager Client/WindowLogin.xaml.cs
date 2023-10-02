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

namespace Bookmark_Manager_Client
{
    /// <summary>
    /// Interaktionslogik für WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        ApplicationSettings settings;
        public WindowLogin(ApplicationSettings settings)
        {
            InitializeComponent();
            this.settings = settings;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            
            //ObjectRepository.AppConfiguration. = TextBoxUrl.Text;
            settings.Username = TextBoxUsername.Text;
            settings.Password = TextBoxPassword.Text;
            this.Close();
        }
    }
}
