using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Get.Common.Cinch
{
    /// <summary>
    /// This class implements the ILoggerService for WPF purposes.
    /// </summary>
    public class WPFLoggerService : ILoggerService
    {
        #region Data
        private String eventSource = String.Empty;
        #endregion

        #region Ctor
        public WPFLoggerService()
        {
            eventSource = "Cinch";
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The EventSource to use for EventLog entries
        /// </summary>
        public String EventSource
        {
            get { return eventSource; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    eventSource = value;
                }
            }
        }
        #endregion

        #region ILoggerService Members

        /// <summary>
        /// Creates a log entry using the parameters provided
        /// </summary>
        /// <param name="logType">The LogType to use</param>
        /// <param name="logEntry">The log message</param>
        public void Log(LogType logType, string logEntry)
        {
            CreateLogSource();
            EventLog.WriteEntry(eventSource, logEntry, TranslateToEventLogEntryType(logType));

        }

        /// <summary>
        /// Creates a log entry using the parameters provided
        /// </summary>
        /// <param name="logType">The LogType to use</param>
        /// <param name="ex">The Exception from which to log the Exception.Message</param>
        public void Log(LogType logType, Exception ex)
        {
            CreateLogSource();
            EventLog.WriteEntry(eventSource, ex.Message.ToString() ?? String.Empty, 
                TranslateToEventLogEntryType(logType));
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Translates a LogType to a Windows EventLogEntryType. 
        /// </summary>
        /// <param name="logType">The Cinch.LogType</param>
        /// <returns>A System.Diagnostics.EventLogEntryType</returns>
        private EventLogEntryType TranslateToEventLogEntryType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                    return EventLogEntryType.Error;
                case LogType.FailureAudit:
                    return EventLogEntryType.FailureAudit;
                case LogType.Information:
                    return EventLogEntryType.Information;
                case LogType.SuccessAudit:
                    return EventLogEntryType.SuccessAudit;
                case LogType.Warning:
                    return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Information;
            }
        }

        /// <summary>
        /// Creates a EventLogSource
        /// </summary>
        private void CreateLogSource()
        {
            if (!EventLog.Exists(eventSource))
            {
                try
                {
                    EventLog.CreateEventSource(eventSource, "Application");
                }
                catch { }
            }
        }
        #endregion
    }
}
