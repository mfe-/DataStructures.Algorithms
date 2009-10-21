using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.IO;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml", "Get.Common")]
namespace Get.Common
{
    public static class Methods
    {
        public static string GetRelativePath(string pFirstPath, string pSecondPath)
        {
            string relativpath = pFirstPath.Minus(pSecondPath);
            return relativpath;
        }
    }
}      
