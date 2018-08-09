using Doujin_Manager.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
                string dirName = "";

                foreach (string file in files)
                {
                    if (file.ToLower().EndsWith(".jpg") ||
                        file.ToLower().EndsWith(".png") ||
                        file.ToLower().EndsWith(".jpeg"))
                    {
                        coverImagePath = CompressImage(file);
                        dirName = Path.GetFileName(Path.GetDirectoryName(file));
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
                        Title = dirName,
                        Author = "UNKOWN",
                        Directory = directory
                    };

                    dataContext.DoujinsViewModel.Doujins.Add(new DoujinControl(doujin));
                    dataContext.DoujinInfoViewModel.Count = "Count: " + dataContext.DoujinsViewModel.Doujins.Count;
                }));
                

            }
            
        }

        private string CompressImage(string imagePath)
        {
            string fileName = Path.GetFileName(imagePath);
            string lastFolderName = Path.GetFileName(Path.GetDirectoryName(imagePath));
            string fileCompressedPath = DirectoryInfo.thumbnailDir + "\\" + lastFolderName + " - "+ fileName;

            if (File.Exists(fileCompressedPath))
                return fileCompressedPath;

            try
            {
                Bitmap image = new Bitmap(imagePath);
                SaveCompressedImage(fileCompressedPath, image, 40);
                Debug.WriteLine("Image compressed: " + fileCompressedPath);
            }
            catch
            {
                Debug.WriteLine("Image failed to be compressed: " + imagePath);
                return imagePath;
            }

            return fileCompressedPath;
        }

        // More quality = higher quality image
        private static void SaveCompressedImage(string path, Bitmap image, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);

            // image codec 
            ImageCodecInfo codec = GetEncoderInfo(GetMameType(path));

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            image.Save(path, codec, encoderParams);
        }

        private static string GetMameType(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return "";
            }
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
    }
}
