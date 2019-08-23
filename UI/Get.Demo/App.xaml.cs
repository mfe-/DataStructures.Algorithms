using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;

namespace DataStructures.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
           /// <summary>
        /// Tritt auf, wenn die Run-Methode des Application-Objekts aufgerufen wird.
        /// http://msdn.microsoft.com/de-de/library/system.windows.application.startup.aspx
        /// </summary>
        /// <param name="e">Enthält die Ereignisdaten für das Startup-Ereignis.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);


            if (System.Diagnostics.Debugger.IsAttached)
            {
                //Binding Errors ausgeben
                PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
                PresentationTraceSources.DataBindingSource.Switch.Level =
                    SourceLevels.Warning | SourceLevels.Error;
            }
        }
         /// <summary>
        /// Tritt auf, wenn durch eine Anwendung eine Ausnahme ausgelöst wird, die jedoch nicht behandelt wird.
        /// Schreibt die Exception in eine error.log Datei, welche sich im Roaming Verzeichnis /Get/FileSync.GUI befindet
        /// </summary>
        /// <param name="sender">Objekt das Ereignis ausgelöst hat.</param>
        /// <param name="e">Enthält die Ereignisdaten für das DispatcherUnhandledException-Ereignis.</param>
        protected void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {

        }
    }
}
