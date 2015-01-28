using System;
using UIKit;

namespace SnackbarSharp.iOS
{
    public class SnackbarManager
    {
        private static Snackbar _currentSnackbar;

//        public static void Show(Snackbar snackbar)
//        {
//            try
//            {
//                Show(snackbar);
//            }
//            catch (Exception e)
//            {
//            }
//        }

        public static void Show(Snackbar snackbar, UIView view)
        {
            if (_currentSnackbar != null)
                _currentSnackbar.Dismiss();
            _currentSnackbar = snackbar;
            _currentSnackbar.Show(view);
        }

        public static void Dismiss()
        {
            if (_currentSnackbar != null)
                _currentSnackbar.Dismiss();
        }
    }
}

