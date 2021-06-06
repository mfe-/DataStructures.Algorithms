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
        /// <summary>
        /// Node which is used for the <see cref="PriorityQueue{TData}"/>
        /// </summary>
        /// <typeparam name="TData1"></typeparam>
        [DebuggerDisplay("Key = {Key} Count = {Datas.Count}")]
        public class PriorityNode<TData1> : BNode<TData1>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PriorityNode{TData1}"/> class.
            /// </summary>
            public PriorityNode(IComparable comparer, TData1 value) : base(comparer, value)
            {
                Datas = new HashSet<TData1>();
            }
            /// <inheritdoc/>
            public ICollection<TData1> Datas { get; }
        }
        private readonly Func<TData, IComparable> _funcKey;
        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue{TData}"/> class.
        /// </summary>
        /// <param name="funcKey">The function which is used to retriev the key of a node</param>
        public PriorityQueue(Func<TData, IComparable> funcKey) : base()
        {
            _funcKey = funcKey;
            FuncNodeFactory = new Func<IComparable, TData, INodeTree<TData>>(
                (key, data) => new PriorityNode<TData>(key, data));
        }
        /// <inheritdoc/>
        public override void Add(IComparable key, TData data)
        {
            INodeTree<TData> q = FuncNodeFactory.Invoke(key, data);
            INodeTree<TData>? r = null; //r will be predecessor of q
            INodeTree<TData>? p = this.RootNode;
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            bool update = false;
            while (p != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                (p as BNode<TData>).AmountofNode += 1;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            (q as PriorityNode<TData>).Datas.Add(data);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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
        /// <summary>
        /// Add <paramref name="data"/> to the queue
        /// </summary>
        /// <param name="data">The data which should be queued</param>
        /// <returns>The retrieved key of the overgiven <paramref name="data"/> after it was added to the queue.</returns>
        public IComparable Enqueue(TData data)
        {
            var key = _funcKey(data);
            Add(key, data);
            return key;
        }
        /// <summary>
        /// Gets the data with the minimum key and removes it from the <see cref="PriorityQueue{TData}"/>
        /// </summary>
        /// <returns></returns>
        public TData Dequeue()
        {
            var minimum = GetMinimum();
            if (minimum is PriorityNode<TData> nodeLeafe)
            {
                TData? data = nodeLeafe.Datas.FirstOrDefault();
                if (data != null)
                {
                    nodeLeafe.Datas.Remove(data);
                    if (nodeLeafe.Datas.Count == 0)
                    {
                        Remove(nodeLeafe.Key);
                    }
                    return data;
                }
                throw new InvalidOperationException($"{nameof(PriorityQueue<TData>)} is empty!");
            }
            else
            {
                throw new InvalidCastException($"We expected a {nameof(PriorityNode<TData>)} when calling ${nameof(GetMinimum)}");
            }

        }
        /// <summary>
        /// Returns if the <see cref="PriorityQueue{TData}"/> contains any data.
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return Count != 0;
        }
    }
}
