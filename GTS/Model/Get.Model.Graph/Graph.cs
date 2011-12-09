using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Get.Model.Graph
{
    [DataContract(Name = "Edge", Namespace = "http://Get.Model.Graph")]
    public class Graph : IExtensibleDataObject
    {
        public ObservableCollection<Vertex> _Vertices = new ObservableCollection<Vertex>();

        public Graph()
        {
            _Vertices.LongCount();
        }
        public void addVertec(Vertex pVertice)
        {
            _Vertices.Add(pVertice);
        }
        [DataMember()]
        public ObservableCollection<Vertex> Vertices
        {
            get
            {
                return _Vertices;
            }
        }
        #region IExtensibleDataObject
        protected ExtensionDataObject extensionData_Value;
        public ExtensionDataObject ExtensionData
        {
            get
            {
                return extensionData_Value;
            }
            set
            {
                extensionData_Value = value;
            }
        }
        #endregion

    }
    public static class GraphExtension
    {
        //private static List<Vertex> counted;
        //public static int CountV<Vertex>(this IEnumerable<Vertex> source)
        //{
        //    return 1;
        //}
    }

}
