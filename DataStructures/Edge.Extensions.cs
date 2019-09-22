﻿using System.Collections.Generic;


namespace DataStructures
{
    /// <summary>
    /// Provides a set of static methods for working with Edge objects.
    /// </summary>
    public static class EdgeExtensions
    {
        public class EdgeComparer : IEqualityComparer<IEdge>
        {
            #region IEqualityComparer
            /// <summary>
            /// Returns a value indicating whether this instance is equal to the edge. Transposed edges will be handled as equal edge.
            /// http://msdn.microsoft.com/en-us/library/bb338049(v=vs.100).aspx
            /// </summary>
            /// <param name="x">edge x</param>
            /// <param name="y">edge y</param>
            /// <returns></returns>
            public bool Equals(IEdge e1, IEdge e2)
            {
                //edge are equal
                if (e1.Equals(e2) && e2.Equals(e1)) return true;

                //edges are not equal but transposed (e1: v1->v2 e2: v2->v1 )
                if ((e1.Equals(e2) && e2.Equals(e1)).Equals(false) &&
                    (EdgeExtensions.Equals(e1, e2, true) && EdgeExtensions.Equals(e1, e2, true)).Equals(true)) return true;

                //diffrent edges
                return false;
            }

            public int GetHashCode(IEdge obj)
            {
                return obj.GetHashCode();
            }
            #endregion
        }
        /// <summary>
        /// Returns a value indicating whether this instance is equal to the edge.
        /// </summary>
        /// <param name="edge">The edge to compare to this instance.</param>
        /// <param name="permute">If the parameter is true transported edges will be handled as equal</param>
        /// <returns>True if the instance and the overgiven edge are euqa; otherwiese, false.</returns>
        public static bool Equals(this IEdge e, IEdge edge, bool permute)
        {
            if (permute)
            {
                if (!e.U.Equals(edge.V)) return false;
                if (!e.V.Equals(edge.U)) return false;
                if (!e.Weighted.Equals(edge.Weighted)) return false;
                if (!e.GetHashCode().Equals(edge.GetHashCode())) return false;
                return true;
            }
            else
            {
                if (!e.U.Equals(edge.U)) return false;
                if (!e.V.Equals(edge.V)) return false;
                if (!e.Weighted.Equals(edge.Weighted)) return false;
                if (!e.GetHashCode().Equals(edge.GetHashCode())) return false;
                return true;
            }
        }
    }
}
