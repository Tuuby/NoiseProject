﻿using System;
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

        private void Slider_GebirgeStauchen_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            map.compressingElevation(Slider_GebirgeStauchen.Value);
            drawMap();
        }

        private void TextBox_GebirgeStauchen_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                if (IsNumberPositiv(TextBox_GebirgeStauchen.Text))
                {
                    int divisor;
                    if (Int32.TryParse(TextBox_GebirgeStauchen.Text, out divisor))
                    {
                        if(divisor >= Slider_GebirgeStauchen.Minimum && divisor <= Slider_GebirgeStauchen.Maximum)
                        {
                            map.compressingElevation(divisor);
                            Slider_GebirgeStauchen.Value = divisor;
                            drawMap();
                        }
                        else 
                        {
                            TextBox_GebirgeStauchen.Text = "1";
                            Slider_GebirgeStauchen.Value = 1;
                            MessageBox.Show("Die eingegebene Zahl muss im Intervall von " + Slider_GebirgeStauchen.Minimum + 
                                            " bis " + Slider_GebirgeStauchen.Maximum + " sein", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        TextBox_GebirgeStauchen.Text = "1";
                        MessageBox.Show("Die eingegebene Zahl ist zu groß", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    TextBox_GebirgeStauchen.Text = "1";
                    MessageBox.Show("Dieses Feld akzeptiert nur positive Zahlen", "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Slider_GebirgeStauchen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte wert = (byte)(e.NewValue);
            TextBox_GebirgeStauchen.Text = wert.ToString();
        }
    }
}
