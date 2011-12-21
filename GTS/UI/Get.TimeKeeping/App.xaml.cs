using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Resources;
using System.Reflection;

namespace Get.UI.TimeKeeping
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Get.Common.GUI.Tray Tray { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);

            ResourceManager _resourceManager;  _resourceManager = new ResourceManager(Assembly.GetExecutingAssembly().GetName().Name + ".Properties.Resources", Assembly.GetExecutingAssembly());
            this.Tray = Tray = new Get.Common.GUI.Tray(((System.Drawing.Icon)(_resourceManager.GetObject("Crystal_Clear_app_kodo"))));

            Tray.CreateMenuItem("Beenden", false);
            Tray.CreateMenuItem("Arbeitszeit aufnehmen");
            Tray.CreateMenuItem("Pausieren");
            Tray.CreateMenuItem("Arbeitszeit beenden");
            Tray.CreateMenuItem("Arbeitszeit nachtragen");
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Tray.NotifyIcon.Dispose();
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (Tray != null)
            {
                Tray.NotifyIcon.Dispose();
            }
        }
        
    }
}
