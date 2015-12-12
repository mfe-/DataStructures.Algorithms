using System;


namespace Get.Common.Cinch
{
    /// <summary>
    /// Available LogEntry options. 
    /// Abstracted to allow some level of UI Agnosticness
    /// </summary>
    public enum LogType
    {
        Error,
        Warning,
        FailureAudit,
        SuccessAudit,
        Information
    }
 
    /// <summary>
    /// This interface defines a very very simple logger interface
    /// to allow the ViewModel to log
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs to the event log using the params provided
        /// </summary>
        /// <param name="logType">The LogType to use.</param>
        /// <param name="logEntry">The actual logEntry string to be logged.</param>
        void Log(LogType logType, String logEntry);

        /// <summary>
        /// Logs to the event log using the params provided
        /// </summary>
        /// <param name="logType">The LogType to use.</param>
        /// <param name="ex">An Exception to be logged.</param>
        void Log(LogType logType, Exception ex);
    }
}
