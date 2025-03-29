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
using System.Threading;
using System.Net.Http;
using System.Windows.Documents;
using System.Net.Http.Headers;
using CefSharp.DevTools.Network;
using CefSharp;
using System.IO;
using Bookmark_Manager_Client.Utils;
using System.Diagnostics;
using System.Text.Json;
using RestSharp.Serializers.Json;
using HandyControl.Tools.Extension;

namespace Bookmark_Manager_Client.DataProvider 
{
    public class DateTimeConverterUsingDateTimeParse : System.Text.Json.Serialization.JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            var stringDate = reader.GetString();
            return DateTime.Parse(stringDate);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
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
                    url = "https://";
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
            //var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            //jsonSerializerOptions.Converters.Add(new DateTimeConverterUsingDateTimeParse());



            //client = new RestClient(options, configureSerialization: s => s.UseSystemTextJson(jsonSerializerOptions));
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

        private async Task<User> getCurrentUserAsync()
        {
            var request = new RestRequest("/currentuser", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return await client.GetAsync<User>(request);
        }

        private async Task<RestResponse<T>> MakeRequestAsync<T>(Func<Task<RestResponse<T>>> func)
        {
            retry:
            var result = await func();
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
            {
                goto retry;
            }
            return result;
        }

        private async Task<RestResponse<T>> MakeRequestAsync<T>(RestRequest request)
        {
            int retryMax = 1;
            int trys = 0;

            while(true)
            {
                var response = await client.ExecuteAsync<T>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if(trys < retryMax)
                    {
                        await LoginAsync();
                        trys++;
                        continue;
                    }
                    else
                        return response;
                }
                
                return response;
            }
            
        }
        private async Task<RestResponse> MakeRequestAsync(RestRequest request)
        {
            int retryMax = 1;
            int trys = 0;

            while(true)
            {
                var response = await client.ExecuteAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (trys < retryMax)
                    {
                        await LoginAsync();
                        trys++;
                        continue;
                    }
                    else
                        return response;
                }
                return response;
            }
        }
        public async Task<IList<Category>> GetCategoriesAsync(uint id = 0)
        {
            RestRequest request;
            if (id == 0)
                request = new RestRequest("categories/", Method.Get);
            else
                request = new RestRequest("categories/" + id.ToString() + "/", Method.Get);

            request.AddHeader("Cache-Control", "no-cache");
            return (await MakeRequestAsync<List<Category>>(request)).Data;

        }

        public async Task<IList<Bookmark>> GetBookmarksAsync( uint id)
        {
            var request = new RestRequest("categories/" + id.ToString() + "/bookmarks/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            var response = await MakeRequestAsync<List<Bookmark>>(request);
            return response.Data;
        
        }
        public async Task<IList<User>> GetPermittedUsersAsync(uint id)
        {
            var request = new RestRequest("categories/" + id.ToString() + "/permissions/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return (await MakeRequestAsync<List<User>>(request)).Data;
        }
        public async Task<IList<User>> GetAllUsersAsync()
        {
            var request = new RestRequest("/users/", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            return (await MakeRequestAsync<List<User>>(request)).Data;
        }
        public async Task<bool> AddCategoryAsync(Category category)
        {
            var request = new RestRequest("/categories/", RestSharp.Method.Post);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(category);

            var response = await MakeRequestAsync<Category>(request);
            if (response.IsSuccessful)
            {
                category.ID = response.Data.ID;
                category.OwnerID = response.Data.OwnerID;
                category.Shared = response.Data.Shared;
                return true;
            }
            return false;
            
        }
        public async Task<bool> ChangeCategoryAsync(Category category)
        {
            var request = new RestRequest("/categories/" + category.ID.ToString() + "/", RestSharp.Method.Put);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(category);

            var response = await MakeRequestAsync<Category>(request);
            if (response.IsSuccessful)
            {
                category.ID = response.Data.ID;
                category.OwnerID = response.Data.OwnerID;
                category.Shared = response.Data.Shared;
                return true;
            }
            return false;

        }
        public async Task<bool> DeleteCategoryAsync(uint categoryId)
        {
            var request = new RestRequest("/categories/" + categoryId.ToString() + "/", RestSharp.Method.Delete);
            var response = await MakeRequestAsync(request);
            if(response.IsSuccessful)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> AddBookmarkAsync(Bookmark bookmark)
        {
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/", Method.Post);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(bookmark);

            var response = await MakeRequestAsync<Category>(request);
            if (response.IsSuccessful)
            {
                bookmark.ID = response.Data.ID;
                return true;
            }
            return false;
            
        }
        public async Task<bool> ChangePermissionsAsync(ICollection<User> users, Category category)
        {
            var request = new RestRequest("/categories/" + category.ID + "/permissions/");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(users);

            var response = await MakeRequestAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
            
        }

        public async Task<bool> ChangeBookmarkAsync(Bookmark bookmark)
        {
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/" + bookmark.ID, Method.Put);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(bookmark);

            var response = await MakeRequestAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;

        }

        public async Task<bool> DeleteBookmarkAsync(Bookmark bookmark)
        {
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/" + bookmark.ID, Method.Delete);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;

            
            try
            {
                var response = await MakeRequestAsync(request);

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
        public async Task<bool> UploadIconAsync(Bookmark bookmark, string iconPath)
        {
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/" + bookmark.ID + "/icon", Method.Post);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "image/png");
            var iconbytes = File.ReadAllBytes(iconPath);
            request.AddBody(iconbytes,ContentType.Binary);
            
            request.RequestFormat = DataFormat.Binary;


            try
            {
                var response = await MakeRequestAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var iconName = IconUtils.ComputeIconHash(iconPath) + ".png";
                    var iconPathNew = ObjectRepository.AppConfiguration.IconsPath + @"\" + iconName;
                    try
                    {
                        if(!File.Exists(iconPathNew))
                            File.Move(iconPath, iconPathNew);
                        bookmark.IconName = iconName;
                    }
                    catch
                    {

                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DownloadIconAsync(Bookmark bookmark)
        {
            if (bookmark.IconName == null || bookmark.IconName == "") return false;

            if (File.Exists(bookmark.IconPath)) return true;
                    
            var request = new RestRequest("/categories/" + bookmark.CategoryID + "/bookmarks/" + bookmark.ID + "/icon", Method.Get);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Binary;

            try
            {
                var response = await MakeRequestAsync<byte[]>(request);

                if (!response.IsSuccessStatusCode)
                    return false;

                if (!File.Exists(bookmark.IconPath))
                    File.WriteAllBytes(bookmark.IconPath, response.RawBytes);

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
                var response = await MakeRequestAsync<List<User>>(request);
                var handler = new ResponseHandler<List<User>>(response);
                if (handler.Handle())
                    return handler.SuccesfullResponseObject;
                else
                    return null;
                
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
                var response = await MakeRequestAsync<List<Category>>(request);

                if (response.IsSuccessStatusCode)
                    return response.Data;
                else
                    return null;
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
                var response = await MakeRequestAsync<List<Bookmark>>(request);

                if (response.IsSuccessStatusCode)
                    return response.Data;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> MoveBookmarkAsync(Category categorySource, Category categoryDestination, Bookmark bookmark)
        {
            var request = new RestRequest("/categories/" + categorySource.ID + "/bookmarks/" + bookmark.ID + "/to/" +categoryDestination.ID, Method.Put);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;

            try
            {
                var response = await MakeRequestAsync(request);

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

        public async Task<bool> AddUserAsync(User user, string password)
        {
            var request = new RestRequest("/users/" + password, Method.Post);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(user);

            try
            {
                var response = await MakeRequestAsync<User>(request);

                if (response.IsSuccessStatusCode)
                {
                    user.ID = response.Data.ID;
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ChangeUserAsync(User user, string password = "")
        {
            var request = new RestRequest("/users/" + user.ID, Method.Put);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(user);

            try
            {
                var response = await MakeRequestAsync<User>(request);

                if (!response.IsSuccessStatusCode)
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }

            if (password == "")
                return true;

            request = new RestRequest("/users/" + user.ID + "/" + password, Method.Put);
            request.AddHeader("Cache-Control", "no-cache");
            request.RequestFormat = DataFormat.Json;

            try
            {
                var response = await MakeRequestAsync(request);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> DeleteUserAsync(uint userId)
        {
            var request = new RestRequest("/users/" +  userId, Method.Delete);
            request.AddHeader("Cache-Control", "no-cache");
            
            try
            {
                var response = await MakeRequestAsync(request);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
