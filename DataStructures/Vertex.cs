using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System;
using System.Diagnostics;
using System.Linq;

namespace DataStructures
{
    [DebuggerDisplay("Vertex={Weighted},GUID={_Guid}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Vertex")]
    public class Vertex<TData> : IVertex<TData>, INotifyPropertyChanged
    {
        protected ObservableCollection<IEdge> _Edges = new ObservableCollection<IEdge>();
        [DataMember(Name = "Guid", Order = 3, IsRequired = true)]
        protected Guid _Guid;
        protected int _weighted;

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
            _weighted = pweighted;
        }
        [IgnoreDataMember]
        public TData Value { get; set; }
        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        [DataMember(Name = "Weighted", Order = 1, IsRequired = true)]
        public int Weighted { get { return _weighted; } set { _weighted = value; NotifyPropertyChanged("Weighted"); } }

        /// <summary>
        /// Gets or sets the list of edges which connects the vertex neighbours
        /// </summary>
        [DataMember(Name = "Edges", Order = 2, IsRequired = true)]
        public ObservableCollection<IEdge> Edges { get { return _Edges; } set { _Edges = value; NotifyPropertyChanged("Edges"); } }

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


        /// <summary>
        /// Creates a un/directed edge to the overgiven Vertex
        /// </summary>
        /// <param name="pu">Vertex to connect</param>
        /// <param name="pweighted">Weighted of the Edge</param>
        /// <param name="directed">False if the edge should be undirected (2 edges); othwise directed (1 edge)</param>
        public virtual IEdge AddEdge(IVertex pu, int pweighted = 0, bool directed = true)
        {
            Edge<TData> e1 = new Edge<TData>(this, pu, pweighted);
            _Edges.Add(e1);
            if (directed == false)
            {
                pu.AddEdge(this, pweighted, true);
            }
            return _Edges.Last();
        }

        public virtual void RemoveEdge(IVertex pu)
        {
            RemoveEdge(pu, true);
        }
        public virtual void RemoveEdge(IVertex pu, bool directed)
        {
            IEdge edge = this.Edges.FirstOrDefault(a => a.U.Equals(this) && a.V.Equals(pu));

            if (directed.Equals(false))
            {
                IEdge edged = edge.V.Edges.FirstOrDefault(a => a.U.Equals(edge.V) && a.V.Equals(this) && a.Weighted.Equals(edge.Weighted));

                edge.V.Edges.Remove(edged);
            }
            this.Edges.Remove(edge);


        }

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
            if (!obj.GetType().Equals(typeof(Vertex<TData>))) return false;

            return this._Guid.Equals((obj as Vertex<TData>)._Guid);
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
        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        int Weighted { get; set; }
        ObservableCollection<IEdge> Edges { get; set; }
        /// <summary>
        /// Creates a un/directed edge to the overgiven Vertex
        /// </summary>
        /// <param name="pu">Vertex to connect</param>
        /// <param name="pweighted">Weighted of the Edge</param>
        /// <param name="directed">False if the edge should be undirected (2 edges); othwise directed (1 edge)</param>
        IEdge AddEdge(IVertex pu, int pweighted = 0, bool directed = true);
        void RemoveEdge(IVertex pu);
        void RemoveEdge(IVertex pu, bool directed);
        /// <summary>
        /// Amount of neighbours
        /// </summary>
        int Size { get; }
    }
    public interface IVertex<out TData> : IVertex, IData<TData>
    {
    }
}