using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System;
using System.Diagnostics;
using System.Linq;

namespace DataStructures
{
    [DebuggerDisplay("Vertex = {Weighted},Size={Size}, GUID = {_Guid}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Vertex")]
    public class Vertex : IVertex, INotifyPropertyChanged
    {
        #region Members
        protected ObservableCollection<Edge> _Edges = new ObservableCollection<Edge>();
        [DataMember(Name = "Guid", Order = 3, IsRequired = true)]
        protected Guid _Guid;

        protected int weighted;
        #endregion

        /// <summary>
        /// Initializes a new instance of the Vertex class.
        /// </summary>
        public Vertex() { _Guid = Guid.NewGuid(); }

        /// <summary>
        /// Initializes a new instance of the Vertex class that contains the specified weighted.
        /// </summary>
        /// <param name="pweighted"></param>
        public Vertex(int pweighted)
            : this()
        {
            weighted = pweighted;
        }
        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        [DataMember(Name = "Weighted", Order = 1, IsRequired = true)]
        public int Weighted { get { return weighted; } set { weighted = value; NotifyPropertyChanged("Weighted"); } }

        /// <summary>
        /// Gets or sets the list of edges which connects the vertex neighbours
        /// </summary>
        [DataMember(Name = "Edges", Order = 2, IsRequired = true)]
        public ObservableCollection<Edge> Edges { get { return _Edges; } set { _Edges = value; NotifyPropertyChanged("Edges"); } }

        /// <summary>
        /// Amount of neighbours
        /// </summary>
        public int Size
        {
            get
            {
                return Edges.Count; //Knotengrad
            }
        }

        #region addEdge
        /// <summary>
        /// Adds an directed edge to overgiven vertex
        /// </summary>
        /// <param name="pu">The vertex which should be added to the instance</param>
        public virtual Edge addEdge(Vertex pu)
        {
            addEdge(pu, 0, false);
            return _Edges.Last();
        }
        /// <summary>
        /// Creates an un/directed edge to the overgiven Vertex
        /// </summary>
        /// <param name="pu">Vertex to connect</param>
        /// <param name="pweighted">Weighted of the Edge</param>
        /// <param name="undirected">True if the edge should be undirected (2 edges); othwise directed (1 edge)</param>
        public virtual void addEdge(Vertex pu, int pweighted, bool undirected)
        {

            Edge e1 = new Edge(this, pu, pweighted);
            _Edges.Add(e1);
            if (undirected == true)
            {
                pu.addEdge(this, pweighted);
            }

        }

        /// <summary>
        /// Adds a directed edge to the overgiven vertex
        /// </summary>
        /// <param name="pu">The vertex which should be added to the instance</param>
        /// <param name="pweighted">Weighted of the added vertex</param>
        public virtual Edge addEdge(Vertex pu, int pweighted)
        {
            addEdge(pu, pweighted, false);
            return _Edges.Last();
        }
        #endregion

        #region removeEdge
        public virtual void removeEdge(Vertex pu)
        {
            removeEdge(pu, true);
        }
        public virtual void removeEdge(Vertex pu,bool directed)
        {
            Edge edge = this.Edges.Where(a => a.U.Equals(this) && a.V.Equals(pu)).FirstOrDefault<Edge>();

            if (directed.Equals(false))
            {
                Edge edged = edge.V.Edges.Where(a => a.U.Equals(edge.V) && a.V.Equals(this) && a.Weighted.Equals(edge.Weighted)).FirstOrDefault<Edge>();

                edge.V.Edges.Remove(edged);
            }
            this.Edges.Remove(edge);


        }
        #endregion 

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return base.ToString() + string.Empty + Weighted;
        }
        /// <summary>
        /// Determines with the guid whether the specified Object is equal to the current Object.
        /// http://msdn.microsoft.com/en-us/library/bsc2ak47.aspx
        /// </summary>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals(typeof(Vertex))) return false;

            return this._Guid.Equals((obj as Vertex)._Guid);
        }
        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return _Guid.GetHashCode();
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
    public interface IVertex
    {
        int Weighted { get; set; }
        ObservableCollection<Edge> Edges { get; set; }
        Edge addEdge(Vertex pu);
        Edge addEdge(Vertex pu, int pweighted);
        void addEdge(Vertex pu, int pweighted, bool undirected);
        void removeEdge(Vertex pu);
        void removeEdge(Vertex pu,bool directed);

    }
}
