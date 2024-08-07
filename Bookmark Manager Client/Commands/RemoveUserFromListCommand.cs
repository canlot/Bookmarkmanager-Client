﻿using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookmark_Manager_Client.Commands
{
    public class RemoveUserFromListCommand : ICommand
    {
        private ObservableCollection<User> users;
        public RemoveUserFromListCommand(ObservableCollection<User> users) 
        {
            this.users = users;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {

            if (parameter is User && (parameter as User).ID != ObjectRepository.DataProvider.CurrentUser.ID)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter is User && (parameter as User).ID != ObjectRepository.DataProvider.CurrentUser.ID)
                users.Remove(parameter as User);
        }
    }
}
