using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Base", "Get.UI.Base")]
namespace Get.UI.Base
{
    /// <summary>
    /// Use the DesignerItem to make your controls moveabel on a Canvas Control
    /// http://www.codeproject.com/KB/WPF/WPFDiagramDesigner_Part2.aspx
    /// </summary>
    public class DesignerItem : ContentControl
    {
        /// <summary>
        /// Occurs when the Content of the DesignerItem changed
        /// </summary>
        public static readonly RoutedEvent ContentPropertyChangedEvent;

        /// <summary>
        /// Represents the method that handle the ContentPropertyChangedEvent routed event 
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data. </param>
        public delegate void ContentPropertyChangedEventHandler(object sender, RoutedContentPropertyChangedEventArgs e);

        static DesignerItem()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignerItem), new FrameworkPropertyMetadata(typeof(DesignerItem)));
            ContentProperty.OverrideMetadata(typeof(DesignerItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnContentPropertyChanged));
            ContentPropertyChangedEvent = EventManager.RegisterRoutedEvent("ContentPropertyChanged", RoutingStrategy.Bubble, typeof(ContentPropertyChangedEventHandler), typeof(DesignerItem));
            
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template != null)
            {
                this.MoveAbelItem  = this.Template.FindName("PART_MoveAbelItem", this) as MoveAbelItem;
                this.ContentPresenter = this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
                this.ResizeDecorator = this.Template.FindName("PART_DesignerItemDecorator", this) as ResizeDecorator;

                if (this.Content != null)
                {
                    RaisContentPropertyChangedEvent(this.Content as UIElement);
                }
            }
        }
        /// <summary>
        /// Callback function of the ContentProperty. This method raises the ContentPropertyChangedEvent
        /// http://msdn.microsoft.com/en-us/library/system.windows.propertychangedcallback.aspx
        /// </summary>
        /// <param name="pDependencyObject">The DependencyObject on which the property has changed value.</param>
        /// <param name="e">Provides data for various property changed events. Typically these events report effective value changes in the value of a read-only dependency property. Another usage is as part of a PropertyChangedCallback implementation.</param>
        private static void OnContentPropertyChanged(DependencyObject pDependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (pDependencyObject != null && pDependencyObject.GetType().Equals(typeof(DesignerItem)))
            {
                DesignerItem designerItem = pDependencyObject as DesignerItem;
                designerItem.RaisContentPropertyChangedEvent(e.NewValue as UIElement);

            }
        }
        /// <summary>
        /// Creates a RoutedContentPropertyChangedEventArgs object and determined if we can rais the the event
        /// </summary>
        /// <param name="e">The Control which was added to the DesignerItem</param>
        protected void RaisContentPropertyChangedEvent(UIElement e)
        {
            
            RoutedContentPropertyChangedEventArgs routedContentPropertyChangedEventArgs = new RoutedContentPropertyChangedEventArgs(ContentPropertyChangedEvent,
                e, this.MoveAbelItem, this.ResizeDecorator);

            //sadly the RaisContentPropertyChangedEvent method can be called when the designerItem hasn't applied the template. This means that the moveabelitem, ResizeDecorator and so on can be null.
            //but the event is only useful if we got all informations so we do not rais the event if we dont have all informations
            if (routedContentPropertyChangedEventArgs.NewContent != null && routedContentPropertyChangedEventArgs.MoveAbelItem != null && routedContentPropertyChangedEventArgs.ResizeDecorator != null)
            {
                this.RaiseEvent(routedContentPropertyChangedEventArgs);
            }
        }
        /// <summary>
        /// Sets IsSelected 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;

            if (designer != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                {
                    this.IsSelected = !this.IsSelected;
                }
                else
                {
                    if (!this.IsSelected)
                    {
                        designer.DeselectAll();
                        this.IsSelected = true;
                    }
                }
            }

            e.Handled = false;
        }

        /// <summary>
        /// Determined if the Control is selected
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        protected MoveAbelItem MoveAbelItem { get; set; }
        protected ContentPresenter ContentPresenter { get; set; }
        protected ResizeDecorator ResizeDecorator { get; set; }

        public event ContentPropertyChangedEventHandler ContentPropertyChanged
        {
            add
            {
                this.AddHandler(ContentPropertyChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(ContentPropertyChangedEvent, value);
            }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(DesignerItem), new UIPropertyMetadata(false, OnIsSelectedChanged));

        private static void OnIsSelectedChanged(DependencyObject pDependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (pDependencyObject != null && pDependencyObject.GetType().Equals(typeof(DesignerItem)))
            {
                DesignerItem designerItem = pDependencyObject as DesignerItem;

                designerItem.RaiseEvent(new RoutedEventArgs(DesignerItem.GotFocusEvent, designerItem));

            }

        }
    }
    /// <summary>
    /// Contains state information and event data associated with a ContentPropertyChangedEvent. 
    /// </summary>
    public class RoutedContentPropertyChangedEventArgs : RoutedEventArgs
    {
        private readonly MoveAbelItem _MoveAbelItem;
        private readonly UIElement _NewContent;
        private readonly ResizeDecorator _ResizeDecorator;

        public RoutedContentPropertyChangedEventArgs(UIElement pNewElement)
            : base()
        {
            this._NewContent = pNewElement;
        }
        public RoutedContentPropertyChangedEventArgs(RoutedEvent routedEvent, UIElement pNewElement)
            : base(routedEvent)
        {

            this._NewContent = pNewElement;
        }
        /// <summary>
        /// Initializes a new instance of the RoutedContentPropertyChangedEventArgs class using the supplied parameters. 
        /// </summary>
        /// <param name="routedEvent">The routed event identifier for this instance of the RoutedContentPropertyChangedEventArgs class.</param>
        /// <param name="pNewElement">The new UIElement which was added to the Content Property of the DesignerItem</param>
        /// <param name="pMoveAbelItem">The instance of the MoveAbelItem of the DesignerItem.</param>
        /// <param name="pResizeDecorator">The instance of the ResizeDecorator of the DesignerItem.</param>
        public RoutedContentPropertyChangedEventArgs(RoutedEvent routedEvent, UIElement pNewElement, MoveAbelItem pMoveAbelItem, ResizeDecorator pResizeDecorator)
            : base(routedEvent)
        {

            this._MoveAbelItem = pMoveAbelItem;
            this._NewContent = pNewElement;
            this._ResizeDecorator = pResizeDecorator;

        }
        public UIElement NewContent { get { return this._NewContent; } }
        public MoveAbelItem MoveAbelItem { get { return this._MoveAbelItem; } }
        public ResizeDecorator ResizeDecorator { get { return this._ResizeDecorator; } }

    }
}
