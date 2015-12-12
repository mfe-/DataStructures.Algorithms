using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace Get.UI.TimeKeeping
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected MouseHookListener _MouseHookListener;
        protected DispatcherTimer _Timer = new DispatcherTimer();
        protected ObservableCollection<WindowSession> WindowSessions = new ObservableCollection<WindowSession>();
        protected int _Elapsed_Time = 0;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);

            ResourceManager _resourceManager; _resourceManager = new ResourceManager(Assembly.GetExecutingAssembly().GetName().Name + ".Properties.Resources", Assembly.GetExecutingAssembly());
            this.Tray = Tray = new Get.Common.GUI.Tray(((System.Drawing.Icon)(_resourceManager.GetObject("Crystal_Clear_app_kodo"))));

            _MouseHookListener = new MouseHookListener(new GlobalHooker());
            _Timer.Interval = new TimeSpan(0, 0, 0,1, 0);

            _Timer.Tick += (sender, args) => { 
                _Elapsed_Time++; };

            Tray.CreateMenuItem("Beenden", false);
            Tray.CreateMenuItem("Arbeitszeit aufnehmen");
            Tray.CreateMenuItem("Pausieren");
            Tray.CreateMenuItem("Arbeitszeit beenden");
            Tray.CreateMenuItem("Arbeitszeit nachtragen");

            Tray.NotifyIcon.ContextMenu.MenuItems.Find("Arbeitszeit aufnehmen", false).First().Click += (sender, eargs) =>
            {
                _MouseHookListener.Enabled = true;
                LastActiveWindow = getPidName();
                _Timer.Start();
            };
            Tray.NotifyIcon.ContextMenu.MenuItems.Find("Arbeitszeit beenden", false).First().Click += (sender, eargs) =>
            {
                _Timer.Stop();
                MouseHookListener_MouseClick(sender, new System.Windows.Forms.MouseEventArgs(System.Windows.Forms.MouseButtons.Left,1,0,0,0));

                MainWindow w = new MainWindow();
                w.Show();
            };


            _MouseHookListener.MouseClick += new System.Windows.Forms.MouseEventHandler(MouseHookListener_MouseClick);







        }

        #region WindowsForeground
        /// <summary>
        /// The GetForegroundWindow function returns a handle to the foreground window.
        /// http://msdn2.microsoft.com/en-us/library/ms633505.aspx
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Get Window Thread ProcessId
        /// http://msdn2.microsoft.com/en-us/library/ms633522.aspx
        /// </summary>
        /// <param name="hWnd">Handle to the foreground window.</param>
        /// <param name="lpdwProcessId">ProcessId</param>
        /// <returns>ProcessId</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary>
        /// Get the Window title name
        /// http://msdn2.microsoft.com/en-us/library/ms633520.aspx
        /// </summary>
        /// <param name="hWnd">Handle to the foreground window.</param>
        /// <param name="lpString">Pointer to the buffer that will receive the text. If the string is as long or longer than the buffer, the string is truncated and terminated with a NULL character.</param>
        /// <param name="nMaxCount">Specifies the maximum number of characters to copy to the buffer, including the NULL character. If the text exceeds this limit, it is truncated.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// SetWindowPos Function
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        /// <param name="hWnd">[in] A handle to the window.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1); //oder public const int HWND_BOTTOM = 0x1;


        private const uint SWP_NOSIZE = 0x1;
        private const uint SWP_NOMOVE = 0x2;
        private const uint SWP_SHOWWINDOW = 0x40;

        /// <summary>
        /// Holt vom aktiven Fenster, das den Focus besitzt HWND
        /// </summary>
        /// <returns>HWND</returns>
        public IntPtr GetFgroundWindow()
        {
            return GetForegroundWindow();
        }
        /// <summary>
        /// Gibt die Prozessid zurück
        /// </summary>
        /// <param name="HWND">HWND</param>
        /// <returns>PID</returns>
        public int GetWindowThreadProcessId(IntPtr p_HWND)
        {
            uint processid;
            GetWindowThreadProcessId(GetFgroundWindow(), out processid);
            return (int)processid;
        }
        #endregion

        protected void MouseHookListener_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (getPidName() != LastActiveWindow)
            {
                WindowSession w = WindowSessions.Where(a => a.WindowTitle.Equals(LastActiveWindow)).FirstOrDefault();

                if (w == null)
                {
                    WindowSessions.Add(new WindowSession() { WindowTitle = LastActiveWindow, TimeSpent = _Elapsed_Time });
                }
                else
                {
                    w.TimeSpent = w.TimeSpent + _Elapsed_Time;
                }
                _Elapsed_Time = 0;
                LastActiveWindow = getPidName();
            }
        }
        public string getPidName()
        {
            //get active window pid
            int processid = GetWindowThreadProcessId(GetFgroundWindow());
            if (processid == 0) return string.Empty;
            Process app = new Process();
            app = Process.GetProcessById(processid);

            string _pidname = app.MainModule.FileVersionInfo.InternalName.ToLower() + " " + app.MainWindowTitle.ToLower();
            return _pidname;
        }
        public Get.Common.GUI.Tray Tray { get; set; }
        public string LastActiveWindow { get; set; }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _Timer.Stop();
            
            if (_MouseHookListener != null)
            {
                _MouseHookListener.Enabled = false;
                _MouseHookListener.MouseClick -= new System.Windows.Forms.MouseEventHandler(MouseHookListener_MouseClick);
                _MouseHookListener = null;
            }

            Tray.NotifyIcon.Dispose();
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (_MouseHookListener != null)
            {
                _MouseHookListener.Enabled = false;
                _MouseHookListener.MouseClick -= new System.Windows.Forms.MouseEventHandler(MouseHookListener_MouseClick);
                _MouseHookListener = null;
            }

            if (Tray != null)
            {
                Tray.NotifyIcon.Dispose();
            }
        }

    }
    public class WindowSession
    {
        public String WindowTitle { get; set; }
        public int TimeSpent { get; set; }
    }
}
