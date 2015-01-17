namespace SnackbarSharp
{
    public interface IEventListener
    {
        void OnShow(Snackbar snackbar);

        void OnShown(Snackbar snackbar);

        void OnDismiss(Snackbar snackbar);

        void OnDismissed(Snackbar snackbar);
    }
}
