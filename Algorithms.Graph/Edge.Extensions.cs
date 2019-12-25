using DataStructures;

namespace Algorithms.Graph
{
    /// <summary>
    /// Provides a set of static methods for working with Edge objects.
    /// </summary>
    public static class EdgeExtensions
    {
        /// <summary>
        /// Returns a value indicating whether this instance is equal to the edge.
        /// </summary>
        /// <param name="edge">The edge to compare to this instance.</param>
        /// <param name="graphIsdirected">If the parameter is false transported edges will be handled as equal</param>
        /// <returns>True if the instance and the overgiven edge are euqa; otherwiese, false.</returns>
        public static bool Equals(this IEdge e, IEdge edge, bool graphIsdirected = true)
        {
            //(use == operator instead of Equals for directed graphs,
            //as the overriden equals of the edge implementeation returns true for transposed edges)
            if (!graphIsdirected)
            {
                //transposed edges uses the hash of the internal guids of the vertices
                if (e != null) return e.Equals(edge);
                return false;
            }
            else
            {
                return e == edge;
            }
        }
    }
}
