using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace com.dbeattie
{
    public class SnackbarLayout : LinearLayout
    {
        private int _maxHeight = int.MaxValue;
        private int _maxWidth = int.MaxValue;

        protected SnackbarLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public SnackbarLayout(Context context) : base(context)
        {
        }

        public SnackbarLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public SnackbarLayout(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public SnackbarLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var width = MeasureSpec.GetSize(widthMeasureSpec);
            if (_maxWidth < width)
            {
                var mode = MeasureSpec.GetMode(widthMeasureSpec);
                widthMeasureSpec = MeasureSpec.MakeMeasureSpec(_maxWidth, mode);
            }

            var height = MeasureSpec.GetSize(heightMeasureSpec);
            if (_maxHeight < height)
            {
                var mode = MeasureSpec.GetMode(heightMeasureSpec);
                heightMeasureSpec = MeasureSpec.MakeMeasureSpec(_maxHeight, mode);
            }

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        public void SetMaxWidth(int maxWidth)
        {
            _maxWidth = maxWidth;
            RequestLayout();
        }

        public void SetMaxHeight(int maxHeight)
        {
            _maxHeight = maxHeight;
            RequestLayout();
        }
    }
}
