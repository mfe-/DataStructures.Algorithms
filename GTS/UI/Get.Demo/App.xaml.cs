using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using System.Reflection;
using System.Resources;
using System.IO;
using System.Collections;

namespace Get.Demo
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
            
            this.LoadXaml(System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Get.Common.dll");

        }
        public void LoadXaml(String Assemblypath)
        {
            var assembly = Assembly.LoadFile(Assemblypath);
            var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".g.resources");
            var resourceReader = new ResourceReader(stream);

            foreach (DictionaryEntry resource in resourceReader)
            {
                if (new FileInfo(resource.Key.ToString()).Extension.Equals(".baml"))
                {
                    Uri uri = new Uri("/" + assembly.GetName().Name + ";component/" + resource.Key.ToString().Replace(".baml", ".xaml"), UriKind.Relative);
                    ResourceDictionary skin = Application.LoadComponent(uri) as ResourceDictionary;
                    this.Resources.MergedDictionaries.Add(skin);
                }
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
