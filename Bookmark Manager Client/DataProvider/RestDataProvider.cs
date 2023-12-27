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

        public int Port => ObjectRepository.AppConfiguration.Port;
        public string Host => ObjectRepository.AppConfiguration.Host;
        public string UserName => ObjectRepository.AppConfiguration.Email;
        public string Password => ObjectRepository.AppConfiguration.Password;

        private User currentUser;
        public User CurrentUser { get => currentUser; }

        public string FullUrl
        {
            get
            {
                string url = "";
                if (!Host.StartsWith("http://"))
                    url = "http://";
                url = url + Host + ":";
                url += Convert.ToString(Port);
                url += "/apiv1";
                return url;
            }
        }

        public bool TestConnection()
        {
            return false;
        }
        public RestDataProvider()
        {
            
        }
        public bool SetUpConnection()
        {
            var options = new RestClientOptions(FullUrl)
            {
                Authenticator = new HttpBasicAuthenticator(UserName, Password, Encoding.UTF8)
            };
            client = new RestClient(options);

            //JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            //jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //client.UseNewtonsoftJson(jsonSerializerSettings);
            
            Console.WriteLine(FullUrl);

            try
            {
                currentUser = getCurrentUser();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
        private User getCurrentUser()
        {
            var request = new RestRequest("/currentuser", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return client.Get<User>(request);
        }
        public IList<Category> GetCategories(uint id = 0)
        {

            RestRequest request;
            if(id == 0)
                request = new RestRequest("categories/", Method.Get);
            else
                request = new RestRequest("categories/" + id.ToString() + "/", Method.Get);

            request.AddHeader("Cache-Control", "no-cache");
            return client.Get<List<Category>>(request); 
        }
        public IList<Category> GetAllCategories()
        {
            throw new NotImplementedException();
        }
        public IList<Bookmark> GetBookmarks( uint id)
        {
            var request = new RestRequest("categories/" + id.ToString() + "/bookmarks/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return client.Get<List<Bookmark>>(request);
        
        }
        public IList<User> GetPermittedUsers(uint id)
        {
            var request = new RestRequest("categories/" + id.ToString() + "/permissions/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return client.Get<List<User>>(request);
        }
        public IList<User> GetAllUsers()
        {
            var request = new RestRequest("/users/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return client.Get<List<User>>(request);
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
        public bool DeleteCategory(uint categoryId)
        {
            var request = new RestRequest("/categories/" + categoryId.ToString() + "/", RestSharp.Method.Delete);
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
        public bool ChangePermissions(ICollection<User> users, uint id)
        {
            var request = new RestRequest("/categories/" + id.ToString() + "/permissions/");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(users);

            var response = client.Put(request);
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
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/" + bookmark.ID, Method.Put);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(bookmark);

            try
            {
                var response = client.Put(request);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool DeleteBookmark(Bookmark bookmark)
        {
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/" + bookmark.ID, Method.Delete);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;

            try
            {
                var response = client.Delete(request);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IList<User> SearchUser(string username)
        {
            throw new NotImplementedException();
        }

        public IList<Category> SearchCategories(string searchString)
        {
            var request = new RestRequest("/categories/search/" +  searchString, Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            try
            {
                var response = client.Get<List<Category>>(request);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<Bookmark> SearchBookmarks(string searchString)
        {
            var request = new RestRequest("/bookmarks/search/" + searchString, Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            try
            {
                var response = client.Get<List<Bookmark>>(request);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
