﻿using System;
using System.Windows;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
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
        
        public void drawMap()
        {
            bitmap = Drawing.Draw(map);
            mapView.Source = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
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
                    byte wert;
                    if (Byte.TryParse(wl.Text, out wert))
                    {
                        //byte wert = Byte.Parse(wl.Text);
                        map.setWaterlevel(wert);
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

        private void Skalierungslevel1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void GenerierKarte_Click(object sender, RoutedEventArgs e)
        {
            if (IsNumberPositiv(SeedEingabe.Text)) // equals a positiv number
            {
                int seed;
                if (Int32.TryParse(SeedEingabe.Text, out seed))
                {
                    map.setElevationSeed(seed);
                    map.GenerateElevation();
                    map.GenerateMoisture();
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
                if (IsNumberPositiv(SeedEingabe.Text)) // equals a positiv number
                {
                    int seed;
                    if (Int32.TryParse(SeedEingabe.Text, out seed))
                    {
                        map.setElevationSeed(seed);
                        map.GenerateElevation();
                        map.GenerateMoisture();
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

        private bool IsNumberPositiv(String numberStr)
        {
            Regex rgx = new Regex(@"^\d+$");
            if (rgx.IsMatch(numberStr)) return true;
            return false;
        }

        private void Skalierungslevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double wert = e.NewValue;
            map.setScale((float)wert);
        }

        private void Skalierungslevel_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            map.GenerateElevation();
            map.GenerateMoisture();
            drawMap();
        }
    }
}
