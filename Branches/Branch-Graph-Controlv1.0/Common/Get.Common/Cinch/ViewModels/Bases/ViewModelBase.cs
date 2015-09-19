using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Linq.Expressions;

namespace Get.Common.Cinch
{
    /// <summary>
    /// Provides a base class for ViewModels to inherit from. This 
    /// base class provides the following
    /// <list type="Bullet">
    /// <item>Mediator pattern implementation</item>
    /// <item>Service resolution</item>
    /// <item>Window lifetime virtual method hooks</item>
    /// <item>INotifyPropertyChanged</item>
    /// <item>Workspace support</item>
    /// </list>
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable, IParentablePropertyExposer
    {
        #region Data
        static readonly Mediator mediator = new Mediator();
        private SimpleCommand closeActivePopUpCommand;
        private SimpleCommand activatedCommand;
        private SimpleCommand deactivatedCommand;
        private SimpleCommand loadedCommand;
        private SimpleCommand unloadedCommand;
        private SimpleCommand closeCommand;
        static ILoggerService logger = null;

        //workspace data
        private SimpleCommand closeWorkSpaceCommand;
        private Boolean isCloseable = true;

        /// <summary>
        /// Service resolver for view models.  Allows derived types to add/remove
        /// services from mapping.
        /// </summary>
        public static readonly ServiceProvider ServiceProvider = new ServiceProvider();

        /// <summary>
        /// This event should be raised to close the view.  Any view tied to this
        /// ViewModel should register a handler on this event and close itself when
        /// this event is raised.  If the view is not bound to the lifetime of the
        /// ViewModel then this event can be ignored.
        /// </summary>
        public event EventHandler<CloseRequestEventArgs> CloseRequest;

        /// <summary>
        /// This event should be raised to activate the UI.  Any view tied to this
        /// ViewModel should register a handler on this event and close itself when
        /// this event is raised.  If the view is not bound to the lifetime of the
        /// ViewModel then this event can be ignored.
        /// </summary>
        public event EventHandler<EventArgs> ActivateRequest;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructs a new ViewModelBase and wires up all the Window based Lifetime
        /// commands such as activatedCommand/deactivatedCommand/loadedCommand/closeCommand
        /// </summary>
        public ViewModelBase()
        {
            //Register all decorated methods to the Mediator
            Mediator.Register(this);

            #region Wire up Window/UserControl based Lifetime commands
            activatedCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => true,
                ExecuteDelegate = x => OnWindowActivated()
            };

            deactivatedCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => true,
                ExecuteDelegate = x => OnWindowDeactivated()
            };

            loadedCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => true,
                ExecuteDelegate = x => OnWindowLoaded()
            };

            unloadedCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => true,
                ExecuteDelegate = x => OnWindowUnloaded()
            };

            closeCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => true,
                ExecuteDelegate = x => OnWindowClose()
            };
            #endregion

            #region Wire up Workspace Command

            //This is used for popup control only
            closeWorkSpaceCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => true,
                ExecuteDelegate = x => ExecuteCloseWorkSpaceCommand()
            };

            #endregion

            //This is used for popup control only
            closeActivePopUpCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => true,
                ExecuteDelegate = x => OnCloseActivePopUp(x)
            };	
        }

        /// <summary>
        /// Registers the default service implemenations with the Unity container, and
        /// then configures Unity container (which allows for changes to be made to pick
        /// up overriden services within Unity configuration).
        /// And finally add all services found to a list of Core services which are available
        /// to the ViewModelBase class
        /// </summary>
        static ViewModelBase()
        {

            try
            {
                //regiser defaults
                RegisterDefaultServices();

                //configure Unity (there could be some different Service implementations
                //in the config that override the defaults just setup
                UnityConfigurationSection section = (UnityConfigurationSection)
                               ConfigurationManager.GetSection("unity");
                if (section != null && section.Containers.Count > 0)
                {
                    section.Containers.Default.Configure(UnitySingleton.Instance.Container);
                }
                
                //fetch the core service
                FetchCoreServiceTypes();
            }
            catch(Exception ex)
            {
                throw new ApplicationException("There was a problem configuring the Unity container\r\n" + ex.Message);
            }
        }
        #endregion

        #region Public/Protected Methods/Events
        /// <summary>
        /// This resolves a service type and returns the implementation.
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Implementation</returns>
        protected T Resolve<T>()
        {
            return ServiceProvider.Resolve<T>();
        }

        /// <summary>
        /// This raises the CloseRequest event to close the UI.
        /// </summary>
        public virtual void RaiseCloseRequest()
        {
            EventHandler<CloseRequestEventArgs> handlers = CloseRequest;

            // Invoke the event handlers
            if (handlers != null)
            {
                try
                {
                    handlers(this, new CloseRequestEventArgs(null));
                }
                catch (Exception ex)
                {
                    LogExceptionIfLoggerAvailable(ex);
                }
            }
        }

        /// <summary>
        /// This raises the CloseRequest event to close the UI.
        /// </summary>
        public virtual void RaiseCloseRequest(bool? dialogResult)
        {
            EventHandler<CloseRequestEventArgs> handlers = CloseRequest;

            // Invoke the event handlers
            if (handlers != null)
            {
                try
                {
                    handlers(this, new CloseRequestEventArgs(dialogResult));
                }
                catch (Exception ex)
                {
                    LogExceptionIfLoggerAvailable(ex);
                }
            }
        }

        /// <summary>
        /// This raises the ActivateRequest event to activate the UI.
        /// </summary>
        public virtual void RaiseActivateRequest()
        {
            EventHandler<EventArgs> handlers = ActivateRequest;

            // Invoke the event handlers
            if (handlers != null)
            {
                try
                {
                    handlers(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    LogExceptionIfLoggerAvailable(ex);
                }
            }
        }

        /// <summary>
        /// Allows Window.Activated hook
        /// </summary>
        protected virtual void OnWindowActivated()
        {
            //Should be overriden if required in inheritors
        }

        /// <summary>
        /// Allows Window.Deactivated hook
        /// </summary>
        protected virtual void OnWindowDeactivated()
        {
            //Should be overriden if required in inheritors
        }

        /// <summary>
        /// Allows Window.Loaded/UserControl.Loaded hook
        /// </summary>
        protected virtual void OnWindowLoaded()
        {
            //Should be overriden if required in inheritors
        }

        /// <summary>
        /// Allows Window.Unloaded/UserControl.Unloaded hook
        /// </summary>
        protected virtual void OnWindowUnloaded()
        {
            //Should be overriden if required in inheritors
        }

        /// <summary>
        /// Allows Window.Close hook
        /// </summary>
        protected virtual void OnWindowClose()
        {
            //Should be overriden if required in inheritors
        }
        #endregion

        #region Public Properties
 
        /// <summary>
        /// Mediator : Mediator = Messaging pattern
        /// </summary>
        public Mediator Mediator
        {
            get { return mediator; }
        }

        /// <summary>
        /// Logger : The ILoggerService implementation in use
        /// </summary>
        public ILoggerService Logger
        {
            get { return logger; }
        }

        /// <summary>
        /// ActivatedCommand : Window Lifetime command
        /// </summary>
        public SimpleCommand ActivatedCommand
        {
            get { return activatedCommand; }
        }

        /// <summary>
        /// DeactivatedCommand : Window Lifetime command
        /// </summary>
        public SimpleCommand DeactivatedCommand
        {
            get { return deactivatedCommand; }
        }

        /// <summary>
        /// LoadedCommand : Window/UserControl Lifetime command
        /// </summary>
        public SimpleCommand LoadedCommand
        {
            get { return loadedCommand; }
        }

        /// <summary>
        /// UnloadedCommand : Window/UserControl Lifetime command
        /// </summary>
        public SimpleCommand UnloadedCommand
        {
            get { return unloadedCommand; }
        }

        /// <summary>
        /// CloseCommand : Window Lifetime command
        /// </summary>
        public SimpleCommand CloseCommand
        {
            get { return closeCommand; }
        }

        /// <summary>
        /// CloseCommand : Close popup command
        /// </summary>
        public SimpleCommand CloseActivePopUpCommand
        {
            get { return closeActivePopUpCommand; }
        }

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public SimpleCommand CloseWorkSpaceCommand
        {
            get
            {
                return closeWorkSpaceCommand;
            }
        }


        /// <summary>
        /// Is the ViewModel closeable 
        /// </summary>
        static PropertyChangedEventArgs isCloseableChangeArgs =
            ObservableHelper.CreateArgs<ViewModelBase>(x => x.IsCloseable);

        public Boolean IsCloseable
        {
            get { return isCloseable; }
            set
            {
                isCloseable = value;
                NotifyPropertyChanged(isCloseableChangeArgs);
            }
        }

        /// <summary>
        /// Returns the user-friendly name of this object.
        /// Child classes can set this property to a new value,
        /// or override it to determine the value on-demand.
        /// </summary>
        public virtual string DisplayName { get; set; }


        #endregion

        #region Private Methods

        /// <summary>
        /// Executes the CloseWorkSpace Command
        /// </summary>
        private void ExecuteCloseWorkSpaceCommand()
        {
            CloseWorkSpaceCommand.CommandSucceeded = false;

            EventHandler<EventArgs> handlers = CloseWorkSpace;

            // Invoke the event handlers
            if (handlers != null)
            {
                try
                {
                    handlers(this, EventArgs.Empty);
                    CloseWorkSpaceCommand.CommandSucceeded = true;
                }
                catch
                {
                    Logger.Log(LogType.Error, "Error firing CloseWorkSpace event");
                }
            }

        }

        /// <summary>
        /// This method registers default services with the service provider. 
        /// These can be overriden by providing a new service implementation 
        /// and a new Unity config section in the project where the new service 
        /// implementation is defined 
        /// </summary>
        private static void RegisterDefaultServices()
        {
            //try and add Logger if there is one available
            try
            {
                UnitySingleton.Instance.Container.RegisterInstance(
                    typeof(ILoggerService), 
                    new WPFLoggerService());

                logger = (ILoggerService)UnitySingleton.Instance.Container.Resolve(
                    typeof(ILoggerService));

                //Although the ILoggerService is exposed as a regular property, we can
                //also add it, in case user want to get it using Resolve<T> method as
                //they do for resolving other services
                ServiceProvider.Add(typeof(ILoggerService), logger);
            }
            catch
            {
                throw new ApplicationException("There is a problem registering the Default Cinch services");
            }


            //try add other default services
            //users can override this using specific Unity App.Config
            //section entry
            try
            {
                //IUIVisualizerService : Register a default WPFUIVisualizerService
                UnitySingleton.Instance.Container.RegisterInstance(
                    typeof(IUIVisualizerService), new WPFUIVisualizerService());

                //IMessageBoxService : Register a default WPFMessageBoxService
                UnitySingleton.Instance.Container.RegisterInstance(
                    typeof(IMessageBoxService), new WPFMessageBoxService());

                //IOpenFileService : Register a default WPFOpenFileService
                UnitySingleton.Instance.Container.RegisterInstance(
                    typeof(IOpenFileService), new WPFOpenFileService());

                //ISaveFileService : Register a default WPFSaveFileService
                UnitySingleton.Instance.Container.RegisterInstance(
                    typeof(ISaveFileService), new WPFSaveFileService());

            }
            catch (ResolutionFailedException rex)
            {
                LogExceptionIfLoggerAvailable(rex);

            }
            catch (Exception ex)
            {
                LogExceptionIfLoggerAvailable(ex);
            }
        }


        /// <summary>
        /// This method registers services with the service provider.
        /// </summary>
        private static void FetchCoreServiceTypes()
        {
            try
            {
                //IMessageBoxService : Allows MessageBoxs to be shown 
                IMessageBoxService messageBoxService = 
                    (IMessageBoxService)UnitySingleton.Instance.Container.Resolve(
                        typeof(IMessageBoxService));
                ServiceProvider.Add(typeof(IMessageBoxService), messageBoxService);

                //IOpenFileService : Allows Opening of files 
                IOpenFileService openFileService = 
                    (IOpenFileService)UnitySingleton.Instance.Container.Resolve(
                        typeof(IOpenFileService));
                ServiceProvider.Add(typeof(IOpenFileService), openFileService);

                //ISaveFileService : Allows Saving of files 
                ISaveFileService saveFileService =
                    (ISaveFileService)UnitySingleton.Instance.Container.Resolve(
                        typeof(ISaveFileService));
                ServiceProvider.Add(typeof(ISaveFileService), saveFileService);

                //IUIVisualizerService : Allows popup management
                IUIVisualizerService uiVisualizerService =
                    (IUIVisualizerService)UnitySingleton.Instance.Container.Resolve(
                        typeof(IUIVisualizerService));
                ServiceProvider.Add(typeof(IUIVisualizerService), uiVisualizerService);

            }
            catch (ResolutionFailedException rex)
            {
                LogExceptionIfLoggerAvailable(rex);
            }
            catch (Exception ex)
            {
                LogExceptionIfLoggerAvailable(ex);
            }
        }

        /// <summary>
        /// Raises RaiseCloseRequest event, passing back correct DialogResult
        /// </summary>
        private void OnCloseActivePopUp(Object param)
        {
            if (param is Boolean)
            {
                // Close the dialog using DialogResult requested
                RaiseCloseRequest((bool)param);
                return;
            }

            //param is not a bool so try and parse it to a Bool
            Boolean popupAction=true;
            Boolean result = Boolean.TryParse(param.ToString(), out popupAction);
            if (result)
            {
                // Close the dialog using DialogResult requested
                RaiseCloseRequest(popupAction);
            }
            else
            {
                // Close the dialog passing back true
                RaiseCloseRequest(true);
            }
        }

        /// <summary>
        /// Logs a message if there is a ILoggerService available. And then throws
        /// new ApplicationException which should be caught somewhere external
        /// to this class
        /// </summary>
        /// <param name="ex">Exception to log</param>
        private static void LogExceptionIfLoggerAvailable(Exception ex)
        {
            if (logger != null)
                logger.Log(LogType.Error, ex);

            throw new ApplicationException(ex.Message);
        }
        #endregion

        #region Event(s)
        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler<EventArgs> CloseWorkSpace;
        #endregion 

        #region Debugging Aides

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify using pre-made PropertyChangedEventArgs
        /// </summary>
        /// <param name="args"></param>
        protected void NotifyPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Notify using String property name
        /// </summary>
        protected void NotifyPropertyChanged(String propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IParentablePropertyExposer
        /// <summary>
        /// Returns the list of delegates that are currently subscribed for the
        /// <see cref="System.ComponentModel.INotifyPropertyChanged">INotifyPropertyChanged</see>
        /// PropertyChanged event
        /// </summary>
        public Delegate[] GetINPCSubscribers()
        {
            return PropertyChanged == null ? null : PropertyChanged.GetInvocationList();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            this.OnDispose();
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

#if DEBUG
        /// <summary>
        /// Useful for ensuring that ViewModel objects are properly garbage collected.
        /// </summary>
        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", 
                this.GetType().Name, this.DisplayName, this.GetHashCode());
            System.Diagnostics.Debug.WriteLine(msg);
        }
#endif

        #endregion // IDisposable Members

    }
}
