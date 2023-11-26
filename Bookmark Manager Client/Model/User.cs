using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.Model
{
    public class User
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public bool Administrator { get; set; }

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
