using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Markup;
using System.Windows.Interop;
using System.Windows;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml", "Get.Common")]
namespace Get.Common
{
    public static class Extension
    {
        /// <summary>
        /// Zum Rekursiven durchlaufen von Objekten
        /// http://mutable.net/blog/archive/2008/05/23/using-linq-to-objects-for-recursion.aspx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="DescendBy"></param>
        /// <returns></returns>
        public static IEnumerable<T> Descendants<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> DescendBy)
        {
            foreach (T value in source)
            {
                yield return value;

                foreach (T child in DescendBy(value).Descendants<T>(DescendBy))
                {
                    yield return child;
                }
            }
        }

        public static IEnumerable<T> EnumToList<T>(this IEnumerable<T> t)
        {
            Type enumType = typeof(T);
            if (enumType.BaseType.Equals(typeof(Enum)))
                return new List<T>();
            Array enumValArray = Enum.GetValues(enumType);
            IList<T> enumValList = new List<T>();
            foreach (int val in enumValArray)
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            return enumValList;
        }
        /// <summary>
        /// Erzeugt aus einer IEnumerable eine ObservableCollection.
        /// </summary>
        /// <typeparam name="T">Type des IEnumerable.</typeparam>
        /// <param name="collection">IEnumerable die in eine ObservableCollection umgewandelt werden soll.</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            ObservableCollection<T> observableCollection = new ObservableCollection<T>();
            if (collection.Count().Equals(0)) return observableCollection;
            foreach (var x in collection)
            {
                observableCollection.Add(x);
            }
            return observableCollection;
        }
        /// <summary>
        /// Zur Verwendung von NotifyPropertyChanged(this.GetMemberName(x=>x.Propertiename));
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="instance"></param>
        /// <param name="projection"></param>
        /// <returns>Gibt den Namen der übergebenen Property zurück.</returns>
        public static string GetMemberName<TEntity, TProperty>(this TEntity instance, Expression<Func<TEntity, TProperty>> projection)
        {
            return ((MemberExpression)projection.Body).Member.Name;
        }
        /// <summary>
        /// Gibt den Variabelname als String zurück
        /// http://abdullin.com/journal/2008/12/13/how-to-find-out-variable-or-parameter-name-in-c.html
        /// </summary>
        /// <typeparam name="T">Typ der Variable</typeparam>
        /// <param name="obj">Variabel von der der Name als string zurück gegeben werden soll.</param>
        /// <param name="expr"></param>
        /// <returns>Den Namen der Variable</returns>
        public static string GetVariabelName<T>(this T obj, Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            return body.Member.Name;
        }

        /// <summary>
        /// Gibt den Handle des übergebenen Window zurück
        /// </summary>
        /// <param name="wnd">Window von dem der Handle ausgelesen werden soll.</param>
        /// <returns>Gibt den Handle des Window zurück</returns>
        public static IntPtr GetHandle(this Window wnd)
        {
            return new WindowInteropHelper(wnd).Handle;
        }

        /// <summary>
        /// Zieht den übergebenen String ab. (Mit der Replace Funktion)
        /// </summary>
        /// <param name="pfirst">String an dem die Extension angewandt wird.</param>
        /// <param name="pStringToDelete">String der abgezogen werden soll.</param>
        /// <returns>Gibt den neuen string zurück</returns>
        public static string Minus(this string pfirst, string pStringToDelete)
        {
            return pStringToDelete.Replace(pfirst, string.Empty);
        }


    }

}
