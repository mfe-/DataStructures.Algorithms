using System.ComponentModel;
using System.Runtime.Serialization;

namespace Get.Model.Graph
{
    [DataContract(Namespace = "http://schemas.get.com/Graph/Edges")]
    public class Edge : INotifyPropertyChanged
    {
        #region Members
        protected Vertex u;
        protected Vertex v;

        protected int weighted;
        #endregion

        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="pu">Vertex of the Edge</param>
        /// <param name="pv">Vertex of the Edge</param>
        public Edge(Vertex pu, Vertex pv)
        {
            u = pu;
            v = pv;
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="pu">Vertex of the Edge</param>
        /// <param name="pv">Vertex of the Edge</param>
        /// <param name="pweighted">Sets the Weighted of the Edge</param>
        public Edge(Vertex pu, Vertex pv, int pweighted)
        {
            u = pu;
            v = pv;
            weighted = pweighted;
        }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "U", Order = 2, IsRequired = true)]
        public Vertex U { get { return u; } set { u = value; NotifyPropertyChanged("U"); } }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "V", Order = 3, IsRequired = true)]
        public Vertex V { get { return v; } set { v = value; NotifyPropertyChanged("V"); } }
        /// <summary>
        /// Gets or sets the Weighted of the Edge
        /// </summary>
        public int Weighted { get { return weighted; } set { weighted = value; NotifyPropertyChanged("Weighted"); } }
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
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
