using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Lifetime;

namespace Get.Common.Remoting
{
    public delegate void RemotePropertyChangedHandler(object sender, RemotePropertyChangedEventArgs e);
    public delegate void RemoteClientInfoHandler(object client);

    /// <summary>
    /// Diese Klasse wird auf dem Server erstellt. Der Client holt sie ab und registriert sich.
    /// Basis: Two-way Remoting with Callbacks and Events http://www.codeproject.com/KB/IP/TwoWayRemoting.aspx
    /// .NET-Remoting: http://msdn.microsoft.com/de-de/library/72x4h507.aspx
    /// </summary>
    public class RemoteServiceTalk : MarshalByRefObject
    {
        /// <summary>
        /// Registrierte Clienten
        /// </summary>
        private static IList<RemoteClient> _list = new List<RemoteClient>();

        /// <summary>
        /// Diese Methode wird vom Clienten aufgerufen. Er registriert sich um Informationen vom RemoteService (Server zu erhalten).
        /// </summary>
        /// <param name="sender">Objekt das Ereignis ausgelöst hat. (Client)</param>
        /// <param name="htc">Event über das er registriert werden soll. Übergeben Sie null, wenn Sie nicht über Ereignise vom
        /// RemoteService informiert werden möchten.</param>
        public void RegisterRemoteClient(object sender, RemotePropertyChangedHandler htc)
        {
            _list.Add(new RemoteClient(sender, htc));

            //Schauen ob der RemoteService (Server) das Event für NewClients abonniert hat
            if (_NewClient != null)
                _NewClient(sender);
        }

        /// <summary>
        /// Diese Methode wird vom RemoteService (Server) aufgerufen, wenn er den Clients etwas schicken will
        /// </summary>
        /// <param name="sender">Objekt das Ereignis ausgelöst hat. (Server)</param>
        /// <param name="e">Informationen für die Clienten</param>
        public static void RaiseHostToClient(object sender, RemotePropertyChangedEventArgs e)
        {
            foreach (RemoteClient client in _list)
            {
                if (client.HostToClient != null)
                    client.HostToClient(sender, e);
            }
        }


        /// <summary>
        /// Da drinen speichern wir einen Funktion welche ausgefüghrt werden soll wenn sich ein neuer Benutzer regestriert
        /// Setzen Sie diese Eigenschaft wie folgt: new RemoteClientInfoHandler(Methodenname)
        /// </summary>
        private static RemoteClientInfoHandler _NewClient;
        public static RemoteClientInfoHandler NewClient
        {
            get { return _NewClient; }
            set { _NewClient = value; }
        }

        /// <summary>
        /// Diese Eigenschaft speichert eine Funktion wenn der Client was zum host schicken will.
        /// Setzen Sie diese Eigenschaft wie folgt: new RemoteClientInfoHandler(Methodenname)
        /// </summary>
        private static RemoteClientInfoHandler _ClientToHostDelegate;
        public static RemoteClientInfoHandler ClientToHostDelegate
        {
            get { return _ClientToHostDelegate; }
            set { _ClientToHostDelegate = value; }
        }

        // a thread-safe queue that will contain any message objects that should
        // be send to the server
        private static Queue _ClientToServer = Queue.Synchronized(new Queue());

        // this instance method allows a client to send a message to the server
        public void SendMessageToServer(object Message)
        {
            _ClientToServer.Enqueue(Message);
        }

        public static Queue ClientToServerQueue
        {
            get { return _ClientToServer; }
        }

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            lease.InitialLeaseTime = new TimeSpan(8, 0, 0);
            return lease;
        }

        /// <summary>
        /// In dieser Klasse speichern wir unsere Clienten mit samt den Informationen, wie z.B. überwelche Events er informiert werden will
        /// </summary>
        private class RemoteClient
        {
            /// <summary>
            /// Konstruktor
            /// </summary>
            /// <param name="client">Objekt das Client representiert</param>
            /// <param name="HostToClient">Event das abonniert wurde.</param>
            public RemoteClient(object client, RemotePropertyChangedHandler HostToClient)
            {
                _Client = client;
                _HostToClient = HostToClient;
            }
            /// <summary>
            /// Hier speichern wir das Objekt vom Clienten rein. Kann beispielsweise als identifaktion verwendet werden.
            /// </summary>
            private object _Client;
            public object Client
            {
                get { return _Client; }
            }
            /// <summary>
            /// Wenn dieser Delegate != null ist, erhält der Client Events
            /// </summary>
            private RemotePropertyChangedHandler _HostToClient;
            public RemotePropertyChangedHandler HostToClient
            {
                get { return _HostToClient; }
            }
        }
    }
    [Serializable()]
    public class RemotePropertyChangedEventArgs : EventArgs
    {
        private readonly string _PropertyName;
        private readonly object _PropertyValue;
        public RemotePropertyChangedEventArgs(string pPropertyName, object pPropertyValue)
            : base()
        {
            this._PropertyName = pPropertyName;
            this._PropertyValue = pPropertyValue;
        }
        /// <summary>
        /// Name des Properties das sich geändert hat.
        /// </summary>
        public string PropertyName { get { return this._PropertyName; } }
        /// <summary>
        /// Wert des geänderten Properties
        /// </summary>
        public object PropertyValue { get { return this._PropertyValue; } }
    }
    /// <summary>
    /// Die Klasse brauchen wir, damit der Server Informationen zum Client schicken kann. Der Client abonniert diese Events.
    /// Da kommen die ganzen Events rein, die der Client abonnieren kann
    /// </summary>
    public class RemoteServiceEvents : MarshalByRefObject
    {
        public RemoteServiceEvents() { }

        public event RemotePropertyChangedHandler OnHostToClient;

        [OneWay]
        public void HandleToClient(object sender, RemotePropertyChangedEventArgs e)
        {
            if (OnHostToClient != null)
                OnHostToClient(sender, e);
        }
    }
    public static class Remoting
    {
        public static String GetURLForObject(MarshalByRefObject obj)
        {
            // trying for CAOs
            ObjRef o = RemotingServices.GetObjRefForProxy(obj);
            if (o != null)
            {
                foreach (object data in o.ChannelInfo.ChannelData)
                {
                    ChannelDataStore ds = data as ChannelDataStore;
                    if (ds != null)
                    {
                        return ds.ChannelUris[0] + o.URI;
                    }
                }
            }
            else
            {
                // either SAO or not remote!
                String URL = RemotingServices.GetObjectUri(obj);
                return URL;
            }
            return null;
        }
    }

}
