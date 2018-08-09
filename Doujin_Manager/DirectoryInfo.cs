using System;

namespace Doujin_Manager
{
    public static class DirectoryInfo
    {
        public static string appdataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\doujin_manager";
        public static string thumbnailDir = appdataDir + @"\thumbnails";
        public static string saveFilePath = appdataDir + @"\settings.xml";
        public static string rootDoujinDirectory = Properties.Settings.Default.DoujinDirectory;
    }
}
