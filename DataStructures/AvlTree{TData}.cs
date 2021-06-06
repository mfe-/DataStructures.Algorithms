using System;

namespace DataStructures
{
    /// <summary>
    /// AVL Tree 
    /// Recommended for tasks which require a lot of lookups and less rebalancing.
    /// If the majority of operations of the tree are insertions or deletions and worst-case orders are not expected
    /// it's recommended to use <seealso cref="BinarySearchTree{TData}"/>
    /// Check also the AVLTreeTest
    /// </summary>
    /// <remarks>
    /// Algorithm		Average	     Worst case
    /// Space            O(n)           O(n) 
    /// Search         O(log(n))     O(log(n))
    /// Insert         O(log(n))     O(log(n))
    /// Delete         O(log(n))     O(log(n))
    /// </remarks>
    /// <typeparam name="TData">The datatype which is used for storing values</typeparam>
    public class AvlTree<TData> : AbstractTree<INodeTree<TData>, TData>
    {
        /// <summary>
        /// Node class used in avl tree
        /// </summary>
        /// <typeparam name="TData1"></typeparam>
        public class ANodeLeafe<TData1> : INodeTree<TData1>
        {
            /// <summary>
            /// Initializes a new node leafe
            /// </summary>
            public ANodeLeafe(IComparable comparer, TData1 value)
            {
                Key = comparer;
                Value = value;
            }
            /// <inheritdoc/>
            public int Balance { get; set; }
            /// <inheritdoc/>
            public TData1 Value { get; set; }
            /// <inheritdoc/>
            public INodeTree<TData1>? P { get; set; }
            /// <inheritdoc/>
            public IComparable Key { get; set; }
            /// <inheritdoc/>
            public INodeTree<TData1>? V { get; set; }
            /// <inheritdoc/>
            public INodeTree<TData1>? U { get; set; }
        }
        /// <summary>
        /// Initializes a new instance of the avl tree.
        /// </summary>
        public AvlTree() : base()
        {
            FuncNodeFactory = (k, data) => new ANodeLeafe<TData>(k, data);
        }

        /// <inheritdoc/>
        public override void Add(IComparable key, TData data)
        {
            if (RootNode == null)
            {
                RootNode = FuncNodeFactory(key, data);
            }
            else
            {

                Add(RootNode, FuncNodeFactory(key, data));
            }
            Count = Count + 1;
        }

        /// <summary>
        /// Inserts a node
        /// </summary>
        /// <param name="node">parent node of the new node</param>
        /// <param name="newNode">The new node to insert</param>
        protected virtual void Add(INodeTree<TData> node, INodeTree<TData> newNode)
        {
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            //newNode.Key < node.Key
            if (newNode.Key.CompareTo(node.Key) == -1)
            {
                // Key needs to be left
                if (node.V != null)
                {
                    // still one left below
                    Add(node.V, newNode);
                    ReBalance(node);

                }
                else
                {
                    // insert left
                    node.V = newNode;
                    newNode.P = node;
                }
            } //newNode.Key > node.Key
            else if (newNode.Key.CompareTo(node.Key) == 1)
            {
                // Key needs to be right
                if (node.U != null)
                {
                    // traversal right below 
                    Add(node.U, newNode);
                    ReBalance(node);
                }
                else
                {
                    // insert right
                    node.U = newNode;
                    newNode.P = node;
                }
            }
            else
            {
                // key already exist
                throw new ArgumentException($"An item with the same key {node.Key} has already been added.");
            }
        }


        /// <inheritdoc/>
        public override void Remove(IComparable key)
        {

            if (RootNode != null)
            {
                Remove(RootNode, key);
            }
        }

        /// <summary>
        /// Removes the node with the overgiven key
        /// </summary>
        /// <param name="node">The current node to start looking up for the key</param>
        /// <param name="k">The key which should be removed</param>
        private void Remove(INodeTree<TData>? node, IComparable k)
        {
            if (node != null)
            {
                //5.CompareTo(6) = -1      First int is smaller.
                //6.CompareTo(5) =  1      First int is larger.
                //5.CompareTo(5) =  0      Ints are equal.
                if (node.Key.CompareTo(k) == 1)
                {
                    // search the key left 
                    Remove(node.V, k);

                }
                else if (node.Key.CompareTo(k) == -1)
                {
                    // search the key right
                    Remove(node.U, k);

                }
                else
                {
                    //found key
                    INodeTree<TData>? successor = null;
                    INodeTree<TData>? p;

                    if (node.V == null || node.U == null)
                    {
                        successor = node;
                    }
                    else
                    {
                        // Successor
                        successor = GetSuccessor(node);
                        node.Key = successor.Key;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        (node as ANodeLeafe<TData>).Balance = (successor as ANodeLeafe<TData>).Balance;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }

                    if (successor.V != null)
                    {
                        p = successor.V;
                    }
                    else
                    {
                        p = successor.U;
                    }

                    if (p != null)
                    {
                        p.P = successor.P;
                    }

                    if (successor.P == null)
                    {
                        RootNode = p;
                    }
                    else
                    {
                        if (successor.P.V != null && successor.Key == successor.P.V.Key)
                        {
                            successor.P.V = p;
                        }
                        else
                        {
                            successor.P.U = p;
                        }
                    }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    (successor as ANodeLeafe<TData>).Balance = CalculateBalance(successor);
                    if (successor.V != null) (successor.V as ANodeLeafe<TData>).Balance = CalculateBalance(successor.V);
                    if (successor.U != null) (successor.U as ANodeLeafe<TData>).Balance = CalculateBalance(successor.U);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    ReBalanceToRoot(successor.P);
                }

            }
        }

        /// <summary>
        /// Calculate the balance of the node
        /// </summary>
        /// <param name="node">The node on which we need to calculate the balance</param>
        /// <returns>The balance value</returns>
        public int CalculateBalance(INodeTree<TData> node)
        {
            return Height(node.U) - Height(node.V);
        }

        /// <summary>
        /// Calculates the height of the node
        /// </summary>
        /// <param name="node">The node of which we calculate the height</param>
        /// <returns>The calculated value</returns>
        public int Height(INodeTree<TData>? node)
        {
            if (node == null)
            {
                return 0;
            }

            return 1 + Math.Max(Height(node.V), Height(node.U));
        }

        /// <summary>
        /// Rebalance from the overgiven <paramref name="node"/> to the root node
        /// </summary>
        /// <param name="node">The noode which we should rebalance</param>
        private void ReBalanceToRoot(INodeTree<TData>? node)
        {
            if (node == null)
                return;

            INodeTree<TData>? oldP = node.P;
            ReBalance(node);
            ReBalanceToRoot(oldP);
        }
        /// <summary>
        /// Rebalance from a node
        /// </summary>
        /// <param name="node">The noode which we should rebalance</param>
        protected void ReBalance(INodeTree<TData> node)
        {
            int Balance = CalculateBalance(node);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            (node as ANodeLeafe<TData>).Balance = Balance;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            // check Balance
            if (Balance == -2)
            {

                if (node.V != null && Height(node.V.V) >= Height(node.V.U))
                {
                    // case 1.1
                    RotateRight(node);
                }
                else
                {
                    // case 1.2
                    if (node.V == null)
                    {
                        throw new InvalidOperationException("When rotating the left node was null. Tree is broken. case 1.2");
                    }
                    RotateLeft(node.V);
                    RotateRight(node);

                }
            }
            else if (Balance == 2)
            {
                if (node.U != null && Height(node.U.U) >= Height(node.U.V))
                {
                    // case 2.1
                    RotateLeft(node);
                }
                else
                {
                    // case 2.2
                    if (node.U == null)
                    {
                        throw new InvalidOperationException("When rotating the right node was null. Tree is broken. case 2.2");
                    }
                    RotateRight(node.U);
                    RotateLeft(node);
                }

            }

        }
        /// <summary>
        /// Returns the successor of the overgiven node
        /// </summary>
        /// <param name="node">The node to look up for the successor</param>
        /// <returns>The successor of the node</returns>
        public INodeTree<TData> GetSuccessor(INodeTree<TData> node)
        {
            INodeTree<TData>? successor;
            if (node.U != null)
            {
                successor = GetMinimum(node.U);
            }
            else
            {
                INodeTree<TData>? successorNode = node.P;

                while (successorNode != null && node == successorNode.U)
                {
                    node = successorNode;
                    successorNode = successorNode.P;
                }

                successor = successorNode;
            }
            if (successor == null)
            {
                throw new ArgumentNullException(nameof(node), "No successor found. Something looks broken.");
            }
            return successor;
        }

        /// <inheritdoc/>
        public override INodeTree<TData>? GetMinimum()
        {
            return GetMinimum(RootNode);
        }

        /// <summary>
        /// Returns the node with the minimum key value from the overgiven node <paramref name="node"/>
        /// </summary>
        /// <param name="node">The node from to look up the node with the minimum key value</param>
        /// <returns>The node with the lowest key</returns>
        protected INodeTree<TData>? GetMinimum(INodeTree<TData>? node)
        {
            if (node == null)
            {
                return null;
            }

            while (node.V != null)
            {
                node = node.V;
            }
            return node;
        }
        /// <inheritdoc/>
        public override INodeTree<TData>? GetMaximum()
        {
            return GetMaximum(RootNode);
        }
        /// <summary>
        /// Returns the node with the maximum key value from the overgiven node <paramref name="node"/>
        /// </summary>
        /// <param name="node">The node from to look up the node with the maximum key value</param>
        /// <returns>The node with the maximum key value</returns>
        protected INodeTree<TData>? GetMaximum(INodeTree<TData>? node)
        {
            if (node == null)
            {
                return null;
            }

            while (node.U != null)
            {
                node = node.U;
            }
            return node;
        }

        /// <summary>
        /// Operates a left rotation on the overgiven <paramref name="n"/> node
        /// </summary>
        /// <param name="n">The node to rotate</param>
        /// <returns>The root node of the subtree</returns>
        protected INodeTree<TData> RotateLeft(INodeTree<TData> n)
        {
            INodeTree<TData>? newRoot = n.U;
            INodeTree<TData> oldRoot = n;
            if (newRoot != null)
            {
                newRoot.P = oldRoot.P;
            }
            if (oldRoot.P == null)
            {
                RootNode = newRoot;
            }
            else
            {
                if (oldRoot.P.V != null && oldRoot.P.V == oldRoot)
                    oldRoot.P.V = newRoot;
                else
                    oldRoot.P.U = newRoot;
            }

            oldRoot.U = newRoot?.V;
            if (oldRoot.U != null)
                oldRoot.U.P = oldRoot;

            if (newRoot != null)
            {
                newRoot.V = oldRoot;
            }

            oldRoot.P = newRoot;
            if (newRoot == null)
            {
                throw new InvalidOperationException("While rotating left the new root was set to null. Tree is broken.");
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            (newRoot as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot);
            if (newRoot?.V != null) (newRoot.V as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot.V);
            if (newRoot?.U != null) (newRoot.U as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot.U);
#pragma warning disable CS8603 // Possible null reference return.
            return newRoot;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        /// <summary>
        /// Operates a right rotation on the overgiven <paramref name="n"/> node
        /// </summary>
        /// <param name="n">The node to rotate</param>
        /// <returns>The root node of the subtree</returns>
        protected INodeTree<TData> RotateRight(INodeTree<TData> n)
        {
            INodeTree<TData>? newRoot = n.V;
            INodeTree<TData> oldRoot = n;
            if (newRoot != null)
            {
                newRoot.P = oldRoot.P;
            }
            if (oldRoot.P == null)
            {
                RootNode = newRoot;
            }
            else
            {
                if (oldRoot.P.V != null && oldRoot.P.V == oldRoot)
                    oldRoot.P.V = newRoot;
                else
                    oldRoot.P.U = newRoot;
            }

            oldRoot.V = newRoot?.U;
            if (oldRoot.V != null)
                oldRoot.V.P = oldRoot;

            if (newRoot != null)
            {
                newRoot.U = oldRoot;
            }

            oldRoot.P = newRoot;
            if (newRoot == null)
            {
                throw new InvalidOperationException("While rotating right the new root was set to null. Tree is broken.");
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            (newRoot as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot);
            if (newRoot?.V != null) (newRoot.V as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot.V);
            if (newRoot?.U != null) (newRoot.U as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot.U);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
            return newRoot;
#pragma warning restore CS8603 // Possible null reference return.
        }
        /// <summary>
        /// Checks the balance of the overgiven node <paramref name="node"/>
        /// </summary>
        /// <param name="node">The node to check for balance</param>
        /// <returns>True if the balance is fine, otherwise false</returns>
        private bool CheckBalance(INodeTree<TData> node)
        {
            bool flag = true;

            int hl = Height(node.V);
            int hr = Height(node.U);
            int Balance = hr - hl;
            if (Balance >= 2 || Balance <= -2)
            {
                return false;
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (Balance != (node as ANodeLeafe<TData>).Balance)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                return false;
            }
            if (node.V != null)
            {
                if (!CheckBalance(node.V))
                    flag = false;
            }
            if (node.U != null)
            {
                if (!CheckBalance(node.U))
                    flag = false;
            }
            return flag;
        }
    }
}
