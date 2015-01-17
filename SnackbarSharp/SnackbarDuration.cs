namespace SnackbarSharp
{
    public class SnackbarDuration
    {
        public static SnackbarDuration Short = new SnackbarDuration(2000);
        public static SnackbarDuration Long = new SnackbarDuration(3500);
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
