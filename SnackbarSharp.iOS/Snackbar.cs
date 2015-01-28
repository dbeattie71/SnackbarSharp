using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace SnackbarSharp.iOS
{
    public class Snackbar : SnackbarLayout
    {
        //        private IActionClickListener _actionClickListener;
        private UIColor _actionColor;
        private string _actionLabel;
        private bool _animated = true;
        private bool _canSwipeToDismiss = true;
        private UIColor _color = SnackbarColors.Background;
        private long _customDuration = -1;
        private Action _dismissRunnable;
        private SnackbarDuration _duration = SnackbarDuration.Long;
        private IEventListener _eventListener;
        private bool _isDismissing;
        private bool _isShowing;
        private int _offset;
        private bool _shouldDismissOnActionClicked = true;
        private long _snackbarFinish;
        private long _snackbarStart;
        private string _text;
        private UIColor _textColor;
        private NSTimer _timer = null;
        private long _timeRemaining = -1;
        private SnackbarType _type = SnackbarType.SingleLine;

        private UIView _rootView;

        //        protected Snackbar(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        //        {
        //            InternalInit();
        //        }
        //
        //        public Snackbar(Context context) : base(context)
        //        {
        //            InternalInit();
        //        }
        //
        //        public Snackbar(Context context, IAttributeSet attrs) : base(context, attrs)
        //        {
        //            InternalInit();
        //        }
        //
        //        public Snackbar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        //        {
        //            InternalInit();
        //        }
        //
        //        public Snackbar(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
        //            : base(context, attrs, defStyleAttr, defStyleRes)
        //        {
        //            InternalInit();
        //        }

        public Snackbar()
        {
            InternalInit();
        }

        private void InternalInit()
        {
            _dismissRunnable = Dismiss;
            _color = SnackbarColors.Background;//Resources.GetColor(Resource.Color.sb__background);
            _textColor = SnackbarColors.TextColor;//Resources.GetColor(Resource.Color.sb__text_color);
            _actionColor = SnackbarColors.ActionBgColor;//Resources.GetColor(Resource.Color.sb__action_bg_color);
        }

        //public static Snackbar With(Context context)
        public static Snackbar With()
        {
//            return new Snackbar(context);
            return new Snackbar();
        }

        public Snackbar Type(SnackbarType type)
        {
            _type = type;
            return this;
        }

        public Snackbar Text(string text)
        {
            _text = text;
            return this;
        }

        public Snackbar Color(UIColor color)
        {
            _color = color;
            return this;
        }

        public Snackbar TextColor(UIColor textColor)
        {
            _textColor = textColor;
            return this;
        }

        public Snackbar ActionLabel(string actionButtonLabel)
        {
            _actionLabel = actionButtonLabel;
            return this;
        }

        public Snackbar ActionColor(UIColor actionColor)
        {
            _actionColor = actionColor;
            return this;
        }

        public Snackbar DismissOnActionClicked(bool shouldDismiss)
        {
            _shouldDismissOnActionClicked = shouldDismiss;
            return this;
        }

        //        public Snackbar ActionListener(IActionClickListener listener)
        //        {
        //            _actionClickListener = listener;
        //            return this;
        //        }

        //        public Snackbar EventListener(IEventListener listener)
        //        {
        //            _eventListener = listener;
        //            return this;
        //        }

        public Snackbar Animation(bool withAnimation)
        {
            _animated = withAnimation;
            return this;
        }

        public Snackbar SwipeToDismiss(bool canSwipeToDismiss)
        {
            _canSwipeToDismiss = canSwipeToDismiss;
            return this;
        }

        public Snackbar Duration(SnackbarDuration duration)
        {
            _duration = duration;
            return this;
        }

        public Snackbar Duration(long duration)
        {
            _customDuration = duration > 0 ? duration : _customDuration;
            return this;
        }

        //        public Snackbar AttachToAbsListView(AbsListView absListView)
        //        {
        //            absListView.ScrollStateChanged += (sender, args) => { Dismiss(); };
        //            return this;
        //        }
        //
        //        public Snackbar AttachToRecyclerView(RecyclerView recyclerView)
        //        {
        //            recyclerView.SetOnScrollListener(new RecyclerViewOnScrollListener(this));
        //            return this;
        //        }

        //        <dimen name="sb__bg_corner_radius">2dp</dimen>
        //            <dimen name="sb__min_width">288dp</dimen>
        //            <dimen name="sb__max_width">568dp</dimen>
        //            <dimen name="sb__offset">24dp</dimen>

        //private FrameLayout.LayoutParams Init(Context parent)
        private void Init()
        {
            //var layout = (SnackbarLayout) LayoutInflater.From(parent).Inflate(Resource.Layout.sb__template, this, true);
            //            var res = Resources;
            //            _offset = 0;
            //            var scale = res.DisplayMetrics.Density;
            //
            //            FrameLayout.LayoutParams layoutParams = null;

            UIView layout = null;
            var bounds = UIScreen.MainScreen.Bounds;

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            { 

                Frame = new CGRect(0, bounds.Height, bounds.Width, _type.GetMinHeight());
                BackgroundColor = _color;
                Layer.CornerRadius = Dimensions.CornerRadius;
                Layer.MasksToBounds = true;

                //layout = new UIView(new CGRect(
                //layout.SetMinimumHeight(DpToPx(_type.GetMinHeight(), scale));
                //                layout.SetMaxHeight(DpToPx(_type.GetMinHeight(), scale));
                //                layout.SetBackgroundColor(_color);
                //                layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                //                    ViewGroup.LayoutParams.WrapContent);

            }
            else
            {
                // Tablet/desktop
                //                _type = SnackbarType.SingleLine; // Force single-line
                //                layout.SetMinimumWidth(res.GetDimensionPixelSize(Resource.Dimension.sb__min_width));
                //                layout.SetMaxWidth(res.GetDimensionPixelSize(Resource.Dimension.sb__max_width));
                //                layout.SetBackgroundResource(Resource.Drawable.sb__bg);
                //                var bg = (GradientDrawable) layout.Background;
                //                bg.SetColor(_color);
                //                layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                //                    DpToPx(_type.GetMaxHeight(), scale))
                //                    {
                //                        LeftMargin = _offset,
                //                        BottomMargin = _offset
                //                    };
            }

//            layoutParams.Gravity = GravityFlags.Bottom;
//            var snackbarText = (TextView) layout.FindViewById(Resource.Id.sb__text);
//            snackbarText.Text = _text;
//            snackbarText.SetTextColor(_textColor);
//            snackbarText.SetMaxLines(_type.GetMaxLines());

            var width = bounds.Width - Dimensions.Offset * 2; //TODO: temp until action button
            var label = new UILabel(new CGRect(24, 12, width, 20));
            label.Text = _text;
            label.TextColor = _textColor;
            label.Font = UIFont.SystemFontOfSize(11);

            AddSubview(label);
//
//            var snackbarAction = (TextView) layout.FindViewById(Resource.Id.sb__action);
//            if (!TextUtils.IsEmpty(_actionLabel))
//            {
//                RequestLayout();
//                snackbarAction.Text = _actionLabel;
//                snackbarAction.SetTextColor(_actionColor);
//
//                snackbarAction.Click += (sender, args) =>
//                    {
//                        if (_actionClickListener != null)
//                            _actionClickListener.OnActionClicked(this);
//                        if (_shouldDismissOnActionClicked)
//                            Dismiss();
//                    };
//                snackbarAction.SetMaxLines(_type.GetMaxLines());
//            }
//            else
//                snackbarAction.Visibility = ViewStates.Gone;
//
//            Clickable = true;
//
//            Func<object, bool> canDismiss = token => true;
//
//            Action<View, object> onDismiss = (view, token) =>
//                {
//                    if (view != null)
//                        Dismiss(false);
//                };
//
//            Action<bool> pauseTimer = shouldPause =>
//                {
//                    if (IsIndefiniteDuration())
//                    {
//                        return;
//                    }
//                    if (shouldPause)
//                    {
//                        RemoveCallbacks(_dismissRunnable);
//
//                        _snackbarFinish = DateTime.Now.Millisecond;
//                    }
//                    else
//                    {
//                        _timeRemaining -= (_snackbarFinish -
//                            _snackbarStart);
//                        StartTimer(_timeRemaining);
//                    }
//                };
//
//            var swipeDismissTouchListener = new SwipeDismissTouchListener(this, null, canDismiss, onDismiss, pauseTimer);
//            if (_canSwipeToDismiss && res.GetBoolean(Resource.Boolean.sb__is_swipeable))
//                SetOnTouchListener(swipeDismissTouchListener);
//
//            return layoutParams;
        }

        //public void Show(Activity targetActivity)
        public void Show(UIView view)
        {
            //var layoutParams = Init(targetActivity);
            //var layoutParams = Init();
            Init();

            //TODO: instead of passing view could get window's first subview
            //var root = (ViewGroup) targetActivity.FindViewById(Android.Resource.Id.Content);
            var root = view;

            //root.RemoveView(this);
            RemoveFromSuperview();

            //if (IsNavigationBarHidden(root))
            //{
            //    var resources = Resources;
            //    var resourceId = resources.GetIdentifier("navigation_bar_height", "dimen", "android");
            //    if (resourceId > 0)
            //        layoutParams.BottomMargin = resources.GetDimensionPixelSize(resourceId);
            //}

            //            root.AddView(this, layoutParams);
            root.AddSubview(this);

            //            BringToFront();

            //            if (Build.VERSION.SdkInt < BuildVersionCodes.Kitkat)
            //            {
            //                root.RequestLayout();
            //                root.Invalidate();
            //            }

            //            _isShowing = true;
            _isShowing = true;

            //            ViewTreeObserver.PreDraw += OnPreDraw;
            //
            //            if (!_animated)
            //            {
            //                if (ShouldStartTimer())
            //                    StartTimer();
            //                return;
            //            }
            if (!_animated)
            {
                if (ShouldStartTimer())
                    StartTimer();
                return;
            }

//            var slideIn = AnimationUtils.LoadAnimation(Context, Resource.Animation.sb__in);
//            slideIn.AnimationEnd += (sender, args) =>
//                {
//                    if (_eventListener != null)
//                        _eventListener.OnShown(this);
//
//                    Post(() =>
//                        {
//                            _snackbarStart = DateTime.Now.Millisecond;
//
//                            if (_timeRemaining == -1)
//                                _timeRemaining = GetDuration();
//
//                            if (ShouldStartTimer())
//                                StartTimer();
//                        });
//                };
//            StartAnimation(slideIn);
            UIView.Transition(this, 0.3, UIViewAnimationOptions.CurveEaseIn | UIViewAnimationOptions.LayoutSubviews, () =>
                {
                    var frame = Frame;
                    Frame = new CGRect(frame.X, frame.Y -= 42, frame.Width, frame.Height);

                }, () =>
                {
                    if (_eventListener != null)
                        _eventListener.OnShown(this);

                    _snackbarStart = DateTime.Now.Millisecond;
                    
                    if (_timeRemaining == -1)
                        _timeRemaining = GetDuration();
                    
                    if (ShouldStartTimer())
                        StartTimer();
                });
        }

        //        private void OnPreDraw(object sender, ViewTreeObserver.PreDrawEventArgs e)
        //        {
        //            ViewTreeObserver.PreDraw -= OnPreDraw;
        //
        //            if (_eventListener != null)
        //            {
        //                _eventListener.OnShow(this);
        //                if (!_animated)
        //                    _eventListener.OnShown(this);
        //            }
        //        }

        private bool ShouldStartTimer()
        {
            return !IsIndefiniteDuration();
        }

        private bool IsIndefiniteDuration()
        {
            return GetDuration() == SnackbarDuration.Indefinite.GetDuration();
        }

        //        private bool IsNavigationBarHidden(ViewGroup root)
        //        {
        //            if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
        //                return false;
        //
        //            var viewFlags = (int) root.WindowSystemUiVisibility;
        //            return (viewFlags & (int) SystemUiFlags.LayoutHideNavigation) ==
        //                (int) SystemUiFlags.LayoutHideNavigation;
        //        }

        private void StartTimer()
        {
//            PostDelayed(_dismissRunnable, GetDuration());
            _timer = NSTimer.CreateTimer(GetDuration(), timer =>
                {
                    _dismissRunnable();
                });

//            #if __UNIFIED__
//            _fadeoutTimer = NSTimer.CreateTimer(duration, timer => DismissWorker ());
//            #else
//            _fadeoutTimer = NSTimer.CreateTimer (duration, DismissWorker);
//            #endif

            NSRunLoop.Main.AddTimer(_timer, NSRunLoopMode.Common);
        }

        private void StartTimer(long duration)
        {
//            PostDelayed(_dismissRunnable, duration);
            _timer = NSTimer.CreateTimer(duration, timer =>
                {
                    _dismissRunnable();
                });
        }

        public void Dismiss()
        {
            Dismiss(_animated);
        }

        private void Dismiss(bool animate)
        {
            if (_isDismissing)
                return;

            _isDismissing = true;

            if (_eventListener != null && _isShowing)
                _eventListener.OnDismiss(this);

            if (!animate)
            {
                Finish();
                return;
            }

//            var slideOut = AnimationUtils.LoadAnimation(Context, Resource.Animation.sb__out);
//            slideOut.AnimationEnd += (sender, args) => { Post(Finish); };
//            StartAnimation(slideOut);

            UIView.Transition(this, 0.3, UIViewAnimationOptions.CurveEaseOut | UIViewAnimationOptions.LayoutSubviews, () =>
                {
                    var frame = Frame;
                    Frame = new CGRect(frame.X, frame.Y += 42, frame.Width, frame.Height);
                }, () =>
                {
                    Finish();
                });
        }

        private void Finish()
        {
//            ClearAnimation();
//
//            var parent = (ViewGroup) Parent;
//            if (parent != null)
//                parent.RemoveView(this);
            var parent = Superview;
            if (parent != null)
                parent.RemoveFromSuperview();
//
            if (_eventListener != null && _isShowing)
                _eventListener.OnDismissed(this);

            _isShowing = false;
        }

        //        protected override void OnDetachedFromWindow()
        //        {
        //            base.OnDetachedFromWindow();
        //
        //            if (_dismissRunnable != null)
        //                RemoveCallbacks(_dismissRunnable);
        //        }

        public UIColor GetActionColor()
        {
            return _actionColor;
        }

        public string GetActionLabel()
        {
            return _actionLabel;
        }

        public UIColor GetTextColor()
        {
            return _textColor;
        }

        public UIColor GetColor()
        {
            return _color;
        }

        public string GetText()
        {
            return _text;
        }

        public long GetDuration()
        {
            return _customDuration == -1 ? _duration.GetDuration() : _customDuration;
        }

        public SnackbarType GetSnackbarType()
        {
            return _type;
        }

        public int GetOffset()
        {
            return _offset;
        }

        public bool IsAnimated()
        {
            return _animated;
        }

        public bool ShouldDismissOnActionClicked()
        {
            return _shouldDismissOnActionClicked;
        }

        public bool IsShowing()
        {
            return _isShowing;
        }

        public bool IsDismissed()
        {
            return !_isShowing;
        }

        private static int DpToPx(int dp, float scale)
        {
            return (int)(dp * scale + 0.5f);
        }

        //        private class RecyclerViewOnScrollListener : RecyclerView.OnScrollListener
        //        {
        //            private readonly Snackbar _snackbar;
        //
        //            public RecyclerViewOnScrollListener(Snackbar snackbar)
        //            {
        //                _snackbar = snackbar;
        //            }
        //
        //            public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
        //            {
        //                base.OnScrollStateChanged(recyclerView, newState);
        //                _snackbar.Dismiss();
        //            }
        //        }
    }
}

