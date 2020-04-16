using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms
{
    /// <summary>
    /// Binary search tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinarySearchTree<TData> : AbstractTree<TData>
    {
        /// <summary>
        /// Represents the method that will handle the get node functionality.
        /// </summary>
        /// <param name="value">The value which we are looking for.</param>
        /// <param name="RootNode">The RootNode node of the tree to start the traversing.</param>
        /// <returns>The node which contains the overgiven value.</returns>
        public delegate INodeLeafe<TData> GetNodeDelegate(IComparable key, INodeLeafe<TData> RootNode);

        /// <summary>
        /// Points to the getNodeHandler
        /// </summary>
        protected GetNodeDelegate getNodeHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySearchTree{T}"/> class.
        /// </summary>
        public BinarySearchTree() : base()
        {
            FuncNodeFactory = new Func<IComparable, TData, INodeLeafe<TData>>((key, data) => new NodeLeafe<TData>(key, data));
            this.getNodeHandler = new GetNodeDelegate(this.GetNodePrivate);
        }
        /// <summary>
        /// Initializes a new instance of the  <see cref="BinarySearchTree{T}"/> class.
        /// </summary>
        /// <param name="getNode">
        /// The delegate which implements the function GetNode(T value, INodeLeafe<TData> RootNode). See the corresponding<see cref="BinarySearchTree.GetNodeDelegate">delegate
        /// documentation</see>  for more details.
        /// </param>
        public BinarySearchTree(GetNodeDelegate getNode) : this()
        {
            this.getNodeHandler = getNode;
        }

        /// <summary>
        /// Gets the node with the correspondening value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual INodeLeafe<TData> GetNode(IComparable key)
        {
            return this.GetNodePrivate(key, this.RootNode);
        }
        protected virtual INodeLeafe<TData> GetNodePrivate(IComparable key, INodeLeafe<TData> RootNode)
        {
            INodeLeafe<TData> p = RootNode;
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            while (p != null)
            {
                if (key.CompareTo(p.Key) == -1)
                {
                    p = p.V;
                }
                else if (p.Key.CompareTo(key) == 0)
                {
                    return p;
                }
                else
                {
                    p = p.U;
                }
            }

            return null;
        }
        /// <summary>
        /// Gets a value that indicates whether the overgiven value exists in the tree
        /// </summary>
        /// <param name="key"><The value to seek in the tree/param>
        /// <returns></returns>
        public bool ContainsKey(IComparable key)
        {
            return this.GetNode(key) != null;
        }
        /// <summary>
        /// Gets the height of the tree
        /// </summary>
        public int Height
        {
            get
            {
                return GetHeight(this.RootNode);
            }
        }
        /// <summary>
        /// Gets the height of the tree starting from the overgiven node
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The height from the node</returns>
        protected virtual int GetHeight(INodeLeafe<TData> node)
        {
            if (node == null)
            {
                return -1;
            }
            return 1 + (Math.Max(GetHeight(node.V), GetHeight(node.U)));
        }
        /// <summary>
        /// Clear the nodes of the tree
        /// </summary>
        public void Clear()
        {
            this.RootNode = null;
        }
        /// <summary>
        /// Add a new value to the tree
        /// </summary>
        /// <param name="data">The value to add</param>
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
                r = p;
                if (q.Key.CompareTo(p.Key) == -1)
                {
                    p = p.V;
                }
                else if (p.Key.CompareTo(key) == 0)
                {
                    return; //if key already exists
                }
                else
                {
                    p = p.U; //same key add to right
                }
            }
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
            Length = Length + 1;
            //node.n = 1 + 
            //node.AmountofNode = size(node.Lef) + size(node.U)+1; (size.node) gibt node.amountnode zurück
        }
        /// <summary>
        /// The value to remove
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(IComparable key)
        {
            if (Empty)
            {
                return;
            }
            INodeLeafe<TData> r = null;
            INodeLeafe<TData> RootNode = this.RootNode;
            INodeLeafe<TData> q = GetNode(key);
            INodeLeafe<TData> p = null;

            if (q.V == null || q.U == null)
            {   //q has max 1 Successor --> will be removed
                r = q;
            }
            else
            {
                //q got two Successor -> will be replaced with successor, the other wil be removed                r = Successor(q);
                //put the data from r to q (q.Value = r.Value;)
                //q = FuncNodeFactory.Invoke(r.Value, q.P, q.V, q.U);
                var temp = FuncNodeFactory.Invoke(r.Key, r.Value);
                temp.P = q.P;
                temp.V = q.V;
                temp.U = q.U;
                q = temp;
                if (q == null)
                { }
            }
            //let p reference the child on r (p=null, if r has no child)
            if (r.V != null)
            {
                p = r.V;
            }
            else
            {
                p = r.U;
            }
            if (p != null)
            {
                p.P = r.P;
                //create new reference from p to new predecessor (the predecessor of r)
            }
            //put p instead of r onto the tree
            if (r.P == null)
            {
                //r was root: new root is p
                this.RootNode = p;
            }
            else
            {
                //put p on the correct side of predecessor of r
                if (r == r.P.V)
                {
                    r.P.V = p; //p will be left predecessor
                }
                else
                {
                    r.P.U = p;
                }
            }
            r = null;

            Length = Length - 1;
            //TODO Save amount of subtree in node
        }
        /// <summary>
        /// Returns the successor from the overgiven node
        /// </summary>
        /// <param name="p">The node from which we should get the successor</param>
        /// <returns>The successor of the overgiven node</returns>
        public virtual INodeLeafe<TData> Successor(INodeLeafe<TData> p)
        {
            INodeLeafe<TData> q = null;
            if (p.U != null)
            {
                return GetMinimum(p.U);
            }
            else
            {
                q = p.P;
                while (q != null && p == q.U)
                {
                    p = q;
                    q = q.P;
                }
                return q;
            }
        }
        /// <summary>
        /// Returns the child node with the minimum value
        /// </summary>
        /// <param name="p">the node from which we should seek the node with the minimum value</param>
        /// <returns>The node with the minimum value</returns>
        public INodeLeafe<TData> GetMinimum(INodeLeafe<TData> p)
        {
            if (p == null)
            {
                return null;
            }
            while (p.V != null)
            {
                p = p.V;
            }
            return p;
        }
        public override INodeLeafe<TData> GetMinimum()
        {
            return GetMinimum(this.RootNode);
        }
        /// <summary>
        /// Returns the child node with the maximum value
        /// </summary>
        /// <param name="p">the node from which we should seek the node with the minimum value</param>
        /// <returns>The node with the minimum value</returns>
        public INodeLeafe<TData> GetMaximum(INodeLeafe<TData> p)
        {
            if (p == null)
            {
                return null;
            }
            while (p.U != null)
            {
                p = p.U;
            }
            return p;
        }
        public override INodeLeafe<TData> GetMaximum()
        {
            return GetMaximum(this.RootNode);
        }
        /// <summary>
        /// Copies the entire Collection<T> to a compatible one-dimensional Array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional Array that is the destination of the elements copied from Collection<T>. The Array must have zero-based indexing.
        /// </param>
        /// <param name="index">
        /// The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo(TData[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (array.Length - index < Length)
            {
                throw new ArgumentException("InsufficientSpace");
            }

            IList<INodeLeafe<TData>> arr = Inorder().ToList();
            for (int i = 0; i < arr.Count; i++)
            {
                array[index++] = arr[i].Value;
            }
        }
        public virtual TData FindIndex(int k)
        {
            //TODO Optimize
            //http://stackoverflow.com/questions/30013591/binary-tree-find-position-in-inorder-traversal
            //INode<T> treenode = InOrder(this.RootNode, new Counter(k));
            return Inorder().ElementAt(k).Value;
        }
        /// <summary>
        /// Returns the position of the key by using the InOrder sequence.
        /// </summary>
        /// <param name="key"></param>
        public virtual int IndexOf(IComparable key)
        {
            ///man sucht sich als erstes den wert und wenn man weiß wieviele kinder man hat (für den aktuellen knoten)
            ///dann weiß man auch an welcher position man sich in der inorder befindet und hätte so das element zurück geben können. 
            ///zumindest hab ich seine erklärung so verstanden.
            //return GetNodePrivate(value, this.RootNode);

            //TODO Optimize
            var list = Inorder();

            int l = 0;
            int h = list.Count() - 1;
            //binary search skriptum
            while (l <= h)
            {
                int m = (l + h) / 2;
                //5.CompareTo(6) = -1      First int is smaller.
                //6.CompareTo(5) =  1      First int is larger.
                //5.CompareTo(5) =  0      Ints are equal.
                if (key.CompareTo(list.ElementAt(m).Value) == 1)
                {
                    l = m + 1;
                }
                else if (key.CompareTo(list.ElementAt(m).Value) == -1)
                {
                    h = m - 1;
                }
                else
                {
                    return m;
                }
            }
            return l;
        }
    }
}
