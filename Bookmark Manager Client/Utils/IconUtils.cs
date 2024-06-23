using Bookmark_Manager_Client.Configurators;
using FaviconFetcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.Utils
{
    public class IconUtils
    {
        public string DownloadIcon(string url)
        {
            var fetcher = new Fetcher();
            var image = fetcher.FetchClosest(new Uri(url), new IconSize(32, 32));
            var iconPath = ObjectRepository.AppConfiguration.UploadPath + "\\" + "upload.png";
            image.Save(iconPath);

            return iconPath;
        }
    }
}
