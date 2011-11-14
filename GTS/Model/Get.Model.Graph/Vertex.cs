using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Get.Model.Graph
{
    [XmlRoot("Vertex")]
    public class Vertex : INotifyPropertyChanged
    {
        protected ObservableCollection<Edge> _Edges = new ObservableCollection<Edge>();

        protected int weighted;

        public Vertex() { }

        public Vertex(int pweighted)
        {
            weighted = pweighted;
        }
        public void addEge(Vertex pu)
        {
            _Edges.Add(new Edge(this, pu));
        }
        public void addEge(Vertex pu, int pweighted)
        {
            _Edges.Add(new Edge(this, pu, pweighted));
        }
        [XmlElement("Weighted")]
        public int Weighted { get { return weighted; } set { weighted = value; NotifyPropertyChanged("Weighted"); } }
        
        [XmlArray("Edges")]
        [XmlArrayItem("Edge", typeof(Edge))]
        public ObservableCollection<Edge> Edges { get { return _Edges; } set { _Edges = value; NotifyPropertyChanged("Edges"); } }

        public override string ToString()
        {
            return base.ToString() + Weighted;
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
