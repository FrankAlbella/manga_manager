using Doujin_Manager.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Doujin_Manager.Util
{
    class DoujinScrubber
    {
        public void PopulateDoujins(CentralViewModel dataContext)
        {
            if (!Directory.Exists(Properties.Settings.Default.DoujinDirectory))
                return;

            string[] allSubDirectories;
            

            try
            {
                allSubDirectories = Directory.GetDirectories(Properties.Settings.Default.DoujinDirectory, "*", SearchOption.AllDirectories);
            }
            catch(UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message,
                    "Unauthorized Access Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            List<string> potentialDoujinDirectories = new List<string>();

            TagScrubber tagScrubber = new TagScrubber();

            ImageCompressor imageCompressor = new ImageCompressor();

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
                string dirName = "";

                foreach (string file in files)
                {
                    if (file.ToLower().EndsWith(".jpg") ||
                        file.ToLower().EndsWith(".png") ||
                        file.ToLower().EndsWith(".jpeg"))
                    {
                        coverImagePath = imageCompressor.CompressImage(file, 40);
                        dirName = Path.GetFileName(Path.GetDirectoryName(file));
                        break;
                    }
                }

                tagScrubber.GatherDoujinDetails(dirName, TagScrubber.SearchMode.Title);

                try
                {   // evokes the main (UI) thread to add the Doujin
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        BitmapImage coverImage = new BitmapImage();

                        coverImage.BeginInit();
                            coverImage.UriSource = new Uri(coverImagePath, UriKind.Absolute);
                            coverImage.CacheOption = BitmapCacheOption.None;
                            coverImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            coverImage.DecodePixelWidth = 140;
                        coverImage.EndInit();

                        coverImage.Freeze();

                        Doujin doujin = new Doujin
                        {
                            CoverImage = coverImage,
                            Title = dirName,
                            Author = tagScrubber.Author,
                            Directory = directory,
                            Tags = tagScrubber.Tags,
                            ID = tagScrubber.ID
                        };

                        dataContext.DoujinsViewModel.Doujins.Add(doujin);
                        dataContext.DoujinInfoViewModel.Count = dataContext.DoujinsViewModel.Doujins.Count.ToString();

                    }));
                }
                catch
                {
                    Debug.WriteLine("Failed to add doujin at: " + coverImagePath);
                }

                tagScrubber.Clear();

                // Add delay to prevent sending too many requests to nHentai
                // currently still results in a ban (403 Forbidden)
                System.Threading.Thread.Sleep(500);
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                CacheToJson(dataContext);
            }));
        }

        private void CacheToJson(CentralViewModel dataContext)
        {
            List<Doujin> doujins = dataContext.DoujinsViewModel.Doujins.ToList<Doujin>();
            string[] aDoujins = new string[doujins.Count];

            for (int i = 0; i < doujins.Count; i++)
            {
                aDoujins[i] = JsonConvert.SerializeObject(doujins[i], Formatting.Indented);
            }

            File.WriteAllLines(PathUtil.cacheFilePath, aDoujins);
        }
    }
}
