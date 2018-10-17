using Doujin_Manager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace Doujin_Manager.Util
{
    class DoujinScrubber
    {
        private readonly CentralViewModel dataContext;
        private Cache cache;

        private static readonly List<string> extensions = new List<string> { ".jpg", ".jpeg", ".png" };

        public DoujinScrubber(CentralViewModel dataContext)
        {
            this.dataContext = dataContext;
        }

        public void PopulateDoujins()
        {
            if (!Directory.Exists(Properties.Settings.Default.DoujinDirectory))
                return;

            List<string> potentialDoujinDirectories = GetPotentialDoujinDirectories();

            if (potentialDoujinDirectories == null)
                return;

            cache = new Cache();

            if (cache.CachedDoujins != null)
            {
                List<string> doujinsFoundInCache = GetDoujinsFoundInCache(potentialDoujinDirectories);

                potentialDoujinDirectories = potentialDoujinDirectories.Except(doujinsFoundInCache).ToList<string>();

                Invoke_AddCachedDoujinsToViewModel(doujinsFoundInCache);
            }   

            TagScrubber tagScrubber = new TagScrubber();

            // Search for the first image in the directory 
            // and set it as cover image + add it to the panel
            foreach (string directory in potentialDoujinDirectories)
            {
                string coverImagePath = GetDefaultCoverPath(directory);
                string dirName = Path.GetFileName(Path.GetDirectoryName(coverImagePath));

                coverImagePath = ImageCompressor.CompressImage(coverImagePath, 40);

                tagScrubber.GatherDoujinDetails(dirName, TagScrubber.SearchMode.Title);

                try
                {
                    Invoke_AddDoujinToViewModel(coverImagePath, dirName, directory, tagScrubber);
                }
                catch(NotSupportedException)
                {
                    Debug.WriteLine("Image corrupted or not supported: " + coverImagePath);
                }
                catch
                {
                    Debug.WriteLine("Failed to add doujin at: " + coverImagePath);
                }

                tagScrubber.Clear();

                // Add delay to prevent sending too many requests to nHentai
                // currently still results in a ban (403 Forbidden)
                System.Threading.Thread.Sleep(1000);
            }

            Invoke_SaveToCache(dataContext.DoujinsViewModel.Doujins.ToList<Doujin>());
        }

        public static string GetDefaultCoverPath(string doujinDirectory)
        {
            if (Directory.Exists(doujinDirectory))
                return Directory.GetFiles(doujinDirectory).FirstOrDefault(s => extensions.Contains(Path.GetExtension(s.ToLower())));

            return null;
        }

        private List<string> GetDoujinsFoundInCache(List<string> doujinDirectories)
        {
            var doujinsFoundInCache = from first in doujinDirectories
                                      join second in cache.CachedDoujins
                                      on first equals second.Directory
                                      select first;

            return doujinsFoundInCache.ToList<string>();
        }

        private void Invoke_SaveToCache(List<Doujin> doujins)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => cache.Save(doujins)));
        }

        private void Invoke_AddDoujinToViewModel(Doujin doujin)
        {
            Invoke_AddDoujinToViewModel(doujin.CoverImage.UriSource.ToString(), doujin.Title, doujin.Author, doujin.Directory, doujin.Parodies, doujin.Characters, doujin.Tags, doujin.ID);
        }

        private void Invoke_AddDoujinToViewModel(string coverImagePath, string title, string directory, TagScrubber tagScrubber)
        {
            Invoke_AddDoujinToViewModel(coverImagePath, title, tagScrubber.Author, directory, tagScrubber.Parodies, tagScrubber.Characters, tagScrubber.Tags, tagScrubber.ID);
        }

        private void Invoke_AddDoujinToViewModel(string coverImagePath, string title, string author, string directory, string parodies, string characters, string tags, string id)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Doujin doujin = new Doujin
                {
                    Title = title,
                    Author = author,
                    Directory = directory,
                    Parodies = parodies,
                    Characters = characters,
                    Tags = tags,
                    ID = id
                };

                doujin.CreateAndSetCoverImage(coverImagePath);

                dataContext.DoujinsViewModel.Doujins.Add(doujin);

            }));
        }

        private void Invoke_AddCachedDoujinsToViewModel(List<string> doujinsFoundInCache)
        {
            var cachedDoujinsQuery =   from first in doujinsFoundInCache
                                        join second in cache.CachedDoujins
                                        on first equals second.Directory
                                        select second;

            List<Doujin> cachedDoujins = cachedDoujinsQuery.ToList<Doujin>();

            foreach (Doujin doujin in cachedDoujins)
            {
                Invoke_AddDoujinToViewModel(doujin);
            }
        }

        private List<string> GetPotentialDoujinDirectories()
        {
            string[] allSubDirectories;


            try
            {
                allSubDirectories = Directory.GetDirectories(Properties.Settings.Default.DoujinDirectory, "*", SearchOption.AllDirectories);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message,
                    "Unauthorized Access Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }

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

            return potentialDoujinDirectories;
        }
    }
}
