using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
//using static WeatherApp.Weather;
//using static WeatherApp.Weather.WeatherData;
using static WeatherApp.WeatherData;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        //public bool IsRefreshing { get; set; }

        //public ICommand RefreshCommand { get; set; }

        public OpenWeatherData Weather { get; set; }

        public MainPage()
        {
            InitializeComponent();


            //RefreshCommand = new Command(() =>
            //{
            //    IsRefreshing = false;
            //});
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = await GetWeather();

        }

      
        async private Task<OpenWeatherData> GetWeather()
        {
            var details = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (details != PermissionStatus.Granted)
            {
                var newdetail = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            var location = await Geolocation.GetLocationAsync();
            var latitude = location.Latitude;
            var longitude = location.Longitude;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid=cd154b8d364ddbf23fb1135e0b44b6af";
            var response = await client.GetStringAsync(url);
            var weatherData = JsonConvert.DeserializeObject<OpenWeatherData>(response);
            return weatherData;
        }
    }
}
