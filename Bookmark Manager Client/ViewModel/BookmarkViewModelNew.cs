using Bookmark_Manager_Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.ViewModel
{
    public class BookmarkViewModelNew : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get => mainViewModel;
            set
            {
                mainViewModel = value;
                Category = MainViewModel.SelectedCategory;
                OnPropertyChanged();
            }
        }

        private Category category;
        public Category Category { get => category; set { category = value; OnPropertyChanged(); } }

        private string url;
        public string Url { get => url; set { grabTitleFromWeb(value); url = value; OnPropertyChanged();  } }
        private string title;
        public string Title { get => title; set { title = value; OnPropertyChanged(); } }

        private string description;
        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        private HttpClient client = new HttpClient();

        public BookmarkViewModelNew()
        {


        }

        private void grabTitleFromWeb(string url)
        {
            if (string.IsNullOrEmpty(url)) return;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () => { await Task.Delay(5000); cancellationTokenSource.Cancel(); });

            var result = Task.Run( async () =>
            {
                
                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(url,cancellationTokenSource.Token))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Title = Regex.Match(responseBody, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>",
                        RegexOptions.IgnoreCase).Groups["Title"].Value;
                    }
                    
                }
                catch (HttpRequestException e)
                {

                }
            });
        }

        public bool SaveBookmark()
        {
            if(Url == "") return false;
            if(Category == null) return false;

            var bookmark = new Bookmark()
            {
                Url = Url,
                CategoryID = Category.ID,
                Title = Title,
                Description = Description
            };

            if(!ObjectRepository.DataProvider.PostBookmark(bookmark)) return false;

            MainViewModel.Bookmarks.Add(bookmark);

            MainViewModel.SetDefaultView();
            return true;
        }
        public void Exit() => MainViewModel.SetDefaultView();
    }
}
