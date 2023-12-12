using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void Start()
        {

        }

        private MenuItem lastSelectedMenuItem;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedMenuItem = sender as MenuItem;

            if (lastSelectedMenuItem != null && lastSelectedMenuItem != clickedMenuItem)
            {
                lastSelectedMenuItem.IsEnabled = true;
            }

            if (clickedMenuItem != null)
            {
                clickedMenuItem.IsEnabled = false;
                lastSelectedMenuItem = clickedMenuItem;
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (lastSelectedMenuItem != null)
            {
                lastSelectedMenuItem.IsEnabled = true;
                lastSelectedMenuItem = null;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
        
    }
}