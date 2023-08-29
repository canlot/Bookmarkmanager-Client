using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Bookmark_Manager_Client.Model;

namespace Bookmark_Manager_Client.Controller
{
    interface IPermitableObjectController
    {
        ObservableCollection<User> AllUsers
        {
            get;
            set;
        }
        ObservableCollection<User> PermittedUsers
        {
            get;
            set;
        }

    }
}
