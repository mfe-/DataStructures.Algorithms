using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;


[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml", "Get.Common")]
namespace Get.Common
{
    public static class GUI
    {
        private static FrameworkElement GetFrameworkElementParent(FrameworkElement element, string name)
        {
            FrameworkElement parent = VisualTreeHelper.GetParent(element) as FrameworkElement;
            if (parent != null)
            {
                if (parent.Name == name)
                {
                    return parent;
                }
                return GetFrameworkElementParent(parent, name);
            }
            return null;

        }
        /// <summary>
        /// 
        /// http://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {

            T parent = VisualTreeHelper.GetParent(child) as T;

            if (parent != null)

                return parent;

            else

                return FindParent<T>(VisualTreeHelper.GetParent(child));

        }

        public class Tray
        {
            private NotifyIcon _notico;
            private bool _Animate = false;
            private ContextMenu _contextMenu = new ContextMenu();

            public Tray()
            {
                Initialize();
            }
            public Tray(System.Drawing.Icon pIcon)
            {
                Initialize();
                _notico.Icon = pIcon;
            }
            /// <summary>
            /// Initialisiert das NotifyIcon
            /// </summary>
            private void Initialize()
            {
                // NotifyIcon erzeugen
                _notico = new NotifyIcon();
                _notico.Visible = true;

                ContextMenu contextMenu = new ContextMenu();

                // Kontextmenüeinträge erzeugen

                _notico.ContextMenu = _contextMenu;

            }
            public void CreateMenuItem(String pName)
            {
                MenuItem menuItem = new MenuItem();
                menuItem = new MenuItem();
                menuItem.Index = 1;
                menuItem.Name = pName;
                menuItem.Text = "&" + menuItem.Name;

                _contextMenu.MenuItems.Add(menuItem);
            }
            public void CreateMenuItem(String pName, bool pTrue)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Index = 2;
                menuItem.Name = pName;
                menuItem.Text = "&" + menuItem.Name;
                menuItem.Click += (sender, e) =>
                {
                    MenuItem m = (MenuItem)sender;
                    m.Checked = !m.Checked;
                };
                menuItem.Checked = pTrue;

                _contextMenu.MenuItems.Add(menuItem);
            }
            public NotifyIcon NotifyIcon
            {
                get
                {
                    return _notico;
                }
            }
        }
        /// <summary>
        /// http://blogs.msdn.com/adam_nathan/archive/2006/05/04/589686.aspx
        /// Aero Glass API
        /// </summary>
        public static class DWMApi
        {
            [DllImport("dwmapi.dll", PreserveSig = false)]
            public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

            [DllImport("dwmapi.dll", PreserveSig = false)]
            public static extern bool DwmIsCompositionEnabled();

            public struct MARGINS
            {
                public MARGINS(Thickness t)
                {
                    Left = (int)t.Left;
                    Right = (int)t.Right;
                    Top = (int)t.Top;
                    Bottom = (int)t.Bottom;
                }
                public int Left;
                public int Right;
                public int Top;
                public int Bottom;
            }

            /// <summary>
            /// Aktiviert Aero Glass innerhalb eines Fenster
            /// Diese Funktion in protected override void OnSourceInitialized(EventArgs e) aufrufen.
            /// </summary>
            /// <param name="pWindow">Fenster bei dem Aero Glass aktiviert werden soll.</param>
            public static void ExtendGlass(Window pWindow)
            {
                Thickness thickness = new Thickness(-1);
                if (!DwmIsCompositionEnabled()) return;

                IntPtr hwnd = new WindowInteropHelper(pWindow).Handle;
                if (hwnd == IntPtr.Zero) throw new InvalidOperationException("The Window must be shown before extending glass.");

                pWindow.Background = Brushes.Transparent;
                HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

                MARGINS margins = new MARGINS(thickness);
                DwmExtendFrameIntoClientArea(hwnd, ref margins);
            }

        }

    }
}
