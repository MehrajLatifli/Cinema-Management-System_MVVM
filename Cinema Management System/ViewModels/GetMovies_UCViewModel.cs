using CefSharp;
using Cinema_Management_System.Command;
using Cinema_Management_System.Models;
using Cinema_Management_System.Views.UserControls;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cinema_Management_System.ViewModels
{
    public class GetMovies_UCViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnpropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        public GetMovies_UC GetMovies_UCs { get; set; }

        public RelayCommand SearchMoviesCommand { get; set; }

        public RelayCommand AddMoviesCommand { get; set; }
        public RelayCommand YourAccounCommand { get; set; }

        public RelayCommand SelectedItemChangedCommand { get; set; }


        public List<MoviesfromOMDbAPI> _MoviesfromOMDbAPI_List { get; set; }
        public List<MoviesfromOMDbAPI> MoviesfromOMDbAPI_List { get { return _MoviesfromOMDbAPI_List; } set { _MoviesfromOMDbAPI_List = value; OnpropertyChanged(); } }

        public List<MovieList> _MovieListInGetMovies { get; set; }
        public List<MovieList> MovieListInGetMovies { get { return _MovieListInGetMovies; } set { _MovieListInGetMovies = value; OnpropertyChanged(); } }

        private MoviesfromOMDbAPI _movie;

        public MoviesfromOMDbAPI Movie { get { return _movie; } set { _movie = value; OnpropertyChanged(); } }


        private MovieList _movie2;

        public MovieList Movie2 { get { return _movie2; } set { _movie2 = value; OnpropertyChanged(); } }


        public dynamic Data { get; set; }

        HttpClient HttpClient = new HttpClient();
        HttpResponseMessage responseMessage = new HttpResponseMessage();

        ObservableCollection<MoviesfromOMDbAPI> AddMoviesList = new ObservableCollection<MoviesfromOMDbAPI>();

        ObservableCollection<MoviesfromOMDbAPI> AddCinemaList = new ObservableCollection<MoviesfromOMDbAPI>();



        DateTime d = DateTime.Now;

        public string a = "";

        StringBuilder stringBuilder = new StringBuilder();

        string OMDbAPI_Key = ConfigurationManager.AppSettings["OMDbAPI_Key"];
        public GetMovies_UCViewModel()
        {


            SearchMoviesCommand = new RelayCommand((e) =>
            {
                var name = GetMovies_UCs.SearchTextBox.Text;
                responseMessage = HttpClient.GetAsync($@"http://www.omdbapi.com/?plot=full&apikey={OMDbAPI_Key}&s={name}").Result;
                var str = responseMessage.Content.ReadAsStringAsync().Result;

                Data = JsonConvert.DeserializeObject(str);





                try
                {





                    if (Data.Search != null)
                    {

                        foreach (var movieLoop in Data.Search)
                        {


                            responseMessage = HttpClient.GetAsync($@"http://www.omdbapi.com/?plot=full&apikey=30169e52&i={movieLoop.imdbID}").Result;
                            var str2 = responseMessage.Content.ReadAsStringAsync().Result;

                            dynamic Data2 = JsonConvert.DeserializeObject(str2);

                            if (Data2.Poster != "N/A")
                            {

                                AddMoviesList.Add(new MoviesfromOMDbAPI()
                                {

                                    Title = $" {Data2.Title}",
                                    Year = $" Year: {Data2.Year}",
                                    Poster = Data2.Poster,
                                    imdbRating = $" imdbRating: {Data2.imdbRating}",
                                    Genre = $" Genre: {Data2.Genre}",
                                    Plot = Data2.Plot,
                                    Rated = Data2.Rated,
                                    Released = Data2.Released,
                                    Runtime = Data2.Runtime,
                                    Director = Data2.Director,
                                    Writer = Data2.Writer,
                                    Actors = Data2.Actors,
                                    Language = Data2.Language,
                                    Country = Data2.Country,
                                    Awards = $" Awards: {Data2.Awards}",
                                    Metascore = Data2.Metascore,
                                    imdbVotes = Data2.imdbVotes,

                                    Type = Data2.Type,
                                    Response = Data2.Response,
                                });
                            }

                            if (Data2.Poster == "N/A")
                            {

                                AddMoviesList.Add(new MoviesfromOMDbAPI()
                                {

                                    Title = $" {Data2.Title}",
                                    Year = $" Year: {Data2.Year}",
                                    Poster = "../../Images/Logo/No Image.png",
                                    imdbRating = $" imdbRating: {Data2.imdbRating}",
                                    Genre = $" Genre: {Data2.Genre}",
                                    Plot = Data2.Plot,
                                    Rated = Data2.Rated,
                                    Released = Data2.Released,
                                    Runtime = Data2.Runtime,
                                    Director = Data2.Director,
                                    Writer = Data2.Writer,
                                    Actors = Data2.Actors,
                                    Language = Data2.Language,
                                    Country = Data2.Country,
                                    Awards = Data2.Awards,
                                    Metascore = Data2.Metascore,
                                    imdbVotes = Data2.imdbVotes,

                                    Type = Data2.Type,
                                    Response = Data2.Response,
                                });
                            }
                            GetMovies_UCs.listbox1.ItemsSource = AddMoviesList;











                        }
                    }

                    else
                    {
                        MessageBox.Show($" There is not  \"{name}\"  in OMDb API");
                    }


                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }




            });


            SelectedItemChangedCommand = new RelayCommand((e) =>
            {



                //MessageBox.Show($"{addMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Title}");
                SearchResultVideos(AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Title, AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Year).GetAwaiter();


            });


            AddMoviesCommand = new RelayCommand((e) =>
            {
                int count = 2;

                if (!string.IsNullOrEmpty(GetMovies_UCs.SearchTextBox.Text))
                {
                    AddCinemaList.Add(new MoviesfromOMDbAPI
                    {
                        ID = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].ID,
                        Title = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Title,
                        Year = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Year,
                        Poster = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Poster,
                        imdbRating = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].imdbRating,
                        Genre = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Genre,
                        Plot = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Plot,
                        Rated = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Rated,
                        Released = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Released,
                        Runtime = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Runtime,
                        Director = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Director,
                        Writer = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Writer,
                        Actors = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Actors,
                        Language = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Language,
                        Country = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Country,
                        Awards = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Awards,
                        Metascore = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Metascore,
                        imdbVotes = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].imdbVotes,
                        Type = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Type,
                        Response = AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Response,
                    });



                    notifier.ShowSuccess($"You select: {AddMoviesList[GetMovies_UCs.listbox1.SelectedIndex].Title}");


                    SaveAddCinemaList();
                }
                else
                {
                    MessageBox.Show($"Search box is empty.");

                }


                //   MessageBox.Show($"{m[GetMovies_UCs.listbox1.SelectedIndex].Year}");
            });
        }



        private string _videoId;
        private async Task SearchResultVideos(string movieName, string year)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = ConfigurationManager.AppSettings["YoutubeAPI_Key"],
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = $"{movieName} {year} Official Trailer"; // Replace with your search term.
            searchListRequest.MaxResults = 1;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            string v = "";

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        //   videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        videos.Add(searchResult.Id.VideoId);
                        break;

                    case "youtube#channel":
                        channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                        break;
                }
            }

            v = videos[0];


            // MessageBox.Show($" \n www./youtu.be/{v}");



            if (GetMovies_UCs.ChromiumBrowser.Address == null)
            {

                GetMovies_UCs.Stackyoutubewb.Visibility = Visibility.Visible;
                GetMovies_UCs.ChromiumBrowser.Address = $@"https://www.youtube.com/embed/{v}";
            }


            if (GetMovies_UCs.ChromiumBrowser.Address != null)
            {

                GetMovies_UCs.ChromiumBrowser.Address = string.Empty;
                GetMovies_UCs.ChromiumBrowser.Reload();
                GetMovies_UCs.ChromiumBrowser.Address = $@"https://www.youtube.com/embed/{v}";






            }







        }


        public void SaveAddCinemaList()
        {


            //  MessageBox.Show($"{AdminPanel_UCs.Name.Text}");

            try
            {
                if (!Directory.Exists("../../DataBase/CinemaData"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData");
                }
                if (!Directory.Exists("../../DataBase/CinemaData/AddCinemaList"))
                {
                    Directory.CreateDirectory("../../DataBase/CinemaData/AddCinemaList");
                }


                if (File.Exists($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml"))
                {
                    File.Delete($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml");

                }


                var xml = new XmlSerializer(typeof(ObservableCollection<MoviesfromOMDbAPI>));
                using (var fs = new FileStream($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, AddCinemaList);
                }


                MoviesfromOMDbAPI adminAccount = null;

                var xml2 = new XmlSerializer(typeof(ObservableCollection<MoviesfromOMDbAPI>));

                using (var fs2 = new FileStream($@"../../DataBase/CinemaData/AddCinemaList/AddCinemaList {d.ToString("MM dd yyyy")}.xml", FileMode.OpenOrCreate))
                {
                    adminAccount = xml2.Deserialize(fs2) as MoviesfromOMDbAPI;

                    foreach (var item in AddCinemaList)
                    {
                        a = $"{item.Year} \n {item.Title} \n {item.Poster} \n {item.imdbRating} \n {item.Genre} \n {item.Plot}";

                        stringBuilder.Append(a);

                        a = stringBuilder.ToString();
                    }


                }
            }
            catch (Exception)
            {

            }


        }

    }









}
