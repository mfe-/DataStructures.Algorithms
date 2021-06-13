using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    /// <summary>
    /// BST - Binary search tree
    /// Recommended for lookups on "dynamic data" (which is changing a lot).
    /// </summary>
    /// <remarks>
    /// Algorithm		Average	     Worst case
    /// Space            O(n)           O(n) 
    /// Search         O(log(n))     O(log(n))
    /// Insert         O(log(n))     O(log(n))
    /// Delete         O(log(n))     O(log(n))
    /// </remarks>
    /// <typeparam name="TData">The datatype which is used for storing values</typeparam>
    public partial class BinarySearchTree<TData> : AbstractTree<INodeTree<TData>, TData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySearchTree{T}"/> class.
        /// </summary>
        public BinarySearchTree() : base()
        {
            FuncNodeFactory = new Func<IComparable, TData, BstNode<TData>>((key, data) => new BstNode<TData>(key, data));
        }
        /// <summary>
        /// Gets a value that indicates whether the overgiven value exists in the tree
        /// </summary>
        /// <param name="key">The value to seek in the tree</param>
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
        protected virtual int GetHeight(INodeTree<TData>? node)
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
        /// <inheritdoc/>
        public override void Add(IComparable key, TData data)
        {
            INodeTree<TData> q = FuncNodeFactory.Invoke(key, data);
            INodeTree<TData>? r = null; //r will be predecessor of q
            INodeTree<TData>? p = this.RootNode;
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            while (p != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                (p as BstNode<TData>).AmountofNode += 1;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                r = p;
                if (q.Key.CompareTo(p.Key) == -1)
                {
                    p = p.V;
                }
                else if (p.Key.CompareTo(key) == 0)
                {
                    //if key already exists
                    throw new ArgumentException($"An item with the same key {p.Key} has already been added.");
                }
                else
                {
                    p = p.U; //key greater - add to right
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
            Count = Count + 1;
        }
        /// <inheritdoc/>
        public override void Remove(IComparable key)
        {
            if (Empty)
            {
                return;
            }
            INodeTree<TData>? r = null;
            //decrease AmountofNode when removing items
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Action<INodeTree<TData>> actionNode = (n) => (n as BstNode<TData>).AmountofNode -= 1;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            INodeTree<TData>? q = GetNode(key, actionNode);
            INodeTree<TData>? p = null;

            if (q?.V == null || q.U == null)
            {   //q has max 1 Successor --> will be removed
                r = q;
            }
            else
            {
                //q got two Successor -> will be replaced with successor, the other wil be removed                r = Successor(q);
                //put the data from r to q (q.Value = r.Value;)
                //q = FuncNodeFactory.Invoke(r.Value, q.P, q.V, q.U);
                if (r == null)
                {
                    throw new InvalidOperationException("node r is null. Tree broken.");
                }
                var temp = FuncNodeFactory.Invoke(r.Key, r.Value);
                temp.P = q.P;
                temp.V = q.V;
                temp.U = q.U;
                q = temp;
                if (q == null)
                { }
            }
            if (r == null)
            {
                throw new InvalidOperationException("node r is null. Tree broken.");
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

            Count = Count - 1;
        }
        /// <summary>
        /// Returns the successor from the overgiven node
        /// </summary>
        /// <param name="p">The node from which we should get the successor</param>
        /// <returns>The successor of the overgiven node</returns>
        public virtual INodeTree<TData>? Successor(INodeTree<TData> p)
        {
            INodeTree<TData>? q;
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
        public INodeTree<TData>? GetMinimum(INodeTree<TData>? p)
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
        /// <inheritdoc/>
        public override INodeTree<TData>? GetMinimum()
        {
            return GetMinimum(this.RootNode);
        }
        /// <summary>
        /// Returns the child node with the maximum value
        /// </summary>
        /// <param name="p">the node from which we should seek the node with the minimum value</param>
        /// <returns>The node with the minimum value</returns>
        public INodeTree<TData>? GetMaximum(INodeTree<TData>? p)
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
        /// <inheritdoc/>
        public override INodeTree<TData>? GetMaximum()
        {
            return GetMaximum(this.RootNode);
        }
        /// <summary>
        /// Copies the entire Collection to a compatible one-dimensional Array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from Collection. The Array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
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

            if (array.Length - index < Count)
            {
                throw new ArgumentException("InsufficientSpace");
            }

            IList<INodeTree<TData>> arr = Inorder().ToList();
            for (int i = 0; i < arr.Count; i++)
            {
                array[index++] = arr[i].Value;
            }
        }
        /// <summary>
        /// Get Element at index using Inorder
        /// Runtime O(log n)
        /// </summary>
        /// <param name="index">The element at index </param>
        /// <returns></returns>
        public INodeTree<TData>? GetElementAt(int index)
        {
            return GetElementAt(index, (this.RootNode as BstNode<TData>));
        }
        /// <summary>
        /// Makes use of the AmountofNode information to retriev the Element at the overgive <paramref name="index"/> in O(logn)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bNodeLeafe"></param>
        /// <returns></returns>
        protected INodeTree<TData>? GetElementAt(int index, BstNode<TData>? bNodeLeafe)
        {
            int leftNodes = 0;
            if (bNodeLeafe?.V != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                leftNodes = (bNodeLeafe.V as BstNode<TData>).AmountofNode + 1;
            }
            if (index == leftNodes)
            {
                return bNodeLeafe;
            }
            if (index < leftNodes)
            {
                return GetElementAt(index, (bNodeLeafe?.V as BstNode<TData>));
            }
            if (index > leftNodes)
            {
                return GetElementAt(index - (leftNodes + 1), (bNodeLeafe?.U as BstNode<TData>));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return null;
        }
        /// <summary>
        /// Returns the <see cref="INodeTree{TData}"/> associated with the given key.
        /// If an entry with the given key is not found, an argument exception will be thrown
        /// </summary>
        /// <param name="key">The key to lookup</param>
        /// <returns>The associated <see cref="INodeTree{TData}"/></returns>
        public INodeTree<TData> this[IComparable key]
        {
            get
            {
                var node = GetNode(key);
                if (node == null)
                {
                    throw new ArgumentException(nameof(key));
                }
                return node;
            }
        }
    }
}
