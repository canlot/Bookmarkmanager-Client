using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.ViewModel
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private Category parentCategory { get; set; }
        public Category ParentCategory 
        { 
            get => parentCategory; 
            set
            {
                parentCategory = value;
                OnPropertyChanged();
            }
        }

        private Category category;
        public Category Category 
        {
            get => category; 
            set
            {
                category = value;
                OnPropertyChanged();
            }
        }
        public EventDispatcher EventDispatcher { get; set; }
        public CategoryViewModel(bool isNew, Category category = null) 
        {
            
        }
    }
}
