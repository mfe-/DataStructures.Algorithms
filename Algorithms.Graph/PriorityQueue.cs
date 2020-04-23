using DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Algorithms.Graph.GraphExtensions;

namespace Algorithms.Graph
{
    public class PriorityQueue : IEnumerable
    {
        Dictionary<IVertex, AEdge> list = new Dictionary<IVertex, AEdge>();
        public PriorityQueue()
        {
        }
        public AEdge Exists(IVertex vertex)
        {
            if (list.ContainsKey(vertex))
            {
                return list[vertex];
            }
            return new AEdge();
        }
        public void Enqueue(AEdge item)
        {
            list.Add(item.V, item);
        }
        public AEdge Dequeue()
        {
            var dequeue = list.OrderBy(a => a.Value.F).FirstOrDefault();
            list.Remove(dequeue.Key);
            return dequeue.Value;
        }
        public void Update(AEdge item)
        {
            AEdge aEdge = new AEdge();
            var key = list.Keys.FirstOrDefault(a => a == item.V);
            list.Remove(key);

            aEdge.F = item.F;
            aEdge.U = item.U;
            aEdge.V = item.V;
            aEdge.Weighted = item.Weighted;
            list.Add(aEdge.V, aEdge);
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public bool Any()
        {
            return list.Any();
        }
    }
}
