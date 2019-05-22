using System;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace NoiseTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap map2 = new Bitmap(500, 500);
        
        Color col = new Color();
        Random rdm = new Random();
        public MainWindow()
        {
            InitializeComponent();
            drawMap();
        }

        private void drawMap()
        {
            int seed = rdm.Next();
            Map map = new Map(500, 500, seed);
            map.setScale(5f);
            map.setWaterlevel(65);

            map.Generate();
            for (int x = 0; x < map2.Width; x++)
            {
                for (int y = 0; y < map2.Height; y++)
                {
                    byte elevation = map.getElevation(x, y);
                    if (map.getElevation(x, y) <= map.getWaterlevel())
                    {
                        col = Color.FromArgb(255, 0, 0, 100);
                    } else
                    {
                        col = Color.FromArgb(255, (byte)(32 + elevation / 1.14), (byte)(32 + elevation / 1.14), (byte)(32 + elevation / 1.14));
                    }
                    map2.SetPixel(x, y, col);
                }
            }
            FileStream fs = new FileStream(@"C:\Users\phant\Desktop\map.png", FileMode.Create);
            map2.Save(fs, ImageFormat.Png);
        }

        private void SeedRoll_Click(object sender, RoutedEventArgs e)
        {
            //MapCanvas.Children.Clear();
            drawMap();
        }
    }
}
