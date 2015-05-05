using System;
using Android.Views;

namespace com.dbeattie
{
    public interface IDismissCallbacks
    {
        bool CanDismiss(Object token);

        void OnDismiss(View view, Object token);

        void PauseTimer(bool shouldPause);
    }
}