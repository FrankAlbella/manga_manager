using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Manager.Util
{
    static class SettingsUtil
    {
        public static string DoujinDirectory
        {
            get { return Properties.Settings.Default.DoujinDirectory; }
            set { Properties.Settings.Default.DoujinDirectory = value; Properties.Settings.Default.Save(); }
        }

        public struct Theme
        {
            public int titleBar;
        }

    }
}
