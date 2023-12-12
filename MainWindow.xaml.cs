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
        public HourlyUnits hourlyUnit = new HourlyUnits();

        private void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                hourlyData.temperature_2m.Add(i);
            }
            for (int i = 0; i < 10; i++)
            {
                hourlyData.precipitation_probability.Add(i * 5);
            }
            for (int i = 0; i < 10; i++)
            {
                hourlyData.wind_speed_10m.Add(i);
            }

            PopulateWeatherCards();
        }


        private void PopulateWeatherCards()
        {
            if (hourlyData != null)
            {
                for (int i = 0; i < 10; i++)
                {
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

                    string[] weatherConditions = new string[]
                    {
                        "PartlyCloudyDay", "PartlyCloudyNight", "Moon", "Clouds", "Sun"
                    };

                    if (i < weatherConditions.Length)
                    {
                        card.Source = (BitmapImage)FindResource(weatherConditions[i]);
                    }
                    else
                    {
                        card.Source = (BitmapImage)FindResource("PartlyCloudyDay");
                    }

                    weatherStackPanel.Children.Add(card);
                }
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
                clickedMenuItem.IsEnabled = false;
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {

        }
            private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
        
    }

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