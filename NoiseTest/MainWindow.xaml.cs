using System;
using System.Windows;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;
using Microsoft.Win32;

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

            map.SetScale(5f);
            map.SetElevationSeed(seed);
            seed = rdm.Next();
            map.SetMoistureSeed(seed);

            InitializeComponent();
            map.GenerateElevation();
            map.GenerateMoisture();
            map.DistributeTrees();

            map.SetWaterlevel((byte) Wasserlevel.Value); // default value for slider -> textbox will be automatically updated
            map.SetWeedlevel(250);

            drawMap();
        }
        
        public void drawMap()
        {
            bitmap = Drawing.Draw(map);
            mapView.Source = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }


        private void SeedRoll_Click(object sender, RoutedEventArgs e) //multiple Seeds?
        {
            int seed = rdm.Next();
            map.SetElevationSeed(seed);
            //insert value in Textbox
            SeedEingabe.Text = seed.ToString();
            seed = rdm.Next();
            map.SetMoistureSeed(seed);
        }


        private void Wasserlevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte wert = (byte)e.NewValue;
            wl.Text = wert.ToString();
            map.SetWaterlevel(wert);
            if (Math.Abs(e.NewValue - e.OldValue) >= 20)
            {
                drawMap();
            }
        }

        private void Wasserlevel_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            drawMap();
        }

        private void Wl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (IsStringAPositivNumber(wl.Text)) // equals a positiv number
                {
                    byte wert;
                    if (Byte.TryParse(wl.Text, out wert))
                    {
                        //byte wert = Byte.Parse(wl.Text);
                        map.SetWaterlevel(wert);
                        Wasserlevel.Value = wert;
                        drawMap();
                    }
                    else
                    {
                        wl.Text = "0";
                        MessageBox.Show("Die eingegebene Zahl ist zu groß", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    wl.Text = "0";
                    MessageBox.Show("Dieses Feld akzeptiert nur positive Zahlen", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GenerierKarte_Click(object sender, RoutedEventArgs e)
        {
            if (IsStringAPositivNumber(SeedEingabe.Text)) // equals a positiv number
            {
                int seed;
                if (Int32.TryParse(SeedEingabe.Text, out seed))
                {
                    map.SetElevationSeed(seed);
                    map.GenerateElevation();
                    map.GenerateMoisture();
                    map.DistributeTrees();
                    if (islandCheck.IsChecked ?? false)
                    {
                        map.MakeIsland();
                    }
                    drawMap();
                }
                else
                {
                    SeedEingabe.Text = "0";
                    MessageBox.Show("Die eingegebene Zahl ist zu groß", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                SeedEingabe.Text = "0";
                MessageBox.Show("Dieses Feld akzeptiert nur positive Zahlen", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SeedEingabe_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // setze eingegebenen Seed in Map Elevation
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (IsStringAPositivNumber(SeedEingabe.Text)) // equals a positiv number
                {
                    int seed;
                    if (Int32.TryParse(SeedEingabe.Text, out seed))
                    {
                        map.SetElevationSeed(seed);
                        map.GenerateElevation();
                        map.GenerateMoisture();
                        map.DistributeTrees();
                        if (islandCheck.IsChecked ?? false)
                        {
                            map.MakeIsland();
                        }
                        drawMap();
                    }
                    else
                    {
                        SeedEingabe.Text = "0";
                        MessageBox.Show("Die eingegebene Zahl ist zu groß", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    SeedEingabe.Text = "0";
                    MessageBox.Show("Dieses Feld akzeptiert nur positive Zahlen", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsStringAPositivNumber(String numberStr)
        {
            Regex rgx = new Regex(@"^\d+$");
            return rgx.IsMatch(numberStr);
        }
        private void Skalierungslevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double wert = e.NewValue;
            Skalierung_Textbox.Text = String.Format("{0:N2}", wert);
            map.SetScale((float)wert);
            if (Math.Abs(e.NewValue - e.OldValue) >= 1)
            {
                map.GenerateElevation();
                map.GenerateMoisture();
                map.DistributeTrees();
                if (islandCheck.IsChecked ?? false)
                {
                    map.MakeIsland();
                }
                drawMap();
            }
        }

        private void Skalierungslevel_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            map.GenerateElevation();
            map.GenerateMoisture();
            map.DistributeTrees();
            if (islandCheck.IsChecked ?? false)
            {
                map.MakeIsland();
            }
            drawMap();
        }

        private void Skalierung_Textbox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (new Regex(@"[1-5]([.,]\d+)?").IsMatch(Skalierung_Textbox.Text))
                {
                    double scale;
                    if (Double.TryParse(Skalierung_Textbox.Text, out scale)) // <- 4.5 interpreted as 45
                    {
                        if (scale <= 5) // need for better regex fpor blocking valuesgreater than 5
                        {
                            map.SetScale((float)scale);
                            map.GenerateElevation();
                            map.GenerateMoisture();
                            map.DistributeTrees();
                            if (islandCheck.IsChecked ?? false)
                            {
                                map.MakeIsland();
                            }
                            drawMap();
                        }
                        else
                        {
                            Skalierung_Textbox.Text = "1";
                            MessageBox.Show("Dieses Feld akzeptiert nur Zahlen zwischen 1 und 5", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        Skalierung_Textbox.Text = "1";
                        MessageBox.Show("Dieses Feld akzeptiert nur Zahlen zwischen 1 und 5", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    Skalierung_Textbox.Text = "1";
                    MessageBox.Show("Dieses Feld akzeptiert nur Zahlen zwischen 1 und 5", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void IslandCheck_Checked(object sender, RoutedEventArgs e)
        {
            map.MakeIsland();
            drawMap();
        }

        private void IslandCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            map.GenerateElevation();
            drawMap();
        }

        private void Button_Speichern_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            // saveFileDialog.Filter = "Name in der List | Dateiformat";
            saveFileDialog.Filter = "Bitmap (*.BMP)|*.BMP |JPG-Image (*.JPG)|*.JPG |Portable Network Grafic (*.PNG)|*.PNG |Graphics Interchange Format (*.GIF)|*.GIF"+ 
                                    "|Portable Document Format (*.PDF)|*.PDF";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.DefaultExt = "bmp";
            if (saveFileDialog.ShowDialog() == true)
                bitmap.Save(saveFileDialog.FileName);
        }

        private void TextBox_LandschaftSt_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (new Regex(@"^[-]?\d+$").IsMatch(TextBox_LandschaftSt.Text)) // equals a number (negativ and/or with digits after a , or . )
                {
                    int wert;
                    if (Int32.TryParse(TextBox_LandschaftSt.Text, out wert))
                    {
                        if (wert <= 100 && wert >= -100)
                        {
                            map.SetElevationDifferenz(wert);
                            map.GenerateElevation();
                            //map.DistributeTrees();
                            if (islandCheck.IsChecked ?? false)
                            {
                                map.MakeIsland();
                            }
                            drawMap();
                        }
                        else
                        {
                            TextBox_LandschaftSt.Text = "0";
                            MessageBox.Show("Diese Feld akzeptiert nur ganzzahlige Zahlen im Intervall von -100 bis 100", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        TextBox_LandschaftSt.Text = "0";
                        MessageBox.Show("Diese Feld akzeptiert nur ganzzahlige Zahlen im Intervall von -100 bis 100", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    TextBox_LandschaftSt.Text = "0";
                    MessageBox.Show("Diese Feld akzeptiert nur ganzzahlige Zahlen im Intervall von -100 bis 100", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Slider_LandschaftSt_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int wert = (int)e.NewValue;
            TextBox_LandschaftSt.Text = wert.ToString();
            map.SetElevationDifferenz(wert);
        }

        private void Slider_LandschaftSt_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            map.GenerateElevation();
            if (islandCheck.IsChecked ?? false)
            {
                map.MakeIsland();
            }
            drawMap();
        }
    }
}
