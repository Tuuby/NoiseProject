using System;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NoiseTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap bitmap = new Bitmap(500, 500);
        Map map = new Map(500, 500);
        Random rdm = new Random();

        public MainWindow()
        {
            int seed = rdm.Next();

            map.setScale(5f);
            map.setElevationSeed(seed);
            seed = rdm.Next();
            map.setMoistureSeed(seed);

            InitializeComponent();
            map.GenerateElevation();
            map.GenerateMoisture();
            drawMap();
        }
        
        public async void drawMap()
        {
            mapView.Source = await Drawing.Draw(map);
        }


        private void SeedRoll_Click(object sender, RoutedEventArgs e) //multiple Seeds?
        {
            int seed = rdm.Next();
            map.setElevationSeed(seed);
            //insert value in Textbox
            SeedEingabe.Text = seed.ToString();
            seed = rdm.Next();
            map.setMoistureSeed(seed);
        }


        private void Wasserlevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte wert = (byte)(Wasserlevel.Value);
            wl.Text = wert.ToString();
            map.setWaterlevel(wert);
        }

        private void Wasserlevel_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            drawMap();
        }

        private void Wl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (IsNumberPositiv(wl.Text)) // equals a positiv number
                {
                    byte wert = (byte)Int32.Parse(wl.Text);
                    map.setWaterlevel(wert);
                    Wasserlevel.Value = wert;
                    drawMap();
                }
                else wl.Text = "0";
            }
        }

        private void Skalierungslevel1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void GenerierKarte_Click(object sender, RoutedEventArgs e)
        {
            map.GenerateElevation();
            map.GenerateMoisture();
            drawMap();
        }

        private void SeedEingabe_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // setze eingegebenen Seed in Map Elevation
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (IsNumberPositiv(SeedEingabe.Text)) // equals a positiv number
                {
                    int seed = Int32.Parse(SeedEingabe.Text);
                    map.setElevationSeed(seed);
                }
                else SeedEingabe.Text = "0";// + Fehlermeldung
            }
        }

        private bool IsNumberPositiv(String numberStr)
        {
            Regex rgx = new Regex(@"^\d+$");
            if (rgx.IsMatch(numberStr)) return true;
            return false;
        }
    }
}
