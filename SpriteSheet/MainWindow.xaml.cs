using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpriteSheet
{
    // <summary>
    // Interaction logic for MainWindow.xaml
    // </summary>
        public partial class MainWindow : Window
        {
            double total = 0.0;
            List<BitmapImage> images = new List<BitmapImage>();

            public MainWindow()
            {
                InitializeComponent();
            }

            void button_Click(object sender, RoutedEventArgs e)
            {

                Microsoft.Win32.OpenFileDialog userPrompt = new Microsoft.Win32.OpenFileDialog();
                var imageFile = userPrompt.ShowDialog();

                if (imageFile == true)
                {
                    ListBox1.Items.Add(userPrompt.FileName);

                    Image image = new Image();
                    
                    BitmapImage bi = new BitmapImage(new Uri(userPrompt.FileName));
                    double picWidth = bi.PixelWidth;

                    Canvas.SetLeft(image, total);
                    Canvas.SetTop(image, (double) 0.0);

                    total += (double)picWidth;

                    image.Source = new BitmapImage(new Uri(userPrompt.FileName));
                    picBox1.Children.Add(image);
                    images.Add(bi);
                }
            }

            private void exButton_Click(object sender, RoutedEventArgs e)
            {
                var spriteSheetImage = BitmapFactory.New(1024, 1024);

                for (int i = 0; i < images.Count; i++)
                {
                    spriteSheetImage.Blit(new Rect(0, 0, images[i].Width, images[i].Height),
                                      new WriteableBitmap(images[i]),
                                      new Rect(0, 0, images[i].Width, images[i].Height));
                }

                var frame = BitmapFrame.Create(spriteSheetImage);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(frame);

                try
                {
                    System.IO.FileStream SaveFile = new System.IO.FileStream("name.png", System.IO.FileMode.Create);
                    encoder.Save(SaveFile);
                    SaveFile.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
}
