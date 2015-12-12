using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System;
using System.Linq;


/// <summary>
/// This forces a TextBoxBase control to be numeric-entry only
/// By using an attached behaviour
/// </summary>
namespace Get.Common.Cinch
{
    /// <summary>
    /// This forces a TextBoxBase control to be numeric-entry only
    /// </summary>
    /// <example>
    /// <![CDATA[  <TextBox Cinch:NumericTextBoxBehavior.IsEnabled="True" />  ]]>
    /// </example>
    public static class NumericTextBoxBehavior
    {
        #region IsEnabled DP
        /// <summary>
        /// Dependency Property for turning on numeric behavior in a TextBox.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", 
                typeof(bool), typeof(NumericTextBoxBehavior),
                    new UIPropertyMetadata(false, OnEnabledStateChanged));

        /// <summary>
        /// Attached Property getter for the IsEnabled property.
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <returns>Current property value</returns>
        public static bool GetIsEnabled(DependencyObject source)
        {
            return (bool)source.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// Attached Property setter for the IsEnabled property.
        /// </summary>
        /// <param name="source">Dependency Object</param>
        /// <param name="value">Value to set on the object</param>
        public static void SetIsEnabled(DependencyObject source, bool value)
        {
            source.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// This is the property changed handler for the IsEnabled property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnEnabledStateChanged(DependencyObject sender, 
            DependencyPropertyChangedEventArgs e)
        {
            TextBoxBase tbb = sender as TextBoxBase;
            if (tbb == null)
                return;

            tbb.PreviewKeyDown -= OnKeyDown;
            DataObject.RemovePastingHandler(tbb, OnClipboardPaste);

            bool b = ((e.NewValue != null && e.NewValue.GetType() == typeof(bool))) ? 
                (bool)e.NewValue : false;
            if (b)
            {
                tbb.PreviewKeyDown += OnKeyDown;
                DataObject.AddPastingHandler(tbb, OnClipboardPaste);
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// This method handles paste and drag/drop events onto the TextBox.  It restricts the character
        /// set to numerics and ensures we have consistent behavior.
        /// </summary>
        /// <param name="sender">TextBox sender</param>
        /// <param name="e">EventArgs</param>
        private static void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
        {
            string text = e.SourceDataObject.GetData(e.FormatToApply) as string;
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Count(ch => !Char.IsNumber(ch)) == 0)
                    return;
            }
            e.CancelCommand();
        }

        /// <summary>
        /// This checks the PreviewKeyDown on the TextBox and 
        /// constrains it to a numeric value.
        /// </summary>
        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Tab ||
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                return;

            e.Handled = true;
        }
        #endregion
    }
}