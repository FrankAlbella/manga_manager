using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Doujin_Manager.Util
{
    class ImageCompressor
    {
        /// <summary>
        /// Compress a bitmap image and returns its file path.
        /// </summary>
        /// <param name="imagePath">Path to the image file.</param>
        /// <param name="quality">A value from 1-100 determining the quality. 100 = max quality; 1 = min quality.</param>
        /// <returns>The file path of the compressed image.</returns>
        public string CompressImage(string imagePath, byte quality)
        {
            string fileName = Path.GetFileName(imagePath);
            string lastFolderName = Path.GetFileName(Path.GetDirectoryName(imagePath));
            string fileCompressedPath = PathUtil.thumbnailDir + "\\" + lastFolderName + " - " + fileName;

            if (File.Exists(fileCompressedPath))
                return fileCompressedPath;

            try
            {
                Bitmap image = new Bitmap(imagePath);
                SaveCompressedImage(fileCompressedPath, image, quality);
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
        private void SaveCompressedImage(string path, Bitmap image, byte quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("Quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Image codec 
            ImageCodecInfo codec = GetEncoderInfo(GetMimeType(path));

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            image.Save(path, codec, encoderParams);
        }

        private string GetMimeType(string path)
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

        private ImageCodecInfo GetEncoderInfo(string mimeType)
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
