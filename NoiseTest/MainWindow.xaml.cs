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
            InitializeComponent();
            map.Generate();
            drawMap();
        }
        /*
        private void drawMap()
        {
            int seed = rdm.Next();
            
            map.setScale(5f);
            map.setSeed(seed);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    byte elevation = map.getElevation(x, y);
                    if (map.getElevation(x, y) <= map.getWaterlevel())
                    {
                        col = System.Drawing.Color.FromArgb(255, 0, 0, 100);
                    }
                    else if (map.getElevation(x,y) <= map.getWeedlevel())
                    {
                        col = System.Drawing.Color.FromArgb(255, 0, elevation, 0);
                    }
                    else
                    {
                        col = System.Drawing.Color.FromArgb(255, (byte)(32 + elevation / 1.14), (byte)(32 + elevation / 1.14), (byte)(32 + elevation / 1.14));
                    }
                    bitmap.SetPixel(x, y, col);
                }
            }
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            mapView.Source = imageSource;
        }
        */
        public async void drawMap()
        {
            mapView.Source = await Drawing.Draw(map);
        }


        private void SeedRoll_Click(object sender, RoutedEventArgs e)
        {
            map.Generate();
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
            map.setWeedlevel(wert);
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
            if (e.Key == System.Windows.Input.Key.Return)
            {
                byte wert = (byte)Int32.Parse(weed.Text);
                map.setWeedlevel(wert);
                weedlevel.Value = wert;
                drawMap();
            }
        }
    }
}
