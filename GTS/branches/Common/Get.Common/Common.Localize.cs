using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Threading;
using System.Windows.Data;
using System.Resources;
using System.Reflection;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml", "Get.Common")]
namespace Get.Common
{
    /// <summary>
    /// Source: http://www.wpftutorial.net/LocalizeMarkupExtension.html
    /// The localize markup extension returns a localized resource
    /// for a specified resource key. 
    /// </summary>
    [MarkupExtensionReturnType(typeof(string)), Localizability(LocalizationCategory.NeverLocalize)]
    public class LocalizeExtension : MarkupExtension
    {
        /// <summary>
        /// Caches the depending target object
        /// </summary>
        private DependencyObject _targetObject;

        /// <summary>
        /// Caches the depending target property
        /// </summary>
        private DependencyProperty _targetProperty;

        /// <summary>
        /// Caches the resolved default type converter
        /// </summary>
        private TypeConverter _typeConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        public LocalizeExtension()
        {
            LocalizationManager.CultureChanged += LocalizationManager_CultureChanged;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="LocalizeExtension"/> is reclaimed by garbage collection.
        /// </summary>
        ~LocalizeExtension()
        {
            LocalizationManager.CultureChanged -= LocalizationManager_CultureChanged;
        }

        /// <summary>
        /// Handles the CultureChanged event of the LocalizationManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LocalizationManager_CultureChanged(object sender, EventArgs e)
        {
            UpdateTarget();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        /// <param name="param">The key that specifies a localization </param>
        public LocalizeExtension(string key)
            : this()
        {
            Key = key;
        }

        /// <summary>
        /// Gets or sets the resource key.
        /// </summary>
        /// <value>The key.</value>
        [ConstructorArgument("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets a format string that is used to format the value
        /// </summary>
        /// <value>The format.</value>
        [ConstructorArgument("format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the default value, that is used, when the 
        /// key was not found or the localized value is null
        /// </summary>
        /// <value>The default value.</value>
        [ConstructorArgument("DefaultValue")]
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value>The converter.</value>
        [ConstructorArgument("Converter")]
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Updates the target.
        /// </summary>
        internal void UpdateTarget()
        {
            if (_targetObject != null && _targetProperty != null)
            {
                _targetObject.SetValue(_targetProperty, ProvideValueInternal());
            }
        }

        /// <summary>
        /// Returns the localized value
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Resolve the depending object and property
            if (_targetObject == null)
            {
                var targetHelper = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
                _targetObject = targetHelper.TargetObject as DependencyObject;
                _targetProperty = targetHelper.TargetProperty as DependencyProperty;
                _typeConverter = TypeDescriptor.GetConverter(_targetProperty.PropertyType);
            }

            return ProvideValueInternal();
        }

        /// <summary>
        /// Provides the value internal.
        /// </summary>
        /// <returns></returns>
        private object ProvideValueInternal()
        {
            // Get the localized value
            object value = LocalizationManager.GetValue(Key);

            // Automatically convert the type if a matching type converter is available
            if (value != null && _typeConverter != null && _typeConverter.CanConvertFrom(value.GetType()))
            {
                value = _typeConverter.ConvertFrom(value);
            }

            // If the value is null, use the fallback value if available
            if (value == null && DefaultValue != null)
            {
                value = DefaultValue;
            }

            // If no fallback value is available, return the key
            if (value == null)
            {
                if (_targetProperty != null && _targetProperty.PropertyType == typeof(string))
                {
                    // Return the key surrounded by question marks for string type properties
                    value = string.Concat("?", Key, "?");
                }
                else
                {
                    // Return the UnsetValue for all other types of dependency properties
                    return DependencyProperty.UnsetValue;
                }
            }

            if (Converter != null)
            {
                value = Converter.Convert(value, _targetProperty.PropertyType, null, CultureInfo.CurrentCulture);
            }

            // Format the value if a format string is provided and the type implements IFormattable
            if (value is IFormattable && Format != null)
            {
                ((IFormattable)value).ToString(Format, CultureInfo.CurrentCulture);
            }

            return value;
        }
    }
    public class LocalizationManager
    {
        /// <summary>
        /// Occurs when the culture changed.
        /// </summary>
        public static event EventHandler CultureChanged;

        /// <summary>
        /// List of registered localization resource providers
        /// </summary>
        public static ILocalizedResourceProvider LocalizationProvider { get; set; }

        /// <summary>
        /// Gets the supported cultures.
        /// </summary>
        /// <value>The supported cultures.</value>
        public static IList<CultureInfo> SupportedCultures { get; private set; }

        /// <summary>
        /// Gets and sets the currently selected culture
        /// </summary>
        public static CultureInfo CurrentCulture
        {
            get { return CultureInfo.CurrentUICulture; }
            set
            {
                Thread.CurrentThread.CurrentUICulture = value;

                if (CultureChanged != null)
                {
                    CultureChanged(typeof(LocalizationManager), EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a localized value for the specified resource key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static object GetValue(string key)
        {
            if (LocalizationProvider != null)
            {
                return LocalizationProvider.GetValue(key);
            }
            return null;
        }


        /// <summary>
        /// Initializes the <see cref="LocalizationManager"/> class.
        /// </summary>
        static LocalizationManager()
        {
            SupportedCultures = new List<CultureInfo>();
        }

    }
    public interface ILocalizedResourceProvider
    {
        /// <summary>
        /// Gets the localized value for the specified key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        object GetValue(string key);

    }
    public class ResourceFileProvider : ResourceManager, ILocalizedResourceProvider
    {
        /// <summary>
        /// Caches the current resource set
        /// </summary>
        private ResourceSet _resourceSet;

        /// <summary>
        /// Gets the localized value for the specified key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            try
            {
                if (_resourceSet != null)
                {
                    return _resourceSet.GetObject(key);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loads the resources.
        /// </summary>
        private void LoadResources()
        {
            ReleaseAllResources();
            _resourceSet = GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceFileProvider"/> class.
        /// </summary>
        /// <param name="baseName">Name of the base.</param>
        /// <param name="assembly">The assembly.</param>
        public ResourceFileProvider(string baseName, Assembly assembly)
            : base(baseName, assembly)
        {
            LoadResources();
            LocalizationManager.CultureChanged += (sender, e) => { LoadResources(); };
        }


    }
}
