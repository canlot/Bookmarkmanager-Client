using Bookmark_Manager_Client.Model;
using Bookmark_Manager_Client.Utils;
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
    public class BookmarkViewModelEdit : INotifyPropertyChanged
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
                Url = MainViewModel.SelectedBookmark.Url;
                Title = MainViewModel.SelectedBookmark.Title;
                Description = MainViewModel.SelectedBookmark.Description;
                OnPropertyChanged();
            }
        }

        private Category category;
        public Category Category { get => category; set { category = value; OnPropertyChanged(); } }

        private string url;
        public string Url { get => url; set { if (Title == "") grabTitleFromWeb(value); url = value; OnPropertyChanged(); } }
        private string title;
        public string Title { get => title; set { title = value; OnPropertyChanged(); } }

        private string description;
        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        private bool isWebLoading;

        public bool IsWebLoading { get => isWebLoading; set { isWebLoading = value; OnPropertyChanged(); } }

        private HttpClient client = new HttpClient();

        private void grabTitleFromWeb(string url)
        {
            if (string.IsNullOrEmpty(url)) return;

            IsWebLoading = true;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () => { await Task.Delay(5000); cancellationTokenSource.Cancel(); });
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
            var result = Task.Run(async () =>
            {

                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(url, cancellationTokenSource.Token))
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
                finally { cancellationTokenSource.Cancel(); IsWebLoading = false; }
            });
        }
        public async Task<bool> UpdateBookmarkAsync()
        {
            if (Url == "") return false;
            if (Category == null) return false;

            MainViewModel.SelectedBookmark.Url = Url;
            MainViewModel.SelectedBookmark.Title = Title;
            MainViewModel.SelectedBookmark.Description = Description;


            if (!await ObjectRepository.DataProvider.ChangeBookmarkAsync(MainViewModel.SelectedBookmark)) return false;
            try
            {
                var iconPath = IconUtils.DownloadIcon(Url);
                await ObjectRepository.DataProvider.UploadIconAsync(MainViewModel.SelectedBookmark, iconPath);
            }
            catch (Exception e) 
            {

            }

            var bookmark = MainViewModel.Bookmarks.Single(x => x.ID == MainViewModel.SelectedBookmark.ID);

            MainViewModel.SetDefaultView();
            return true;
        }
        public void Exit() => MainViewModel.SetDefaultView();
    }
}
