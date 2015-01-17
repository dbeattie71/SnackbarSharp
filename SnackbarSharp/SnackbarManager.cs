using System;
using Android.App;

namespace SnackbarSharp
{
    public class SnackbarManager
    {
        private static Snackbar _currentSnackbar;

        public static void Show(Snackbar snackbar)
        {
            try
            {
                Show(snackbar, (Activity) snackbar.Context);
            }
            catch (Exception e)
            {
            }
        }

        public static void Show(Snackbar snackbar, Activity activity)
        {
            if (_currentSnackbar != null)
                _currentSnackbar.Dismiss();
            _currentSnackbar = snackbar;
            _currentSnackbar.Show(activity);
        }

        public static void Dismiss()
        {
            if (_currentSnackbar != null)
                _currentSnackbar.Dismiss();
        }
    }
}
