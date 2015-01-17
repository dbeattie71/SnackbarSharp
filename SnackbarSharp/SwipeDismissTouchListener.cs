using System;
using Android.Animation;
using Android.Views;

namespace SnackbarSharp
{
    public class SwipeDismissTouchListener : AnimatorListenerAdapter, View.IOnTouchListener
    {
        // Transient properties
        private float _downX;
        private float _downY;
        private bool _swiping;
        private int _swipingSlop;
        private float _translationX;
        private VelocityTracker _velocityTracker;
        private int _viewWidth = 1; // 1 and not 0 to prevent dividing by zero
        private readonly long _animationTime;
        private readonly Func<object, bool> _canDismiss;
        private readonly int _maxFlingVelocity;
        private readonly int _minFlingVelocity;
        private readonly Action<View, object> _onDismiss;
        private readonly Action<bool> _pauseTimer;
        // Cached ViewConfiguration and system-wide constant values
        private readonly int _slop;
        private readonly Object _token;
        // Fixed properties
        private readonly View _view;

        public SwipeDismissTouchListener(View view,
                                         Object token,
                                         Func<object, bool> canDismiss,
                                         Action<View, object> onDismiss,
                                         Action<bool> pauseTimer)
        {
            var vc = ViewConfiguration.Get(view.Context);
            _slop = vc.ScaledTouchSlop;
            _minFlingVelocity = vc.ScaledMinimumFlingVelocity * 16;
            _maxFlingVelocity = vc.ScaledMaximumFlingVelocity;
            _animationTime = view.Context.Resources.GetInteger(Android.Resource.Integer.ConfigShortAnimTime);
            _view = view;
            _token = token;

            _canDismiss = canDismiss;
            _onDismiss = onDismiss;
            _pauseTimer = pauseTimer;
            //Callbacks = callbacks;
        }

        public bool OnTouch(View view, MotionEvent motionEvent)
        {
            // offset because the view is translated during swipe
            motionEvent.OffsetLocation(_translationX, 0);

            if (_viewWidth < 2)
                _viewWidth = _view.Width;

            switch (motionEvent.ActionMasked)
            {
                case MotionEventActions.Down:
                    return Down(motionEvent);

                case MotionEventActions.Up:
                    Up(motionEvent);
                    break;

                case MotionEventActions.Cancel:
                    if (_velocityTracker == null)
                        break;

                    Cancel();
                    break;

                case MotionEventActions.Move:
                    if (_velocityTracker == null)
                        break;

                    if (Move(motionEvent))
                        return true;
                    break;
            }

            return false;
        }

        private bool Move(MotionEvent motionEvent)
        {
            _velocityTracker.AddMovement(motionEvent);
            var deltaX = motionEvent.RawX - _downX;
            var deltaY = motionEvent.RawY - _downY;
            if (Math.Abs(deltaX) > _slop && Math.Abs(deltaY) < Math.Abs(deltaX) / 2)
            {
                _swiping = true;
                _swipingSlop = (deltaX > 0 ? _slop : -_slop);

                if (_view.Parent != null)
                    _view.Parent.RequestDisallowInterceptTouchEvent(true);

                // Cancel listview's touch
                var cancelEvent = MotionEvent.Obtain(motionEvent);
                cancelEvent.Action =
                    (MotionEventActions)
                        ((int) MotionEventActions.Cancel |
                         (motionEvent.ActionIndex << (int) MotionEventActions.PointerIndexShift));

                _view.OnTouchEvent(cancelEvent);
                cancelEvent.Recycle();
            }

            if (_swiping)
            {
                _translationX = deltaX;
                _view.TranslationX = deltaX - _swipingSlop;
                // TODO: use an ease-out interpolator or such
                _view.Alpha = Math.Max(0f, Math.Min(1f, 1f - 2f * Math.Abs(deltaX) / _viewWidth));
                return true;
            }
            return false;
        }

        private void Cancel()
        {
            _view.Animate()
                .TranslationX(0)
                .Alpha(1)
                .SetDuration(_animationTime)
                .SetListener(null);
            _velocityTracker.Recycle();
            _velocityTracker = null;
            _translationX = 0;
            _downX = 0;
            _downY = 0;
            _swiping = false;
        }

        private void Up(MotionEvent motionEvent)
        {
            if (_velocityTracker == null)
                return;

            _pauseTimer(false);
            var deltaX = motionEvent.RawX - _downX;
            _velocityTracker.AddMovement(motionEvent);
            _velocityTracker.ComputeCurrentVelocity(1000);
            var velocityX = _velocityTracker.XVelocity;
            var absVelocityX = Math.Abs(velocityX);
            var absVelocityY = Math.Abs(_velocityTracker.YVelocity);
            var dismiss = false;
            var dismissRight = false;
            if (Math.Abs(deltaX) > _viewWidth / 2 && _swiping)
            {
                dismiss = true;
                dismissRight = deltaX > 0;
            }
            else if (_minFlingVelocity <= absVelocityX && absVelocityX <= _maxFlingVelocity
                     && absVelocityY < absVelocityX
                     && absVelocityY < absVelocityX && _swiping)
            {
                // dismiss only if flinging in the same direction as dragging
                dismiss = (velocityX < 0) == (deltaX < 0);
                dismissRight = _velocityTracker.XVelocity > 0;
            }
            if (dismiss)
            {
                // dismiss

                _view.Animate()
                    .TranslationX(dismissRight ? _viewWidth : -_viewWidth)
                    .Alpha(0)
                    .SetDuration(_animationTime)
                    .SetListener(this);
            }
            else if (_swiping)
            {
                // cancel
                _view.Animate()
                    .TranslationX(0)
                    .Alpha(1)
                    .SetDuration(_animationTime)
                    .SetListener(null);
            }
            if (_velocityTracker != null)
            {
                _velocityTracker.Recycle();
                _velocityTracker = null;
            }
            _translationX = 0;
            _downX = 0;
            _downY = 0;
            _swiping = false;
        }

        private bool Down(MotionEvent motionEvent)
        {
            // TODO: ensure this is a finger, and set a flag
            _downX = motionEvent.RawX;
            _downY = motionEvent.RawY;
            if (_canDismiss(_token))
            {
                _pauseTimer(true);
                _velocityTracker = VelocityTracker.Obtain();
                _velocityTracker.AddMovement(motionEvent);
            }
            return false;
        }

        public override void OnAnimationEnd(Animator animation)
        {
            PerformDismiss();
        }

        private void PerformDismiss()
        {
            _onDismiss(_view, _token);
        }
    }
}
