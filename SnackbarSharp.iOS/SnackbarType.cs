using System;

namespace SnackbarSharp.iOS
{
    public class SnackbarType
    {
        public static SnackbarType SingleLine = new SnackbarType(48, 48, 1);
        public static SnackbarType MultiLine = new SnackbarType(48, 80, 2);
        private readonly int _maxHeight;
        private readonly int _maxLines;
        private readonly int _minHeight;

        public SnackbarType(int minHeight, int maxHeight, int maxLines)
        {
            _minHeight = minHeight;
            _maxHeight = maxHeight;
            _maxLines = maxLines;
        }

        public int GetMinHeight()
        {
            return _minHeight;
        }

        public int GetMaxHeight()
        {
            return _maxHeight;
        }

        public int GetMaxLines()
        {
            return _maxLines;
        }
    }
}

