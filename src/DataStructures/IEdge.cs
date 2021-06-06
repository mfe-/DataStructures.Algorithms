namespace DataStructures
{
    /// <summary>
    /// An edge that connects vertices <see cref="IEdge.U"/> and <see cref="IEdge.V"/>
    /// </summary>
    public interface IEdge
    {
        /// <summary>
        /// Get or sets the vertex
        /// </summary>
        IVertex U { get; set; }
        /// <summary>
        /// Get or sets the vertex
        /// </summary>
        IVertex V { get; set; }
        /// <summary>
        /// Get or sets the weight of the edge
        /// </summary>
        double Weighted { get; set; }
    }
}
