using Bookmark_Manager_Client.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Resources;

namespace Bookmark_Manager_Client.Localization
{
    public class Localizationprovider : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private static readonly Localizationprovider instance = new Localizationprovider();

        public static Localizationprovider Instance
        {
            get { return instance; }
        }

        ResourceManager resourceManager = Localization.Strings.ResourceManager;
        
        
        private CultureInfo currentCulture = null;

        public string this[string key]
        {
            get { return this.resourceManager.GetString(key, this.currentCulture); }
        }

        public CultureInfo CurrentCulture
        {
            get { return this.currentCulture; }
            set
            {
                if (this.currentCulture != value)
                {
                    this.currentCulture = value;
                    var @event = this.PropertyChanged;
                    if (@event != null)
                    {
                        @event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                    }
                }
            }
        }
    }
    public class LocalizationExtension : Binding
    {
        public LocalizationExtension(string name) : base("[" + name + "]")
        {
            this.Mode = BindingMode.OneWay;
            this.Source = Localizationprovider.Instance;
        }
    }
}
