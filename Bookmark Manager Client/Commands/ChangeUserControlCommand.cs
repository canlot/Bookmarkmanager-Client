using Bookmark_Manager_Client.UserControls;
using Bookmark_Manager_Client.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bookmark_Manager_Client.Commands
{
    public class ChangeUserControlCommand : ICommand, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private UserControl userControl;
        public UserControl UserControl
        {
            get => userControl;
            set
            {
                userControl = value;
                OnPropertyChanged();
            }
        }
        private MainViewModel viewModel;
        public ChangeUserControlCommand(MainViewModel viewModel) 
        {
            this.viewModel = viewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            /*
            if (parameter == null)
            {
                return true;
            }
            if (parameter is string)
            {
                switch (parameter)
                {
                    case "CategoryUserControlNew":
                        return true;
                    case "CategoryUserControlEdit":
                        return true;
                    case "BookmarkUserControlNew":
                        if (viewModel.SelectedCategory == null)
                            return false;
                        else return true;
                    default:
                        return true;
                }

            }
            */
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter == null) 
            {
                return;
            }
            if(parameter is string) 
            {
                switch(parameter)
                {
                    case "CategoryUserControlNew":
                        UserControl = new CategoryUserControlNew();
                        break;
                    case "CategoryUserControlEdit":
                        if (viewModel.SelectedCategory == null) return;
                        UserControl = new CategoryUserControlEdit();
                        break;
                    case "CategoryUserControlDelete":
                        break;
                    case "BookmarkUserControlNew":
                        UserControl = new BookmarkUserControlNew();
                        break;
                    case "BookmarkUserControlEdit":
                        if(viewModel.SelectedBookmark == null) return;
                        UserControl = new BookmarkUserControlEdit();
                        break;
                    case "SettingsUserControl":
                        UserControl = new SettingsUserControl();
                        break;
                    case "BrowserUserControl":
                        if (viewModel.SelectedBookmark == null) return;
                        UserControl = new BrowserUserControl(viewModel.SelectedBookmark.Url);
                        break;
                    default:
                        UserControl = new UserControl();
                        break;
                }
                
            }
            return;
        }
    }
}
