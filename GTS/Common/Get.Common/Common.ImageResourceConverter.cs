using System;
using System.Linq;
using System.Windows.Markup;
using System.Windows;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml", "Get.Common")]
namespace Get.Common
{
    /// <summary>
    /// Mit dieser MarkupExtension kann man Bilder aus einer Resource-Datei (.resx) laden und direkt im XAML übergeben.
    /// Verwendung wie folgt
    /// <Image Source="{GetCommon:ImageResourceConverter Get.Common.Resource.Image1}" Width="20"/>
    /// <Image Source="{GetCommon:ImageResourceConverter Get.Demo.Resource.Image1@Get.Demo.exe}" Width="20"/>
    /// Syntax: Namespace bzw. ProjektName.ResourceName.Name des Bildes auf die man zugreifen will.
    /// Gibt man hinter dem Namespace.ResourceName.BildName kein @ an nimmt die MarkupExtension an, dass die Resource sich in der aktuellen Instanz befindet.
    /// Syntax: Namespace.ResourceName.BildName@DLLName in der sich die Resource befindet
    /// Die Dll wird im Verzeichnis Environment.CurrentDirectory gesucht.
    /// </summary>
    [MarkupExtensionReturnType(typeof(ImageSource)), Localizability(LocalizationCategory.NeverLocalize)]
    public class ImageResourceConverterExtension : MarkupExtension
    {
        #region Members
        /// <summary>
        /// Seperator zwischen Namespace und Resource
        /// </summary>
        private const char _point = '.';

        /// <summary>
        /// Seperator zwischen ProjektName.ResourceName.Name und DLLName
        /// </summary>
        private const char _ExternAssemblySpliter = '@';
        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor - Initialisiert die Properties
        /// </summary>
        public ImageResourceConverterExtension()
        {
            ImageName = string.Empty;
            BaseName = string.Empty;
        }
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="pkey">Key der bei der MarkupExtension angegeben wurde. Z.B. "Get.Demo.Resource.Image1@Get.Demo.exe"</param>
        public ImageResourceConverterExtension(string pkey)
            : this()
        {
            Key = pkey;

            //Prüfen ob die Resource-Datei aus einer fremden Assembly geladen werden soll
            if (!pkey.Contains(_ExternAssemblySpliter))
            {
                //Properties ImageName und BaseName setzen
                SetImageNameAndBaseName(pkey);
            }
            else
            {
                //Fremde Assembly laden - Dateiname extrahieren
                string filename = pkey.Split(_ExternAssemblySpliter).Last();
                FileInfo fileInfo = new FileInfo(Environment.CurrentDirectory + "\\" + filename);

                if (!fileInfo.Exists)
                    throw new FileNotFoundException();

                //Assembly laden
                Assembly assembly = Assembly.LoadFrom(fileInfo.ToString());
                ResourceAssembly = assembly;

                SetImageNameAndBaseName(pkey.Replace(_ExternAssemblySpliter.ToString() + filename, string.Empty));
            }

        }
        #endregion

        #region Funktionen
        /// <summary>
        /// Extrahiert die Werte aus dem übergeben Key und weist sie den jeweiligen Properties zu.
        /// </summary>
        /// <param name="pkey">MarkupExtension Key Z.B. "Get.Demo.Resource.Image1@Get.Demo.exe"</param>
        private void SetImageNameAndBaseName(string pkey)
        {
            //Letzter Value ist der BildName
            ImageName = pkey.Split(_point).Last();
            //Der Rest besteht aus Namespace.ResourceName
            BaseName = pkey.Replace(_point.ToString() + ImageName, string.Empty);
        }
        /// <summary>
        /// Gibt ein ImageSource Objekt zurück an der die MarkupExtension gesetzt wurde.
        /// </summary>
        /// <param name="pserviceProvider">Objekt, das Dienste für die Markuperweiterung bereitstellen kann.</param>
        /// <returns>ImageSource Objekt</returns>
        public override object ProvideValue(IServiceProvider pserviceProvider)
        {
            //Bild aus Resource laden
            if (ResourceAssembly == null)
                return GetBitmapImageFromResource(BaseName, ImageName, this.GetType().Assembly);
            else
                return GetBitmapImageFromResource(BaseName, ImageName, ResourceAssembly);
        }
        /// <summary>
        /// Ladet mithilfe der übergebenen Informationen ein Bild aus der Resource (.resx) und konvertiert es in ein ImageSource Objekt
        /// </summary>
        /// <param name="pBaseName"></param>
        /// <param name="pImageName"></param>
        /// <param name="pAssembly"></param>
        /// <returns></returns>
        public static ImageSource GetBitmapImageFromResource(string pBaseName, string pImageName, Assembly pAssembly)
        {
            ResourceManager resourceManager = new ResourceManager(pBaseName, pAssembly);

            //System.Drawing.Bitmap aus Resource holen
            object image = resourceManager.GetObject(pImageName);
            if (image == null) return null;

            if (!image.GetType().Equals(typeof(Bitmap))) return null;

            Bitmap bitmap = image as Bitmap;

            //Bitmap in ImageSource konvertieren
            BitmapImage bitmapImage = new BitmapImage();
            MemoryStream stream = new MemoryStream();

            bitmap.Save(stream, ImageFormat.Png);

            //ImageSource aus Stream holen http://cornucopia30.blogspot.com/2007/08/wpf-point-image-to-embedded-resource.html
            PngBitmapDecoder bitmapDecoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            ImageSource imageSource = bitmapDecoder.Frames[0];
            return imageSource;
        }
        #endregion Funktionen

        #region Properties

        /// <summary>
        /// Gets or sets the resource key.
        /// </summary>
        /// <value>The key.</value>
        [ConstructorArgument("pkey")]
        public string Key { get; private set; }

        /// <summary>
        /// Ruft den Namespace bzw. Projekt-Namen + Resourcename ab oder legt diesen fest.
        /// </summary>
        public string BaseName { get; private set; }

        /// <summary>
        /// Ruft den Wert des Bildnamens ab oder legt diesen fest.
        /// </summary>
        public string ImageName { get; private set; }

        /// <summary>
        /// Ruft die Externe Assembly ab oder legt diese fest. Diese Propertie ist nur gesetzt wenn man in der MarkupExtension das @ mit dem DLLNamen festgelegt hat.
        /// </summary>
        public Assembly ResourceAssembly { get; private set; }

        #endregion
    }
}
