using Doujin_Manager.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Doujin_Manager
{
    class DoujinScrubber
    {
        public void SearchAll(DoujinViewModel dataContext)
        {
            if (DirectoryInfo.rootDoujinDirectory == string.Empty)
                return;

            string[] allSubDirectories = Directory.GetDirectories(DirectoryInfo.rootDoujinDirectory, "*", SearchOption.AllDirectories);

            List<string> potentialDoujinDirectories = new List<string>();

            // Search all subdirectories which contain an image
            foreach (string directory in allSubDirectories)
            {
                string[] files = Directory.GetFiles(directory);

                if (files.Any(s => s.ToLower().EndsWith(".jpg")) ||
                    files.Any(s => s.ToLower().EndsWith(".png")) ||
                    files.Any(s => s.ToLower().EndsWith(".jpeg")))
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

                foreach (string file in files)
                {
                    if (file.ToLower().EndsWith(".jpg") ||
                        file.ToLower().EndsWith(".png") ||
                        file.ToLower().EndsWith(".jpeg"))
                    {
                        coverImagePath = file;
                        break;
                    }
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    BitmapImage coverImage = new BitmapImage(new Uri(coverImagePath));
                    coverImage.CacheOption = BitmapCacheOption.None;
                    coverImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    Doujin doujin = new Doujin
                    {
                        CoverImage = coverImage,
                        Title = Path.GetFileName(Path.GetDirectoryName(coverImagePath)),
                        Author = "UNKOWN",
                        Directory = directory
                    };

                    dataContext.DoujinsViewModel.Doujins.Add(new DoujinControl(doujin));

                }));
                

            }
            dataContext.DoujinInfoViewModel.Count = "Count: " + dataContext.DoujinsViewModel.Doujins.Count;
        }
    }
}
