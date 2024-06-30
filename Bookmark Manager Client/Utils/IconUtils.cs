using Bookmark_Manager_Client.Configurators;
using FaviconFetcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.Utils
{
    public static class IconUtils
    {
        public static string DownloadIcon(string url)
        {
            var fetcher = new Fetcher();
            var iconPath = ObjectRepository.AppConfiguration.UploadPath + @"\" + "upload.png";

            using (var image = fetcher.FetchClosest(new Uri(url), new IconSize(64, 64)))
            {
                if (image == null)
                    throw new ArgumentNullException("image");

                image.Save(iconPath);
                image.Dispose();
            }
                
            return iconPath;
        }
        public static string ComputeIconHash(string path)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    byte[] hashValue = mySHA256.ComputeHash(stream);
                    return ByteArrayToString(hashValue);
                }
            }
        }
        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }
    }
}
