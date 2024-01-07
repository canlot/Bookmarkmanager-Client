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

        public async Task<bool> LoginAsync()
        {
            var options = new RestClientOptions(FullUrl)
            {
                Authenticator = new HttpBasicAuthenticator(UserName, Password, Encoding.UTF8)
            };
            var loginClient = new RestClient(options);
            var request = new RestRequest("/login", Method.Post);
            request.AddHeader("Cache-Control", "no-cache");
            var answer = await loginClient.PostAsync(request);

            options = new RestClientOptions(FullUrl)
            {
                Authenticator = new JwtAuthenticator(answer.Content)
            };
            client = new RestClient(options);
            try
            {
                currentUser = await getCurrentUserAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public RestDataProvider()
        {
            
        }
        public async Task<bool> SetUpConnectionAsync()
        {
            var options = new RestClientOptions(FullUrl)
            {
                Authenticator = new HttpBasicAuthenticator(UserName, Password, Encoding.UTF8)
            };
            client = new RestClient(options);

            //JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            //jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //client.UseNewtonsoftJson(jsonSerializerSettings);
            

            try
            {
                currentUser = await getCurrentUserAsync();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
        private async Task<User> getCurrentUserAsync()
        {
            var request = new RestRequest("/currentuser", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return await client.GetAsync<User>(request);
        }
        public async Task<IList<Category>> GetCategoriesAsync(uint id = 0)
        {
            RestRequest request;
            if(id == 0)
                request = new RestRequest("categories/", Method.Get);
            else
                request = new RestRequest("categories/" + id.ToString() + "/", Method.Get);

            request.AddHeader("Cache-Control", "no-cache");
            var list = await client.GetAsync<List<Category>>(request);
            Console.WriteLine("Hallo");
            return list;
        }

        public async Task<IList<Bookmark>> GetBookmarksAsync( uint id)
        {
            var request = new RestRequest("categories/" + id.ToString() + "/bookmarks/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return await client.GetAsync<List<Bookmark>>(request);
        
        }
        public async Task<IList<User>> GetPermittedUsersAsync(uint id)
        {
            var request = new RestRequest("categories/" + id.ToString() + "/permissions/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return await client.GetAsync<List<User>>(request);
        }
        public async Task<IList<User>> GetAllUsersAsync()
        {
            var request = new RestRequest("/users/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return await client.GetAsync<List<User>>(request);
        }
        public async Task<bool> PostCategoryAsync(Category category)
        {
            var request = new RestRequest("/categories/", RestSharp.Method.Post);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(category);
            try
            {
                var cat = await client.PostAsync<Category>(request);
                category.ID = cat.ID;
                category.OwnerID = cat.OwnerID;
                category.Shared = cat.Shared;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        public async Task<bool> PutCategoryAsync(Category category)
        {
            var request = new RestRequest("/categories/" + category.ID.ToString() + "/", RestSharp.Method.Put);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(category);

            try
            {
                var cat = await client.PutAsync<Category>(request);
                category.ID = cat.ID;
                category.OwnerID = cat.OwnerID;
                return true;
            }
            catch(Exception ex) 
            {
                return false;
            }

        }
        public async Task<bool> DeleteCategoryAsync(uint categoryId)
        {
            var request = new RestRequest("/categories/" + categoryId.ToString() + "/", RestSharp.Method.Delete);
            var response = await client.DeleteAsync(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> PostBookmarkAsync(Bookmark bookmark)
        {
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/", Method.Post);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(bookmark);

            try
            {
                var bm = await client.PostAsync<Category>(request);
                bookmark.ID = bm.ID;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        public async Task<bool> ChangePermissionsAsync(ICollection<User> users, uint id)
        {
            var request = new RestRequest("/categories/" + id + "/permissions/");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(users);

            var response = await client.PutAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public async Task<bool> PutBookmarkAsync(Bookmark bookmark)
        {
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/" + bookmark.ID, Method.Put);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(bookmark);

            try
            {
                var response = await client.PutAsync(request);
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

        public async Task<bool> DeleteBookmarkAsync(Bookmark bookmark)
        {
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/" + bookmark.ID, Method.Delete);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;

            try
            {
                var response = await client.DeleteAsync(request);
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

        public async Task<IList<User>> SearchUsersAsync(string username)
        {
            var request = new RestRequest("/users/search/" + username, Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            try
            {
                var response = await client.GetAsync<List<User>>(request);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IList<Category>> SearchCategoriesAsync(string searchString)
        {
            var request = new RestRequest("/categories/search/" +  searchString, Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            try
            {
                var response = await client.GetAsync<List<Category>>(request);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IList<Bookmark>> SearchBookmarksAsync(string searchString)
        {
            var request = new RestRequest("/bookmarks/search/" + searchString, Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            try
            {
                var response = await client.GetAsync<List<Bookmark>>(request);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
