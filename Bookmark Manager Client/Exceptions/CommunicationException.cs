using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.Exceptions
{
    public class CommunicationException : Exception
    {
        public string ServerMessage;
        public string InternalMessage;
        public CommunicationException() { }
    }
}
