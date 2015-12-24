using System;
using System.Windows;


namespace Get.Common.Cinch
{
    /// <summary>
    /// This interface defines a interface that will allow 
    /// a ViewModel to save a file
    /// </summary>
    public interface ISaveFileService
    {
        /// <summary>
        /// FileName
        /// </summary>
        Boolean OverwritePrompt { get; set; }

        /// <summary>
        /// FileName
        /// </summary>
        String FileName { get; set; }

        /// <summary>
        /// Filter
        /// </summary>
        String Filter { get; set; }

        /// <summary>
        /// Filter
        /// </summary>
        String InitialDirectory { get; set; }

        /// <summary>
        /// This method should show a window that allows a file to be saved
        /// </summary>
        /// <param name="owner">The owner window of the dialog</param>
        /// <returns>A bool from the ShowDialog call</returns>
        bool? ShowDialog(Window owner);
    }
}
