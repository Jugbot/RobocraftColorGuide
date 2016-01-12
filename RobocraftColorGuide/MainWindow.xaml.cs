using Microsoft.Win32;
using System;
using Bitmap = System.Drawing.Bitmap;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RobocraftColorGuide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Color WHITE = Color.FromRgb(255, 255, 255);
        private static Color GREY = Color.FromRgb(111, 111, 111);
        private static Color ORANGE = Color.FromRgb(255, 129, 32);
        private static Color LIGHTBLUE = Color.FromRgb(17, 167, 253);
        private static Color BLACK = Color.FromRgb(0, 0, 0);
        private static Color RED = Color.FromRgb(223, 28, 2);
        private static Color YELLOW = Color.FromRgb(254, 222, 25);
        private static Color GREEN = Color.FromRgb(53, 188, 28);
        private static Color HOTPINK = Color.FromRgb(236, 2, 194);
        private static Color BLUE = Color.FromRgb(12, 72, 221);
        private static Color PURPLE = Color.FromRgb(151, 40, 216);
        private static Color BROWN = Color.FromRgb(161, 80, 26);
        private static Color LIME = Color.FromRgb(170, 183, 54);
        private static Color DULLGREEN = Color.FromRgb(4, 137, 64);
        private static Color BEIGE = Color.FromRgb(237, 210, 161);
        private static Color PINK = Color.FromRgb(253, 163, 193);
        private static Color[] colors = new Color[] {
            WHITE,
            GREY,
            ORANGE,
            LIGHTBLUE,
            BLACK,
            RED,
            YELLOW,
            GREEN,
            HOTPINK,
            BLUE,
            PURPLE,
            BROWN,
            LIME,
            DULLGREEN,
            BEIGE,
            PINK,
        };

        public MainWindow()
        {
            InitializeComponent();
            White.Background = new SolidColorBrush(WHITE);
            Grey.Background = new SolidColorBrush(GREY);
            Orange.Background = new SolidColorBrush(ORANGE);
            LightBlue.Background = new SolidColorBrush(LIGHTBLUE);
            Black.Background = new SolidColorBrush(BLACK);
            Red.Background = new SolidColorBrush(RED);
            Yellow.Background = new SolidColorBrush(YELLOW);
            Green.Background = new SolidColorBrush(GREEN);
            Hotpink.Background = new SolidColorBrush(HOTPINK);
            Blue.Background = new SolidColorBrush(BLUE);
            Purple.Background = new SolidColorBrush(PURPLE);
            Brown.Background = new SolidColorBrush(BROWN);
            Lime.Background = new SolidColorBrush(LIME);
            DullGreen.Background = new SolidColorBrush(DULLGREEN);
            Beige.Background = new SolidColorBrush(BEIGE);
            Pink.Background = new SolidColorBrush(PINK);

        }

        private int X = 16, Y = 16;

        private void createGrid()
        {
            colorGrid.Children.Clear();
            colorGrid.RowDefinitions.Clear();
            colorGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < X; i++)
                colorGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < Y; i++)
                colorGrid.RowDefinitions.Add(new RowDefinition());

        }

        private void loadImage(string path)
        {
            Bitmap b = new Bitmap(new Bitmap(path), X, Y);
            createGrid();
            for (int i = 0; i < X; i++)
                for (int j = 0; j < Y; j++)
                {
                    Rectangle temp = new Rectangle();
                    System.Drawing.Color c = b.GetPixel(i, j);
                    temp.Fill = new SolidColorBrush(closestMatch(Color.FromArgb(c.A, c.R, c.G, c.B)));
                    temp.ToolTip = "X" + (i + 1) + " Y" + (49-j);
                    Grid.SetColumn(temp, i);
                    Grid.SetRow(temp, j);
                    colorGrid.Children.Add(temp);
                }

        }

        private void update()
        {
            if (File.Exists(textBoxOpenFile.Text)) //&& System.IO.Path.GetExtension(textBoxOpenFile.Text).Equals(".png"))
                loadImage(textBoxOpenFile.Text);
        }

        private Color closestMatch(Color c)
        {
            Color final = Color.FromRgb(255, 204, 203);
            double min = 1000000.0;
            foreach (Color color in colors)
            {
                if (!color.Equals(new Color()))// && c.A > 10)
                {

                    double diff = YUVDifference(RGBtoYUV(color), RGBtoYUV(c));// LABDifference(RGBtoLAB(color), RGBtoLAB(c));
                    if (diff < min)
                    {
                        min = diff;
                        final = color;
                    }
                }
            }


            return final;
        }
        private int[] RGBtoYUV(Color c)
        {
            int Y = (int)((((66 * c.R + 129 * c.G + 25 * c.B + 128) >> 8) + 16) * slider1.Value);
            int U = (int)((((-38 * c.R - 74 * c.G + 112 * c.B + 128) >> 8) + 128) * slider2.Value);
            int V = (int)((((112 * c.R - 94 * c.G - 18 * c.B + 128) >> 8) + 128) * slider3.Value);
            return new int[] { Y, U, V };
        }

        private int YUVDifference(int[] c1, int[] c2)
        {
            return (c1[0] - c2[0]) * (c1[0] - c2[0]) + (c1[1] - c2[1]) * (c1[1] - c2[1]) + (c1[2] - c2[2]) * (c1[2] - c2[2]);
        }

        private double LABDifference(double[] lab1, double[] lab2)
        {
            double xC1 = Math.Sqrt((lab1[1] * lab1[1]) + (lab1[2] * lab1[2]));
            double xC2 = Math.Sqrt((lab2[1] * lab2[1]) + (lab2[2] * lab2[2]));
            double xDL = lab2[0] - lab1[0];
            double xDC = xC2 - xC1;
            double xDE = Math.Sqrt(((lab1[0] - lab2[0]) * (lab1[0] - lab2[0]))
                      + ((lab1[1] - lab2[1]) * (lab1[1] - lab2[1]))
                      + ((lab1[2] - lab2[2]) * (lab1[2] - lab2[2])));
            double xDH;
            if (Math.Sqrt(xDE) > (Math.Sqrt(Math.Abs(xDL)) + Math.Sqrt(Math.Abs(xDC))))
            {
                xDH = Math.Sqrt((xDE * xDE) - (xDL * xDL) - (xDC * xDC));
            }
            else
            {
                xDH = 0;
            }
            double xSC = 1 + (0.045 * xC1);
            double xSH = 1 + (0.015 * xC1);
            xDL /= 1; //WEIGHTS
            xDC /= 1 * xSC;
            xDH /= 1 * xSH;
            double delta = Math.Sqrt(xDL * xDL + xDC * xDC + xDH * xDH);
            Console.WriteLine(delta);
            return delta;
        }

        private double[] RGBtoLAB(Color c)
        {
            double R = (c.R / 255.0);
            double G = (c.G / 255.0);
            double B = (c.B / 255.0);
            //Console.WriteLine(R+ ", " + G+ ", " + B);
            if (R > 0.04045)
                R = Math.Pow(((R + 0.055) / 1.055), 2.4);
            else
                R = R / 12.92;
            if (G > 0.04045)
                G = Math.Pow(((G + 0.055) / 1.055), 2.4);
            else
                G = G / 12.92;
            if (B > 0.04045)
                B = Math.Pow(((B + 0.055) / 1.055), 2.4);
            else
                B = B / 12.92;

            R *= 100.0;
            G *= 100.0;
            B *= 100.0;

            double X = R * 0.4124 + G * 0.3576 + B * 0.1805;
            double Y = R * 0.2126 + G * 0.7152 + B * 0.0722;
            double Z = R * 0.0193 + G * 0.1192 + B * 0.9505;

            //Console.WriteLine(X + ", " + Y + ", " + Z);

            double var_X = X / 95.047;
            double var_Y = Y / 100.0;
            double var_Z = Z / 108.883;

            if (var_X > 0.008856)
                var_X = Math.Pow(var_X, (1.0 / 3.0));
            else
                var_X = (7.787 * var_X) + (16.0 / 116);
            if (var_Y > 0.008856)
                var_Y = Math.Pow(var_Y, (1.0 / 3.0));
            else
                var_Y = (7.787 * var_Y) + (16.0 / 116.0);
            if (var_Z > 0.008856)
                var_Z = Math.Pow(var_Z, (1.0 / 3.0));
            else
                var_Z = (7.787 * var_Z) + (16.0 / 116.0);

            double CIEL = (116.0 * var_Y) - 16.0;
            double CIEA = 500.0 * (var_X - var_Y);
            double CIEB = 200.0 * (var_Y - var_Z);
            //Console.WriteLine(CIEL + ", " + CIEA + ", " + CIEB);
            return new double[] { CIEL, CIEA, CIEB };

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                textBoxOpenFile.Text = openFileDialog.FileName;

            if (File.Exists(textBoxOpenFile.Text))// && System.IO.Path.GetExtension(textBoxOpenFile.Text).Equals(".png"))
                loadImage(textBoxOpenFile.Text);
            else MessageBox.Show("Invalid image file!", "Error");
        }

        private void textBoxOpenFile_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            if (File.Exists(textBoxOpenFile.Text))// && System.IO.Path.GetExtension(textBoxOpenFile.Text).Equals(".png"))
                loadImage(textBoxOpenFile.Text);
            else MessageBox.Show("Invalid image file!", "Error");
        }

        private void textBoxX_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i;
            if (Int32.TryParse(textBoxX.Text, out i) && i < 50 && i > 0)
            {
                X = i;
                update();
            }
            else if (textBoxX.Text != "")
                textBoxX.Text = "49";
        }

        private void textBoxY_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i;
            if (Int32.TryParse(textBoxY.Text, out i) && i < 50 && i > 0)
            {
                Y = i;
                update();
            }
            else if (textBoxX.Text != "")
                textBoxY.Text = "49";

        }

        private void White_Click(object sender, RoutedEventArgs e)
        {
            if (White.IsChecked == true)
                colors[0] = WHITE;
            else
                colors[0] = new Color();
            update();
        }

        private void Grey_Click(object sender, RoutedEventArgs e)
        {
            if (Grey.IsChecked == true)
                colors[1] = GREY;
            else
                colors[1] = new Color();
            update();
        }

        private void Orange_Click(object sender, RoutedEventArgs e)
        {
            if (Orange.IsChecked == true)
                colors[2] = ORANGE;
            else
                colors[2] = new Color();
            update();
        }

        private void LightBlue_Click(object sender, RoutedEventArgs e)
        {
            if (LightBlue.IsChecked == true)
                colors[3] = LIGHTBLUE;
            else
                colors[3] = new Color();
            update();
        }

        private void Black_Click(object sender, RoutedEventArgs e)
        {
            if (Black.IsChecked == true)
                colors[4] = BLACK;
            else
                colors[4] = new Color();
            update();
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            if (Red.IsChecked == true)
                colors[5] = RED;
            else
                colors[5] = new Color();
            update();
        }

        private void Yellow_Click(object sender, RoutedEventArgs e)
        {
            if (Yellow.IsChecked == true)
                colors[6] = YELLOW;
            else
                colors[6] = new Color();
            update();
        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            if (Green.IsChecked == true)
                colors[7] = GREEN;
            else
                colors[7] = new Color();
            update();
        }

        private void Hotpink_Click(object sender, RoutedEventArgs e)
        {
            if (Hotpink.IsChecked == true)
                colors[8] = HOTPINK;
            else
                colors[8] = new Color();
            update();
        }

        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            if (Blue.IsChecked == true)
                colors[9] = BLUE;
            else
                colors[9] = new Color();
            update();
        }

        private void Purple_Click(object sender, RoutedEventArgs e)
        {
            if (Purple.IsChecked == true)
                colors[10] = PURPLE;
            else
                colors[10] = new Color();
            update();
        }

        private void Brown_Click(object sender, RoutedEventArgs e)
        {
            if (Brown.IsChecked == true)
                colors[11] = BROWN;
            else
                colors[11] = new Color();
            update();
        }

        private void Lime_Click(object sender, RoutedEventArgs e)
        {
            if (Lime.IsChecked == true)
                colors[12] = LIME;
            else
                colors[12] = new Color();
            update();
        }

        private void DullGreen_Click(object sender, RoutedEventArgs e)
        {
            if (DullGreen.IsChecked == true)
                colors[13] = DULLGREEN;
            else
                colors[13] = new Color();
            update();
        }

        private void Beige_Click(object sender, RoutedEventArgs e)
        {
            if (Beige.IsChecked == true)
                colors[14] = BEIGE;
            else
                colors[14] = new Color();
            update();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            update();
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            update();
        }

        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            update();
        }

        private void Pink_Click(object sender, RoutedEventArgs e)
        {
            if (Pink.IsChecked == true)
                colors[15] = PINK;
            else
                colors[15] = new Color();
            update();
        }
    }
}
