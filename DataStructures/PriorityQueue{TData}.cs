using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataStructures
{
    /// <summary>
    /// PriorityQueue using <see cref="BinarySearchTree{TData}"/> with a list on each node to save duplicates
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class PriorityQueue<TData> : BinarySearchTree<TData>
    {
        [DebuggerDisplay("Key = {Key} Count = {Datas.Count}")]
        public class PriorityNode<TData1> : BNodeLeafe<TData1>
        {
            public PriorityNode(IComparable comparer, TData1 value) : base(comparer, value)
            {
                Datas = new HashSet<TData1>();
            }
            public ICollection<TData1> Datas { get; }
        }
        private readonly Func<TData, IComparable> _funcKey;
        public PriorityQueue(Func<TData, IComparable> funcKey) : base()
        {
            _funcKey = funcKey;
            FuncNodeFactory = new Func<IComparable, TData, INodeLeafe<TData>>(
                (key, data) => new PriorityNode<TData>(key, data));
        }
        public override void Add(IComparable key, TData data)
        {
            INodeLeafe<TData> q = FuncNodeFactory?.Invoke(key, data);
            INodeLeafe<TData> r = null; //r will be predecessor of q
            INodeLeafe<TData> p = this.RootNode;
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            bool update = false;
            while (p != null)
            {
                (p as BNodeLeafe<TData>).AmountofNode += 1;
                r = p;
                if (q.Key.CompareTo(p.Key) == -1)
                {
                    p = p.V;
                }
                else if (p.Key.CompareTo(key) == 0)
                {
                    //same key - we dont need a new node
                    q = p;
                    update = true;
                    break;
                }
                else
                {
                    p = p.U; 
                }
            }

            (q as PriorityNode<TData>).Datas.Add(data);
            if (update) return;

            q.P = r;
            q.V = null;
            q.U = null;

            if (r == null)
            {
                this.RootNode = q;
            }
            else
            {
                if (q.Key.CompareTo(r.Key) == -1)
                {
                    r.V = q;
                }
                else
                {
                    r.U = q;
                }
            }
            //increase size of tree;
            Count = Count + 1;
        }
        public IComparable Enqueue(TData data)
        {
            var key = _funcKey(data);
            Add(key, data);
            return key;
        }
        public TData Dequeue()
        {
            PriorityNode<TData> nodeLeafe = (PriorityNode<TData>)GetMinimum();
            TData data = nodeLeafe.Datas.FirstOrDefault();
            nodeLeafe.Datas.Remove(data);
            if (nodeLeafe.Datas.Count == 0)
            {
                Remove(nodeLeafe.Key);
            }
            return data;
        }

        public bool Any()
        {
            return Count != 0;
        }
    }
}
