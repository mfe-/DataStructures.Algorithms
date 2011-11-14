
using System.Xml.Serialization;
using System.ComponentModel;
using System;

namespace Get.Model.Graph
{
    [XmlRoot("Edge")]
    public class Edge : INotifyPropertyChanged
    {
        protected Vertex u;
        protected Vertex v;

        protected int weighted;

        public Edge() { }

        public Edge(Vertex pu, Vertex pv)
        {
            u = pu;
            v = pv;
        }

        public Edge(Vertex pu, Vertex pv, int pweighted)
        {
            u = pu;
            v = pv;
            weighted = pweighted;
        }
        [XmlIgnore] //wegen Zirkuläre Verweise. Wenn Objekte auf sich selbst verweisen, resultiert die Serialisierung durch Replizierung auch dann in einer Endlosschleife, wenn die Verweise über andere Objekte führen.
        public Vertex U { get { return u; } set { u = value; NotifyPropertyChanged("U"); } }
        [XmlElement("V")]
        public Vertex V { get { return v; } set { v = value; NotifyPropertyChanged("V"); } }
        [XmlElement("Weighted")]
        public int Weighted { get { return weighted; } set { weighted = value; NotifyPropertyChanged("Weighted"); } }

        public override string ToString()
        {
            return base.ToString() + " " + U.ToString() + " ->" + V.ToString();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify using String property name
        /// </summary>
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
