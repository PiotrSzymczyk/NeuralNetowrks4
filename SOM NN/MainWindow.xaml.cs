using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using Xceed.Wpf.Toolkit;
using Color = System.Windows.Media.Color;
using PixelFormat = System.Windows.Media.PixelFormat;
using Point = System.Drawing.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace SOM_NN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SOM net;
        private int netSize = 10;
        private Color[] trainingSet;
        private int numOfIterations = 100;

        public Color SelectedColor { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            net = new SOM(netSize);
            UpdateDisplay();
            ColorPicker.SelectedColor = Color.FromRgb(0, 0, 0);
        }

        private void UpdateDisplay(Point position = default(Point))
        {
            var tmp = ImageOps.ParseVectorToImage(net.Colors, netSize);
            if (position != default(Point))
            {
                for (int i = 0; i < netSize; i++)
                {
                    tmp.SetPixel(position.X, i, System.Drawing.Color.Black);
                    tmp.SetPixel(i, position.Y, System.Drawing.Color.Black);
                }
            }

            tmp.Save("result.bmp");
            imap.Source = ImageOps.ToBitmapImage(ImageOps.ResizeBitmap(tmp, (int) imap.Width, (int) imap.Height));
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> routedPropertyChangedEventArgs)
        {
            if (ColorPicker.SelectedColor != null) SelectedColor = ColorPicker.SelectedColor.Value;
        }

        public void Learn(object sender, RoutedEventArgs routedEventArgs)
        {
            LoadTrainingSet();
            net = new SOM(netSize);
            net.TimeConst = numOfIterations / net.MapRadius;
            for (int i = 0; i < numOfIterations; i++)
            {
                var trainingVector = trainingSet[RandomGenerator.Next(trainingSet.Length)];
                net.Learn(trainingVector, i);
                UpdateDisplay();
            }
        }

        public void Process(object sender, RoutedEventArgs routedEventArgs)
        {
            var position = net.FindBmu(ColorPicker.SelectedColor.Value);
            UpdateDisplay(position);
        }

        private void LoadTrainingSet()
        {
            var lines = File.ReadAllLines(tbPath.Text);
            trainingSet = new Color[lines.Length];
            int i = 0;
            foreach (var line in lines)
            {
                var rgb = line.Split(';').Select(val => byte.Parse(val)).ToArray();
                trainingSet[i++] = Color.FromRgb(rgb[0], rgb[1], rgb[2]);
            }
        }

        private void tbIterations_TextChanged(object sender, TextChangedEventArgs e)
        {
            numOfIterations = int.Parse(tbIterations.Text);
        }

        private void tbNetSize_TextChanged(object sender, TextChangedEventArgs e)
        {

            netSize = int.Parse(tbNetSize.Text);
        }
    }
}
