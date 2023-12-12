using System.IO.Pipes;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.IO.Pipes;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherApp.UserControls;
using MaterialDesignThemes.Wpf;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ShowInTaskbar = true;

            Start();
        }

        public HourlyData hourlyData = new HourlyData();
        public bool isSubscribed;
        public string location;
        public bool isDayNow;
        
        private void Start()
        {
            for (int i = 0; i < 24; i++)
            {
                hourlyData.time.Add($"{i:D2}:00");
                hourlyData.temperature_2m.Add(i);
                hourlyData.apparent_temperature.Add(i);
                hourlyData.cloud_cover.Add(i * 5);
                hourlyData.precipitation_probability.Add(i * 5);
                hourlyData.relative_humidity_2m.Add(i * 5);
                hourlyData.wind_speed_10m.Add(i);
                hourlyData.weather_code.Add(i+i*5);
            }
            DefineCurrentWeather();
            PopulateWeatherCards();
        }

        public void DefineCurrentWeather()
        {
            for (int i = 0; i < 24; i++)
            {
                string givenTime = hourlyData.time[i];

                DateTime currentTime = DateTime.Now;

                if (DateTime.TryParseExact(givenTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedTime))
                {
                    int givenHour = parsedTime.Hour;

                    if (currentTime.Hour == givenHour)
                    {
                        if (currentTime.Hour > 5 && currentTime.Hour < 7)
                        {
                            isDayNow = true;
                        }
                        SetCurrentWeather(i);
                        break;
                    }
                }
            }
        }

        public void SetCurrentWeather(int currentHourIndex)
        {
            CurrentWeather_image.Source = SetWeatherIcon(hourlyData.weather_code[currentHourIndex], isDayNow);
            Temperature_textBox.Text = $"{hourlyData.temperature_2m[currentHourIndex]}°";
            AppearentTemperature_textBox.Text = $"Feels like {hourlyData.apparent_temperature[currentHourIndex]}°";
            CloudCoverLevel_textBox.Text = SetCloudCoverLevel(currentHourIndex);
            Precipitation_textBox.Text = $"Precipitation - {hourlyData.precipitation_probability[currentHourIndex]}%";
            Humidity_textBox.Text = $"Humidity - {hourlyData.relative_humidity_2m[currentHourIndex]}%";
            WindSpeed_textBox.Text = $"Wind speed - {hourlyData.wind_speed_10m[currentHourIndex]} km/h";
        }

        private void PopulateWeatherCards()
        {
            weatherStackPanel.Children.Clear();
            bool isDay;

            if (hourlyData != null)
            {
                for (int i = 0; i < 24; i++)
                {
                    isDay = (i > 5 && i < 7) ? true : false;
                    var card = new WeatherPerHourCard();

                    card.Hour = $"{i:D2}:00";

                    if (i < hourlyData.temperature_2m.Count)
                    {
                        card.Temperature = $"{hourlyData.temperature_2m[i]}°";
                        card.Precipitation = $"{hourlyData.precipitation_probability[i]}%";
                        card.Wind = $"{hourlyData.wind_speed_10m[i]} km/h";
                    }
                    else
                    {
                        card.Temperature = "N/A";
                        card.Precipitation = "N/A";
                        card.Wind = "N/A";
                    }

                    card.Source = SetWeatherIcon(hourlyData.weather_code[i], isDay);

                    weatherStackPanel.Children.Add(card);
                }
            }
        }

        public string SetCloudCoverLevel(int CloudCoverLevel)
        {
            if (CloudCoverLevel <= 30)
            {
                return "Clear sky";
            }
            else if (CloudCoverLevel > 30 && CloudCoverLevel < 60)
            {
                return "Partly cloudy";
            }
            else {
                return "Overcast";
            }
        }

        public BitmapImage SetWeatherIcon(int weatherCode, bool isDay)
        {
            if ((weatherCode == 0 || weatherCode == 1) && isDay)
            {
                return (BitmapImage)FindResource("Sun");
            }
            else if ((weatherCode == 0 || weatherCode == 1) && !isDay)
            {
                return (BitmapImage)FindResource("Moon");
            }
            else if (weatherCode == 2 && isDay)
            {
                return (BitmapImage)FindResource("PartlyCloudyDay");
            }
            else if (weatherCode == 2 && !isDay)
            {
                return (BitmapImage)FindResource("PartlyCloudyNight");
            }
            else if ((weatherCode > 50 && weatherCode < 70) || (weatherCode > 80 && weatherCode < 100))
            {
                return (BitmapImage)FindResource("Rain");
            }
            else if (weatherCode > 70 && weatherCode < 80)
            {
                return (BitmapImage)FindResource("Snow");
            }
            else
            {
                return (BitmapImage)FindResource("Clouds");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (object item in MainMenuItem.Items)
            {
                if (item is MenuItem menuItem)
                {
                    if (!menuItem.IsEnabled)
                    {
                        menuItem.IsEnabled = true;
                    }
                }
            }
            if (sender is MenuItem clickedMenuItem)
            {
                location = clickedMenuItem.Header?.ToString();
                clickedMenuItem.IsEnabled = false;
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e) { }
 
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DefineCurrentWeather();
            PopulateWeatherCards();
        }

        private void btnSubscribe_Click(object sender, RoutedEventArgs e)
        {
            isSubscribed = isSubscribed ? false : true;
        }
    }

    //======================================= Orest's code

    public class WeatherData
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public double elevation { get; set; }

        public HourlyUnits hourly_units { get; set; }
        public HourlyData hourly { get; set; }

        public override string ToString()
        {
            return $"Latitude: {latitude}, Longitude: {longitude}, GenerationTimeMs: {generationtime_ms}, UtcOffsetSeconds: {utc_offset_seconds}, Timezone: {timezone}, TimezoneAbbreviation: {timezone_abbreviation}, Elevation: {elevation}, \nHourlyUnits: {hourly_units}, \nHourly: {hourly}";
        }
    }

    public class HourlyUnits
    {
        public string time { get; set; }
        public string temperature_2m { get; set; }
        public string relative_humidity_2m { get; set; }
        public string apparent_temperature { get; set; }
        public string precipitation_probability { get; set; }
        public string weather_code { get; set; }
        public string cloud_cover { get; set; }
        public string wind_speed_10m { get; set; }

        public HourlyUnits(string givenTime, double givenTemperature, int givenHumidity, double givenAppearTempereture,
            int givenPrecipitation, int givenWeatherCode, int givenCloudCover, int givenWindSpeed)
        {
            time = givenTime;
            temperature_2m = givenTemperature.ToString();
            relative_humidity_2m = givenHumidity.ToString();
            apparent_temperature = givenAppearTempereture.ToString();
            precipitation_probability = givenPrecipitation.ToString();
            weather_code = givenWeatherCode.ToString();
            cloud_cover = givenCloudCover.ToString();
            wind_speed_10m = givenWindSpeed.ToString();
        }

        public override string ToString()
        {
            return $"Time: {time}, Temperature2m: {temperature_2m}, RelativeHumidity2m: {relative_humidity_2m}, ApparentTemperature: {apparent_temperature}, PrecipitationProbability: {precipitation_probability}, WeatherCode: {weather_code}, CloudCover: {cloud_cover}, WindSpeed10m: {wind_speed_10m}";
        }
    }

    public class HourlyData
    {
        public List<string> time { get; set; }
        public List<double> temperature_2m { get; set; }
        public List<int> relative_humidity_2m { get; set; }
        public List<double> apparent_temperature { get; set; }
        public List<int> precipitation_probability { get; set; }
        public List<int> weather_code { get; set; }
        public List<int> cloud_cover { get; set; }
        public List<double> wind_speed_10m { get; set; }

        public HourlyData()
        {
            time = new List<string>();
            temperature_2m = new List<double>();
            relative_humidity_2m = new List<int>();
            apparent_temperature = new List<double>();
            precipitation_probability = new List<int>();
            weather_code = new List<int>();
            cloud_cover = new List<int>();
            wind_speed_10m = new List<double>();
        }

        public override string ToString()
        {
            return $"Time: [{string.Join(", ", time)}], Temperature2m: [{string.Join(", ", temperature_2m)}], RelativeHumidity2m: [{string.Join(", ", relative_humidity_2m)}], ApparentTemperature: [{string.Join(", ", apparent_temperature)}], PrecipitationProbability: [{string.Join(", ", precipitation_probability)}], WeatherCode: [{string.Join(", ", weather_code)}], CloudCover: [{string.Join(", ", cloud_cover)}], WindSpeed10m: [{string.Join(", ", wind_speed_10m)}]";
        }
    }
    public class StartClass
    {
        static public void StartMethod()
        {
            var client = new NamedPipeClientStream("WeatherApp");
            client.Connect();

            StreamReader reader = new StreamReader(client);
            StreamWriter writer = new StreamWriter(client);

            Console.WriteLine("Connected to the server. Type 'exit' to close the client.");

            while (true)
            {
                Console.Write("Enter a message: ");
                string input = Console.ReadLine();

                if (String.IsNullOrEmpty(input))
                {
                    continue;
                }


                else if (input.ToLower() == "exit")
                {
                    Console.WriteLine("Exit");
                    writer.WriteLine(input);
                    writer.Flush();
                    break;
                }

                else if (input.ToLower() == "subscribe")
                {

                    // subscribe
                    Console.WriteLine("Subscribed");
                    writer.WriteLine(input);
                    writer.Flush();
                }

                else if (input.ToLower() == "unsubscribe")
                {
                    // unsubscribe
                    Console.WriteLine("Unsubscribed");
                    writer.WriteLine(input);
                    writer.Flush();
                }

                else if (input.ToLower() == "get")
                {
                    writer.WriteLine(input);
                    writer.Flush();
                    try
                    {
                        string response = reader.ReadLine();
                        WeatherData receivedWeather = JsonConvert.DeserializeObject<WeatherData>(response);
                        Console.WriteLine($"Received Weather Data:\n{receivedWeather}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Got problem with deserealizing. Maybe you unsubscribed???");
                    }

                }
                else if (input.ToLower().StartsWith("citychange"))
                {
                    writer.WriteLine(input);
                    writer.Flush();
                    try
                    {
                        string response = reader.ReadLine();
                        Console.WriteLine($"{response}");
                    }
                    catch
                    {
                        Console.WriteLine("Error");
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input");
                }

            }
            client.Close();
            client.Dispose();
            Console.WriteLine("Client closed.");
        }
    }
}