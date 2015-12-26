using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Represents a base tree class. Implemented binary tree concept.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tree<T> : ITree<T> where T : IComparable
    {
        /// <summary>
        /// Represents the method that will handle the get node functionality.
        /// </summary>
        /// <param name="value">The value which we are looking for.</param>
        /// <param name="root">The root node of the tree to start the traversing.</param>
        /// <returns>The node which contains the overgiven value.</returns>
        public delegate ITreeNode<T> GetNodeDelegate(T value, ITreeNode<T> root);

        /// <summary>
        /// Points to the getNodeHandler
        /// </summary>
        protected GetNodeDelegate getNodeHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree{T}"/> class.
        /// </summary>
        public Tree()
        {
            this.getNodeHandler = new GetNodeDelegate(this.GetNodePrivate);
        }
        /// <summary>
        /// Initializes a new instance of the  <see cref="Tree{T}"/> class.
        /// </summary>
        /// <param name="getNode">
        /// The delegate which implements the function GetNode(T value, ITreeNode<T> root). See the corresponding<see cref="Tree.GetNodeDelegate">delegate</see> 
        /// documentation for more details.
        /// </param>
        public Tree(GetNodeDelegate getNode)
        {
            this.getNodeHandler = getNode;
        }

        /// <summary>
        /// Initializes a new instance of the  <see cref="Tree{T}"/> class.
        /// </summary>
        /// <param name="getNode">
        /// A function which implements the GetNode(T value, ITreeNode<T> root). See the corresponding<see cref="Tree.GetNodeDelegate">delegate</see> 
        /// documentation for more details.</param>
        public Tree(Func<T, ITreeNode<T>, ITreeNode<T>> getNode)
        {
            this.getNodeHandler = new GetNodeDelegate(getNode);
        }
        /// <summary>
        /// Get or sets the tree root node.
        /// </summary>
        public ITreeNode<T> Root
        {
            get;
            protected set;
        }
        /// <summary>
        /// Gets the value that indicates whether the tree is empty or not
        /// </summary>
        public virtual bool Empty
        {
            get
            {
                return Root == null;
            }
        }
        //oder Node.AmountNode
        /// <summary>
        /// Gets the amount of nodes of the tree
        /// </summary>
        public int Length
        {
            get;
            protected set;
        }
        /// <summary>
        /// Gets the node with the correspondening value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ITreeNode<T> GetNode(T value)
        {
            return this.GetNodePrivate(value, this.Root);
        }
        protected virtual ITreeNode<T> GetNodePrivate<T>(T value, ITreeNode<T> root) where T : IComparable
        {
            ITreeNode<T> p = root;
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            while (p != null)
            {
                if (value.CompareTo(p.Value) == -1)
                {
                    p = p.Left;
                }
                else if (p.Value.CompareTo(value) == 0)
                {
                    return p;
                }
                else
                {
                    p = p.Right;
                }
            }

            return null;
        }
        /// <summary>
        /// Gets a value that indicates whether the overgiven value exists in the tree
        /// </summary>
        /// <param name="val"><The value to seek in the tree/param>
        /// <returns></returns>
        public bool Exists(T val)
        {
            return this.GetNode(val) != null;
        }
        /// <summary>
        /// Gets the height of the tree
        /// </summary>
        public int Height
        {
            get
            {
                return GetHeight(this.Root);
            }
        }
        /// <summary>
        /// Gets the height of the tree starting from the overgiven node
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The height from the node</returns>
        protected virtual int GetHeight(ITreeNode<T> node)
        {
            if (node == null)
            {
                return -1;
            }
            return 1 + (Math.Max(GetHeight(node.Left), GetHeight(node.Right)));
        }
        /// <summary>
        /// Reset the tree
        /// </summary>
        public void Clear()
        {
            //if (items.IsReadOnly)
            //{
                //ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            //}

            ClearItems();
        }
        /// <summary>
        /// Clear the nodes of the tree
        /// </summary>
        protected virtual void ClearItems()
        {
            this.Root = null;
        }
        /// <summary>
        /// Add a new value to the tree
        /// </summary>
        /// <param name="val">The value to add</param>
        public void Add(T val)
        {
            ITreeNode<T> q = new TreeNode<T>(val);
            ITreeNode<T> r = null; //r wird vorgÃ¤nger von q
            ITreeNode<T> p = this.Root;
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            while (p != null)
            {
                r = p;
                if (q.Value.CompareTo(p.Value) == -1)
                {
                    p = p.Left;
                }
                else if (p.Value.CompareTo(val) == 0)
                {
                    return; //if key already exists
                }
                else
                {
                    p = p.Right; //gleiche SchlÃ¼ssel kommen nach rechts
                }
            }
            q.Parent = r;
            q.Left = null;
            q.Right = null;

            if (r == null)
            {
                this.Root = q;
            }
            else
            {
                if (q.Value.CompareTo(r.Value) == -1)
                {
                    r.Left = q;
                }
                else
                {
                    r.Right = q;
                }
            }
            //increase size of tree;
            Length = Length + 1;
            //node.n = 1 + 
            //node.AmountofNode = size(node.Lef) + size(node.right)+1; (size.node) gibt node.amountnode zurück
        }
        /// <summary>
        /// The value to remove
        /// </summary>
        /// <param name="val"></param>
        public void Remove(T val)
        {
            if (Empty)
            {
                return;
            }
            ITreeNode<T> r = null;
            ITreeNode<T> root = this.Root;
            ITreeNode<T> q = GetNode(val);
            ITreeNode<T> p = null;

            if (q.Left == null || q.Right == null)
            {   //q hat max 1 NaChfolger --> wird selbst entfernt
                r = q;
            }
            else
            {
                //q hat 2 Nacfolger -> wird durch successor ersetzt, dieser wird entfernt
                r = Successor(q);
                //umhängen der daten von r nach q
                q.Value = r.Value;
            }
            //lasse p auf kind von r verweisen (p=null, falls r keine kinder hat)
            if (r.Left != null)
            {
                p = r.Left;
            }
            else
            {
                p = r.Right;
            }
            if (p != null)
            {
                p.Parent = r.Parent;
                //erzeuge einen verweis von p auf seinen neuen vorgÃ¤nger ( den vorgÃ¤nger von r)
            }
            //hänge p anstelle von r in den Baum ein
            if (r.Parent == null)
            {
                //r war Wurzel: neue wurzel ist p
                this.Root = p;
            }
            else
            {
                //hänge p an der richtigen seite des vorgÃ¤ngerknotens von r ein
                if (r == r.Parent.Left)
                {
                    r.Parent.Left = p; //p sei linker nachfolger
                }
                else
                {
                    r.Parent.Right = p;
                }
            }
            r = null;

            Length = Length - 1;
            //TODO Save amount of subtree in node
        }
        /// <summary>
        /// Returns all nodes of the tree ascending.
        /// </summary>
        /// <remarks>
        /// Recursiv implemention. By default you would call the method with the root node of the tree and pass an empty initalized collection.
        /// </remarks>
        /// <typeparam name="T">The value type of the nodes</typeparam>
        /// <param name="p">The starting node.</param>
        /// <param name="list">The current created list</param>
        /// <returns>A list of all nodes from the starting node.</returns>
        protected IList<ITreeNode<T>> InOrder<T>(ITreeNode<T> p, IList<ITreeNode<T>> list)
        {
            if (p != null)
            {
                InOrder<T>(p.Left, list);
                list.Add(p);
                InOrder<T>(p.Right, list);
            }
            return list;
        }
        /// <summary>
        /// Returns the successor from the overgiven node
        /// </summary>
        /// <param name="p">The node from which we should get the successor</param>
        /// <returns>The successor of the overgiven node</returns>
        public virtual ITreeNode<T> Successor(ITreeNode<T> p)
        {
            ITreeNode<T> q = null;
            if (p.Right != null)
            {
                return Minimum(p.Right);
            }
            else
            {
                q = p.Parent;
                while (q != null && p == q.Right)
                {
                    p = q;
                    q = q.Parent;
                }
                return q;
            }
        }
        /// <summary>
        /// Returns the child node with the minimum value
        /// </summary>
        /// <param name="p">the node from which we should seek the node with the minimum value</param>
        /// <returns>The node with the minimum value</returns>
        public virtual ITreeNode<T> Minimum(ITreeNode<T> p)
        {
            if (p == null)
            {
                return null;
            }
            while (p.Left != null)
            {
                p = p.Left;
            }
            return p;
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
        public void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (array.Length - index < Length)
            {
                throw new ArgumentException("InsufficientSpace");
            }

            IList<ITreeNode<T>> arr = InOrder(this.Root, new List<ITreeNode<T>>());
            for (int i = 0; i < arr.Count;i++ )
            {
                array[index++] = arr[i].Value;
            }
        }
        public virtual T FindIndex(int k)
        {
            //TODO Optimize
            //http://stackoverflow.com/questions/30013591/binary-tree-find-position-in-inorder-traversal
            //INode<T> treenode = InOrder(this.Root, new Counter(k));
            return InOrder(this.Root, new List<ITreeNode<T>>()).ElementAt(k).Value;
        }
        /// <summary>
        ///  Liefert die Position des Wertes val in der InorderReihenfolge aller Werte
        ///  im Baum zuruck. Wiederum gilt für diese Aufgabe, dass das Â¨ erste
        ///  Element an Position 0 steht. Falls das Element nicht im Baum vorhanden
        ///  ist, dann soll jene Position zuruckgeliefert werden an der es eingef Â¨
        ///  ugt werden w Â¨ urde
        /// </summary>
        /// <param name="value"></param>
        public virtual int IndexOf(T value)
        {
            ///man sucht sich als erstes den wert und wenn man weiß wieviele kinder man hat (für den aktuellen knoten)
            ///dann weiß man auch an welcher position man sich in der inorder befindet und hätte so das element zurück geben können. 
            ///zumindest hab ich seine erklärung so verstanden. hatte den simon strassl
            //return GetNodePrivate(value, this.Root);

            //TODO Optimize
            var list = InOrder(this.Root, new List<ITreeNode<T>>());

            int l = 0;
            int h = list.Count - 1;
            //binary search skriptum
            while (l <= h)
            {
                int m = (l + h) / 2;
                //5.CompareTo(6) = -1      First int is smaller.
                //6.CompareTo(5) =  1      First int is larger.
                //5.CompareTo(5) =  0      Ints are equal.
                if (value.CompareTo(list.ElementAt(m).Value) == 1)
                {
                    l = m + 1;
                }
                else if (value.CompareTo(list.ElementAt(m).Value) == -1)
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
        /// <summary>
        /// Tree node type which extends the classic node to the property childrens
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class Node<T> : Get.the.Solution.DataStructure.Node<T>
        {
            /// <summary>
            /// Returns the amount of childrens of the node
            /// </summary>
            public int Childrens { get; set; }
        }


    }
}
