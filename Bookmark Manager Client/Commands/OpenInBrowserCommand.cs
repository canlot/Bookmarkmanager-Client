using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookmark_Manager_Client.Commands
{
    public class OpenInBrowserCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter != null && parameter is string)
                System.Diagnostics.Process.Start((string)parameter);
        }
    }
}
