using Bookmark_Manager_Client.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Bookmark_Manager_Client.Model
{
    public class Bookmark : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private uint id;
        public uint ID { get => id; set { id = value; OnPropertyChanged(); } }

        private uint categoryID;
        public uint CategoryID { get => categoryID; set { categoryID = value; OnPropertyChanged(); } }

        private string url;
        public string Url { get => url; set { url = value; OnPropertyChanged(); } }

        private string title;
        public string Title { get => title; set { title = value; OnPropertyChanged(); } }

        private string description;
        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        private DateTime createDate;
        public DateTime CreateDate { get => createDate; set { createDate = value; OnPropertyChanged(); } }

        private string iconName;

        public string IconName
        {
            get { return iconName; }
            set { iconName = value; OnPropertyChanged(); OnPropertyChanged("IconPath"); }
        }

        public string IconPath => ObjectRepository.AppConfiguration.IconsPath + @"\" + IconName;

        public OpenInBrowserCommand OpenInBrowserCommand { get; set; }
        public CopyToClipboardCommand CopyToClipboardCommand { get; set; }

        public Bookmark() 
        {
            OpenInBrowserCommand = new OpenInBrowserCommand();
            CopyToClipboardCommand = new CopyToClipboardCommand();
        }
    }
}
