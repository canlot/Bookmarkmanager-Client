using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using Bookmark_Manager_Client.Model;
using Newtonsoft.Json;

namespace Bookmark_Manager_Client.DataProvider 
{
    public class RestDataProvider : IDataProvider
    {
        private static RestDataProvider instance;
        private RestClient client;
        private string host;
        public string InternalUrl
        {
            get; private set;
        }
        private int port;
        public int Port
        {
            get => port;
            set
            {
                port = value;
                recalculateInternalUrl();
            }
        }
        public string Host
        {
            get => host;
            set
            {
                host = value;
                recalculateInternalUrl();
            }
        }
        private string userName;
        public string UserName
        {
            get => userName;
            set => userName = value;
        }
        private string password;
        public string Password
        {
            get => password;
            set => password = value;
        }
        private User currentUser;
        public User CurrentUser { get => currentUser; }
        private void recalculateInternalUrl()
        {
            string afterurl = "/apiv1";
            string host = this.host ?? "localhost"; //If this.host == null then host = localhost
            int port = this.port == 0 ? 8080 : this.port;
            InternalUrl = "http://" + host + ":" + Convert.ToString(port) + afterurl;
        }
        public bool TestConnection()
        {
            return false;
        }
        private RestDataProvider()
        {
            
        }
        public void SetUpConnection()
        {
            recalculateInternalUrl();
            var options = new RestClientOptions(InternalUrl)
            {
                Authenticator = new HttpBasicAuthenticator(userName, password, Encoding.UTF8)
            };
            client = new RestClient(options);

            //JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            //jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //client.UseNewtonsoftJson(jsonSerializerSettings);
            
            Console.WriteLine(InternalUrl);

            currentUser = getCurrentUser();
        }
        private User getCurrentUser()
        {
            //var request = new RestRequest("currentuser", RestSharp.DataFormat.Json);
            var request = new RestRequest("currentuser", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            //return client.Get<User>(request).Data;
            return client.Get<User>(request);
        }
        private static readonly object _lock = new object();
        public static RestDataProvider GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                    instance = new RestDataProvider();
                return instance;
            }
            else
                return instance;
        }
        public ObservableCollection<Category> GetCategories(uint id = 0)
        {

            RestRequest request;
            if(id == 0)
                request = new RestRequest("categories/", Method.Get);
            //request = new RestRequest("categories/", RestSharp.DataFormat.Json);
            else
                request = new RestRequest("categories/" + id.ToString() + "/", Method.Get);
            //request = new RestRequest("categories/" + id.ToString() + "/", RestSharp.DataFormat.Json);

            request.AddHeader("Cache-Control", "no-cache");
            //return client.Get<ObservableCollection<Category>>(request).Data;
            return client.Get<ObservableCollection<Category>>(request); 
        }
        public ObservableCollection<Category> GetAllCategories()
        {
            throw new NotImplementedException();
        }
        public ObservableCollection<Bookmark> GetBookmarks( uint id)
        {
            //var request = new RestRequest("categories/" + id.ToString() + "/bookmarks/", RestSharp.DataFormat.Json);
            var request = new RestRequest("categories/" + id.ToString() + "/bookmarks/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            //return client.Get<ObservableCollection<Bookmark>>(request).Data;
            return client.Get<ObservableCollection<Bookmark>>(request);
        
        }
        public ObservableCollection<User> GetPermittedUsers(uint id)
        {
            //var request = new RestRequest("categories/" + id.ToString() + "/permissions/", RestSharp.DataFormat.Json);
            var request = new RestRequest("categories/" + id.ToString() + "/permissions/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return client.Get<ObservableCollection<User>>(request);
            //return client.Get<ObservableCollection<User>>(request).Data;
        }
        public ObservableCollection<User> GetAllUsers()
        {
            //var request = new RestRequest("/users/", RestSharp.DataFormat.Json);
            var request = new RestRequest("/users/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            //return client.Get<ObservableCollection<User>>(request).Data;
            return client.Get<ObservableCollection<User>>(request);
        }
        public bool PostCategory(Category category)
        {
            var request = new RestRequest("/categories/", RestSharp.Method.Post);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(category);
            try
            {
                var cat = client.Post<Category>(request);
                category.ID = cat.ID;
                category.OwnerID = cat.OwnerID;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        public bool PutCategory(Category category)
        {
            var request = new RestRequest("/categories/" + category.ID.ToString() + "/", RestSharp.Method.Put);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(category);

            try
            {
                var cat = client.Put<Category>(request);
                category.ID = cat.ID;
                category.OwnerID = cat.OwnerID;
                return true;
            }
            catch(Exception ex) 
            {
                return false;
            }

        }
        public bool DeleteCategory(Category category)
        {
            var request = new RestRequest("/categories/" + category.ID.ToString() + "/", RestSharp.Method.Delete);
            var response = client.Delete(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public bool PostBookmark(Bookmark bookmark)
        {
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/", Method.Post);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(bookmark);

            try
            {
                var bm = client.Post<Category>(request);
                bookmark.ID = bm.ID;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        public bool PostPermission(ICollection<User> users, uint id)
        {
            var request = new RestRequest("/categories/" + id.ToString() + "/permissions/");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(users);

            var response = client.Post(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }
        public bool RemovePermission(ICollection<User> users, uint id)
        {
            var request = new RestRequest("/categories/" + id.ToString() + "/permissions/");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(users);

            var response = client.Delete(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public bool PutBookmark(Bookmark bookmark)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBookmark(Bookmark bookmark)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<User> SearchUser(string username)
        {
            throw new NotImplementedException();
        }
    }
}
