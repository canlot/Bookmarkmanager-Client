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
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            PasswordBoxPassword.Password = LoginViewModel.Password;
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel.Password = PasswordBoxPassword.Password;
            LoginViewModel.SaveSettings();
            if(await ObjectRepository.DataProvider.LoginAsync())
                this.Close();
        }

        private void ButtonAbort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void PasswordBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginViewModel.Password = PasswordBoxPassword.Password;
                LoginViewModel.SaveSettings();
                if (await ObjectRepository.DataProvider.LoginAsync())
                    this.Close();
            }
        }
    }
}
