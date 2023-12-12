using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherApp.UserControls
{
    /// <summary>
    /// Interaction logic for WeatherPerHourCard.xaml
    /// </summary>
    public partial class WeatherPerHourCard : UserControl
    {
        public WeatherPerHourCard()
        {
            InitializeComponent();
        }
        public string Hour
        {
            get { return (string)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }
        public static readonly DependencyProperty HourProperty = DependencyProperty.Register("Hour", typeof(string), typeof(WeatherPerHourCard));

        public BitmapImage Source
        {
            get { return (BitmapImage)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(BitmapImage), typeof(WeatherPerHourCard));

        public string Temperature
        {
            get { return (string)GetValue(TemperatureProperty); }
            set { SetValue(TemperatureProperty, value); }
        }
        public static readonly DependencyProperty TemperatureProperty = DependencyProperty.Register("Temperature", typeof(string), typeof(WeatherPerHourCard));

        public string Precipitation
        {
            get { return (string)GetValue(PrecipitationProperty); }
            set { SetValue(PrecipitationProperty, value); }
        }
        public static readonly DependencyProperty PrecipitationProperty = DependencyProperty.Register("Precipitation", typeof(string), typeof(WeatherPerHourCard));

        public string Wind
        {
            get { return (string)GetValue(WindProperty); }
            set { SetValue(WindProperty, value); }
        }
        public static readonly DependencyProperty WindProperty = DependencyProperty.Register("Wind", typeof(string), typeof(WeatherPerHourCard));

    }
}
