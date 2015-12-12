using System;
using System.Collections.Generic;
using System.Windows;


namespace Get.Common.Cinch
{
    /// <summary>
    /// This class implements the IOpenFileService for Unit testing purposes.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    ///        TestOpenFileService testOpenFileService =
    ///            (TestOpenFileService)
    ///                ViewModelBase.ServiceProvider.Resolve<IOpenFileService>();
    ///                
    ///        //Queue up the response we expect for our given TestOpenFileService
    ///        //for a given ICommand/Method call within the test ViewModel
    ///        testOpenFileService.ShowDialogResponders.Enqueue
    ///            (() =>
    ///              {
    ///                testOpenFileService.FileName = @"c:\test.txt";
    ///                return true;
    ///              }
    ///            );
    /// ]]>
    /// </example>
    public class TestOpenFileService : IOpenFileService
    {
        #region Data
        /// <summary>
        /// Should be set in the actual Test file delegate callback
        /// </summary>
        private String fileName = String.Empty;
        private String filter = String.Empty;
        private String initialDirectory = String.Empty;
        
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
        public TestOpenFileService()
        {
            ShowDialogResponders = new Queue<Func<bool?>>();
        }
        #endregion

        #region IOpenFileService Members
        /// <summary>
        /// This method should show a window that allows a file to be selected
        /// </summary>
        /// <param name="owner">The owner window of the dialog</param>
        /// <returns>A bool from the ShowDialog call</returns>
        public bool? ShowDialog(Window owner)
        {
            if (ShowDialogResponders.Count == 0)
                throw new ApplicationException(
                    "TestOpenFileService ShowDialog method expects a Func<bool?> callback \r\n" +
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

        #endregion
    }
}
