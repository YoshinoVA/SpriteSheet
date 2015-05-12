using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
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
            //string[] fName;

            public MainWindow()
            {
                InitializeComponent();
            }

            public string[] openFile()
            {
                string[] fileName = new string[] { };

                Microsoft.Win32.OpenFileDialog openFile2 = new Microsoft.Win32.OpenFileDialog();

                openFile2.Filter = "All files (*.*)|*.*";
                openFile2.FilterIndex = 2;
                openFile2.RestoreDirectory = true;

                return fileName;
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
                Microsoft.Win32.SaveFileDialog saveThing = new Microsoft.Win32.SaveFileDialog();

                if (saveThing.ShowDialog() == true)
                {
                    int total2 = 0;

                    var spriteSheetImage = BitmapFactory.New(1024, 1024);

                    for (int i = 0; i < images.Count; i++)
                    {
                        spriteSheetImage.Blit(new Rect(total2, 0, images[i].Width, images[i].Height),
                                          new WriteableBitmap(images[i]),
                                          new Rect(0, 0, images[i].Width, images[i].Height));

                        total2 += (int)images[i].Width;
                    }

                    var frame = BitmapFrame.Create(spriteSheetImage);

                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(frame);

                    try
                    {
                        System.IO.FileStream SaveFile = new System.IO.FileStream(saveThing.FileName, System.IO.FileMode.Create);
                        encoder.Save(SaveFile);
                        SaveFile.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            public void SaveXML(string fName)
                {
                int lastIndex = fName.LastIndexOf('\\');
                string file = fName.Substring(lastIndex + 1);
                lastIndex = file.LastIndexOf('.');
                string xml = "";
                if(file != "")
                {
                    xml = file.Substring(0, lastIndex) + ".xml";
                }
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", null, null);
                doc.AppendChild(xmlDec);
                XmlElement Atlas = doc.CreateElement("TextureAtlas");
                Atlas.SetAttribute("ImagePath", file);
                for (int i = 0; i < images.Count; i++)
                {
                    XmlElement SpriteChild = doc.CreateElement("sprite");
                    string x = images[i].DpiX.ToString();
                    SpriteChild.SetAttribute("X", x);

                    string y = images[i].DpiY.ToString();
                    SpriteChild.SetAttribute("Y", y);

                    string width = images[i].Width.ToString();
                    SpriteChild.SetAttribute("Width", width);

                    string height = images[i].Height.ToString();
                    SpriteChild.SetAttribute("Height", height);

                    Atlas.AppendChild(SpriteChild);
                }
                doc.AppendChild(Atlas);
                if (xml != "")
                {
                    doc.Save(fName);
                }
                else
                {
                    return;
                }

                }

            private void XMLexport_Click(object sender, RoutedEventArgs e)
            {
                Microsoft.Win32.SaveFileDialog saveThing = new Microsoft.Win32.SaveFileDialog();
                if (saveThing.ShowDialog() == true)
                {
                    SaveXML(saveThing.FileName);
                }
            }
        }
}
