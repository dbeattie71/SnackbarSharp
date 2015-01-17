using System;
using Android.Views;

namespace SnackbarSharp
{
    public interface IDismissCallbacks
    {
        bool CanDismiss(Object token);

        void OnDismiss(View view, Object token);

        void PauseTimer(bool shouldPause);
    }
}