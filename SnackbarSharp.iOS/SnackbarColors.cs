using System;
using UIKit;

namespace SnackbarSharp.iOS
{
    public class SnackbarColors
    {
        private static UIColor MakeUIColor(uint code)
        {
            return new UIColor((code >> 16) / 255.0f, ((code & 0xff00) >> 8) / 255.0f, (code & 0xff) / 255.0f, 1.0f);
        }

        public static readonly UIColor Background = MakeUIColor(0x323232);

        public static readonly UIColor TextColor = MakeUIColor(0xFFFFFFFF);

        public static readonly UIColor ActionTextColor = MakeUIColor(0xFFFFFFFF);

        public static readonly UIColor ActionBgColor = MakeUIColor(0x1AFFFFFF);
    }
}

