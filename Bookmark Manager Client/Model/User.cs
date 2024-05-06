using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.Model
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public uint ID { get; set; }
        public string email;
        public string Email { get => email; set { email = value; OnPropertyChanged(); } }
        private string username;
        public string Name { get => username; set { username = value; OnPropertyChanged(); }  }

        public bool administrator;
        public bool Administrator { get => administrator; set { administrator = value; OnPropertyChanged(); } }

        public static bool operator ==(User a, User b)
        {
            if(a is null || b is null) return false;
            if (a.ID == b.ID)
                return true;
            return false;
        }
        public static bool operator !=(User a, User b)
        {
            if( a is null || b is null) return true;
            if (a.ID != b.ID)
                return true;
            return false;
        }
        public override bool Equals(Object user)
        {
            if ((user is null) || !this.GetType().Equals(user.GetType()))
            {
                return false;
            }
            if ((User)user == this)
                return true;
            else
                return false;
        }
        public override int GetHashCode()
        {
            return (int)ID;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
