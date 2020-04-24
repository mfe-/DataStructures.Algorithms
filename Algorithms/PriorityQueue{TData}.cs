using DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algorithms
{
    [DebuggerDisplay("Count = {Count}")]
    public class PriorityQueue<TData> : BinarySearchTree<TData>
    {
        private readonly Func<TData, IComparable> _funcKey;
        private readonly Dictionary<string, IComparable> keyValuePairs = new Dictionary<string, IComparable>();
        public PriorityQueue(Func<TData, IComparable> funcKey) : base()
        {
            _funcKey = funcKey;
        }
        public TData Exists(TData data, out IComparable key)
        {
            key = -1;
            if (keyValuePairs.ContainsKey(data.ToString()))
            {
                INodeLeafe<TData> nodeLeafe = GetNode(keyValuePairs[data.ToString()]);
                key = nodeLeafe.Key;
                return nodeLeafe.Value;

            }
            return default(TData);
        }
        public TData Exists(Func<TData, bool> findFunc, out IComparable key)
        {
            key = -1;
            INodeLeafe<TData> nodeLeafe = Find(findFunc);
            if (nodeLeafe != null)
            {
                key = nodeLeafe.Key;
                return nodeLeafe.Value;
            }
            return default(TData);
        }
        public override void Add(IComparable key, TData data)
        {
            INodeLeafe<TData> q = FuncNodeFactory?.Invoke(key, data);
            INodeLeafe<TData> r = null; //r will be predecessor of q
            INodeLeafe<TData> p = this.RootNode;
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
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
                    //40
                    //   40 p.Key
                    //       41 (p.U.Key)
                    if (p.Key.CompareTo(key) == 0 && p.Key.CompareTo(p?.U?.Key) == -1)
                    {
                        q.U = p.U;
                        break;
                    }
                    else
                    {
                        //same key - dont throw exception as the binarysearchtree would do
                        p = p.U;
                    }
                }
                else
                {
                    p = p.U;

                }
            }
            q.P = r;

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
            INodeLeafe<TData> nodeLeafe = GetMinimum();
            Remove(nodeLeafe.Key);
            return nodeLeafe.Value;
        }
        public IComparable Update(IComparable oldKey, TData data)
        {
            Remove(oldKey);
            var key = _funcKey(data);
            Add(key, data);
            return key;
        }

        public bool Any()
        {
            return Count != 0;
        }
    }
}
