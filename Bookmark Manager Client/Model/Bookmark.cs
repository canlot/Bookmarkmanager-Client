using Bookmark_Manager_Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.Model
{
    public class Bookmark
    {
        public uint ID { get; set; }
        public uint CategoryID { get; set; }
        public string Url { get; set; }

        public OpenInBrowserCommand OpenInBrowserCommand { get; set; }

        public Bookmark() 
        {
            OpenInBrowserCommand = new OpenInBrowserCommand();
        }
    }
}
