using Doujin_Manager.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Doujin_Manager
{
    class DoujinScrubber
    {
        readonly string doujinRootDirectory = @"G:\Users\Monster PC\Desktop\hentai\Doujins";

        public void SearchAll(WrapPanel panel)
        {
            if (!Directory.Exists(doujinRootDirectory))
            {
                MessageBoxResult result = MessageBox.Show("Why did you lie to me you fucking retard",
                                          "Directory Not Found");
                return;
            }
            string[] allSubDirectories = Directory.GetDirectories(doujinRootDirectory);

            List<string> potentialDoujinDirectories = new List<string>();

            // Search all subdirectories which contain an image
            foreach (string directory in allSubDirectories)
            {
                string[] files = Directory.GetFiles(directory);

                if(files.Any(s => s.EndsWith(".jpg")) ||
                    files.Any(s => s.EndsWith(".png")) ||
                    files.Any(s => s.EndsWith(".jpeg")))
                {
                    potentialDoujinDirectories.Add(directory);
                }
            }

            // Search for the first image in the directory 
            // and set it as cover image + add it to the panel

            foreach (string directory in potentialDoujinDirectories)
            {
                string[] files = Directory.GetFiles(directory);
                string coverImagePath = "";

                foreach(string file in files)
                {
                    if(file.EndsWith(".jpg") ||
                        file.EndsWith(".png") ||
                        file.EndsWith(".jpeg"))
                    {
                        coverImagePath = file;
                        break;
                    }
                }

                Doujin doujin = new Doujin();
                doujin.CoverImage = new BitmapImage(new Uri(coverImagePath));
                doujin.Title = Path.GetFileName(Path.GetDirectoryName(coverImagePath));
                doujin.Author = "UNKOWN";

                panel.Children.Add(new DoujinControl(doujin));
            }
        }
    }
}
