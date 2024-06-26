using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;

namespace Bookmark_Manager_Client.Configurators
{
    public class AppConfiguration
    {

        public static string GlobalConfigurationPath = ".";
        private static string AppConfigurationFilePath
        {
            get => GlobalConfigurationPath + "\\" + "config.ini";
        }
        private string clientCachePath;

        public string ClientCachePath
        {
            get { return clientCachePath; }
            set { clientCachePath = value; }
        }
        private string iconsPath;

        public string IconsPath
        {
            get { return iconsPath; }
            set { iconsPath = value; }
        }

        private string uploadPath;

        public string UploadPath
        {
            get { return uploadPath; }
            set { uploadPath = value; }
        }

        public string Host
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }
        public bool SSL
        {
            get;
            set;
        }
        public bool IgnoreCertificate
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public AppConfiguration()
        {
            LoadConfig();
            var currentDir = Directory.GetCurrentDirectory();
            clientCachePath = "cache";
            iconsPath = currentDir + @"\" + clientCachePath + @"\" + "icons";
            uploadPath = currentDir + @"\" +  clientCachePath + @"\" + "upload";
            createPathIfNotExist(clientCachePath);
            createPathIfNotExist(iconsPath);
            createPathIfNotExist(uploadPath);

        }
        private void createPathIfNotExist(string path)
        {
            if (path == null || path == "") return;
            if (Directory.Exists(path)) return;

            Directory.CreateDirectory(path);
        }
        public bool LoadConfig()
        {
            try
            {
                var parser = new FileIniDataParser();
                var configData = parser.ReadFile(AppConfigurationFilePath);
                string global = "Global";
                Host = configData[global]["Host"];
                Port = int.Parse(configData[global]["Port"]);
                SSL = bool.Parse(configData[global]["SSL"]);
                IgnoreCertificate = bool.Parse(configData[global]["IgnoreCertificate"]);
                Email = configData[global]["UserName"];
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SaveConfig()
        {
            try
            {
                var parser = new FileIniDataParser();
                var configData = new IniData();
                string global = "Global";
                configData[global]["Host"] = Host;
                configData[global]["Port"] = Convert.ToString(Port);
                configData[global]["SSL"] = Convert.ToString(SSL);
                configData[global]["IgnoreCertificate"] = Convert.ToString(IgnoreCertificate);
                configData[global]["UserName"] = Email;
                parser.WriteFile(AppConfigurationFilePath, configData);
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
