using Doujin_Manager.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Doujin_Manager
{
    class DoujinScrubber
    {
        readonly string doujinRootDirectory = @"F:\Stuff\More Stuff";

        public void SearchAll(WrapPanel panel, ref int count)
        {
            string[] allSubDirectories = Directory.GetDirectories(doujinRootDirectory, "*", SearchOption.AllDirectories);

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
            count = 0;
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

                try
                {
                    Doujin doujin = new Doujin
                    {
                        CoverImage = new BitmapImage(new Uri(coverImagePath)),
                        Title = Path.GetFileName(Path.GetDirectoryName(coverImagePath)),
                        Author = "UNKOWN"
                    };

                    count++;

                    panel.Children.Add(new DoujinControl(doujin));
                }
                catch
                {
                    Debug.WriteLine("Problem with: " + coverImagePath);
                }
            }
        }
    }
}
