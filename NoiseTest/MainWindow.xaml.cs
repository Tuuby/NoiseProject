using System;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;

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


        private void SeedRoll_Click(object sender, RoutedEventArgs e)
        {
            int seed = rdm.Next();
            map.setElevationSeed(seed);
            seed = rdm.Next();
            map.setMoistureSeed(seed);

            map.GenerateElevation();
            map.GenerateMoisture();
            drawMap();
        }


        private void Wasserlevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte wert = (byte)(Wasserlevel.Value);
            wl.Text = wert.ToString();
            map.setWaterlevel(wert);
        }

        private void Weedlevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte wert = (byte)(weedlevel.Value);
            weed.Text = wert.ToString();
            //map.setWeedlevel(wert);
            //drawMap();
        }

        private void Wasserlevel_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            drawMap();
        }

        private void Weedlevel_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            drawMap();
        }

        private void Wl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                byte wert = (byte)Int32.Parse(wl.Text);
                map.setWaterlevel(wert);
                Wasserlevel.Value = wert;
                drawMap();
            }
        }

        
        private void Weed_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            /*
            if (e.Key == System.Windows.Input.Key.Return)
            {
                byte wert = (byte)Int32.Parse(weed.Text);
                map.setWeedlevel(wert);
                weedlevel.Value = wert;
                drawMap();
            }
            */
        }
        
    }
}
