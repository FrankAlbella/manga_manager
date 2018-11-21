using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Doujin_Manager.Util
{
    static class SettingsUtil
    {
        public static string DoujinDirectory
        {
            get { return Properties.Settings.Default.DoujinDirectory; }
            set { Properties.Settings.Default.DoujinDirectory = value; Properties.Settings.Default.Save(); }
        }

        public static Theme WhiteTheme = new Theme("#d9d9d9", "#1f1f1f", "#0d0d0d", "#0d0d0d");

        public struct Theme
        {
            public readonly Brush fontBrush;
            public readonly Brush titleBarBrush;
            public readonly Brush doujinInfoBackgroundBrush;
            public readonly Brush doujinViewBackgroundBrush;

            public Theme(Brush font, Brush titleBar, Brush doujinInfoBackground, Brush doujinViewBackground)
            {
                this.fontBrush = font;
                this.titleBarBrush = titleBar;
                this.doujinInfoBackgroundBrush = doujinInfoBackground;
                this.doujinViewBackgroundBrush = doujinViewBackground;
            }

            public Theme(string font, string titleBar, string doujinInfoBackground, string doujinViewBackground)
            {
                BrushConverter bc = new BrushConverter();

                this.fontBrush = (Brush)bc.ConvertFromString(font);
                this.titleBarBrush = (Brush)bc.ConvertFromString(titleBar);
                this.doujinInfoBackgroundBrush = (Brush)bc.ConvertFromString(doujinInfoBackground);
                this.doujinViewBackgroundBrush = (Brush)bc.ConvertFromString(doujinViewBackground);
            }
        }

    }
}
