using System;
using System.Collections.Generic;
using System.Windows;


namespace Get.Common.Cinch
{
    /// <summary>
    /// This class implements the ISaveFileService for Unit testing purposes.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    ///        TestSaveFileService testSaveFileService =
    ///            (TestSaveFileService)
    ///                ViewModelBase.ServiceProvider.Resolve<IOpenSaveService>();
    ///                
    ///         //Queue up the response we expect for our given TestSaveFileService
    ///         //for a given ICommand/Method call within the test ViewModel
    ///         testSaveFileService.ShowDialogResponders.Enqueue
    ///         (() =>
    ///         {
	///             string path = @"c:\test.txt";
    ///             testSaveFileService.FileName = path ;
    ///             return true;
    ///         }
    ///);
    /// ]]>
    /// </example>
    public class TestSaveFileService : ISaveFileService
    {
        #region Data
        /// <summary>
        /// Should be set in the actual Test file delegate callback
        /// </summary>
        private String fileName = String.Empty;
        private String filter = String.Empty;
        private String initialDirectory = String.Empty;
        private Boolean overwritePrompt=false;
        
        /// <summary>
        /// Queue of callback delegates for the ShowDialog methods expected
        /// for the item under test
        /// </summary>
        public Queue<Func<bool?>> ShowDialogResponders { get; set; }

        #endregion

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public TestSaveFileService()
        {
            ShowDialogResponders = new Queue<Func<bool?>>();
        }
        #endregion

        #region ISaveFileService Members
        /// <summary>
        /// This method should show a window that allows a file to be saved
        /// </summary>
        /// <param name="owner">The owner window of the dialog</param>
        /// <returns>A bool from the ShowDialog call</returns>
        public bool? ShowDialog(Window owner)
        {
            if (ShowDialogResponders.Count == 0)
                throw new ApplicationException(
                    "TestSaveFileService ShowDialog method expects a Func<bool?> callback \r\n" +
                    "delegate to be enqueued for each ShowDialog call");
            else
            {
                Func<bool?> responder = ShowDialogResponders.Dequeue();
                return responder();
            }
        }

        /// <summary>
        /// FileName : Set in Test file 
        /// delegate callback (This MUST be done for the Test implementation 
        /// to work the same as actual WPF service implementation)
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Filter : Set in Test file 
        /// delegate callback (if required)
        /// </summary>
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        /// <summary>
        /// InitialDirectory : Set in Test file 
        /// delegate callback (if required)
        /// </summary>
        public string InitialDirectory
        {
            get { return initialDirectory; }
            set { initialDirectory = value; }
        }

        /// <summary>
        /// OverwritePrompt : Set in Test file 
        /// delegate callback (if required)
        /// </summary>
        public bool OverwritePrompt
        {
            get { return overwritePrompt; }
            set { overwritePrompt = value; }
        }
        #endregion
    }
}
