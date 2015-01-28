using System;

namespace SnackbarSharp.iOS
{
    public class SnackbarDuration
    {
        public static SnackbarDuration Short = new SnackbarDuration(TimeSpan.FromMilliseconds(2000).Seconds);
        public static SnackbarDuration Long = new SnackbarDuration(TimeSpan.FromMilliseconds(3500).Seconds);
        public static SnackbarDuration Indefinite = new SnackbarDuration(-1);
        private readonly long _duration;

        public SnackbarDuration(long duration)
        {
            _duration = duration;
        }

        public long GetDuration()
        {
            return _duration;
        }
    }
}

