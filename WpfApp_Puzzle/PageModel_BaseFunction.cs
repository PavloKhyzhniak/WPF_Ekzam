using System.Windows;

namespace WpfApp_Puzzle
{
    public static class PageModel_BaseFunction
    {

        public static void HidePage(object obj)
        {
            if (obj == null)
                return;

            if (obj is Window window)
                if (window.WindowState != WindowState.Minimized)
                {
                    window.WindowState = WindowState.Minimized;
                }
        }

        public static void MinimizedPage(object obj)
        {
            if (obj == null)
                return;

            if (obj is Window window)
                if (window.WindowState != WindowState.Maximized)
                    window.WindowState = WindowState.Maximized;
                else
                    window.WindowState = WindowState.Normal;
        }

        public static void ClosePage(object obj)
        {
            if (obj == null)
                return;

            if (obj is Window window)
                window.Close();
        }

        public static void DragPage(object obj)
        {
            if (obj == null)
                return;

            if (obj is Window window)
                window.DragMove();
        }
    }
}
