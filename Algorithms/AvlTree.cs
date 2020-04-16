using DataStructures;
using System;

namespace Algorithms
{
    /// <summary>
    /// AVL Tree 
    /// Recommended for tasks which require a lot of lookups
    /// Not recommended for inserts and deletions taks
    /// Check also the AVLTreeTest
    /// </summary>
    /// <remarks>
    /// Algorithm		Average	     Worst case
    /// Space            O(n)           O(n) 
    /// Search         O(log(n))     O(log(n))
    /// Insert         O(log(n))     O(log(n))
    /// Delete         O(log(n))     O(log(n))
    /// </remarks>
    /// <typeparam name="TData">The datatype to store</typeparam>
    public class AvlTree<TData> : AbstractTree<TData>
    {
        public class ANodeLeafe<TData> : NodeLeafe<TData>
        {
            public ANodeLeafe(IComparable comparer, TData Value) : base(comparer, Value)
            {
            }
            public int Balance { get; set; }
        }
        public AvlTree() : base()
        {
            FuncNodeFactory = (k, data) => new ANodeLeafe<TData>(k, data);
        }

        /// <inheritdoc/>
        public override void Add(IComparable k, TData data)
        {
            if (RootNode == null)
            {
                RootNode = FuncNodeFactory(k, data);
            }
            else
            {

                Add(RootNode, FuncNodeFactory(k, data));
            }
            Length = Length + 1;
        }

        /// <summary>
        /// Inserts a node
        /// </summary>
        /// <param name="node">parent node of the new node</param>
        /// <param name="newNode">The new node to insert</param>
        protected void Add(INodeLeafe<TData> node, INodeLeafe<TData> newNode)
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
        public override void Remove(IComparable k)
        {

            if (RootNode != null)
            {
                Remove(RootNode, k);
            }
        }

        /// <summary>
        /// Removes the node with the overgiven key
        /// </summary>
        /// <param name="node">The current node to start looking up for the key</param>
        /// <param name="k">The key which should be removed</param>
        private void Remove(INodeLeafe<TData> node, IComparable k)
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
                    INodeLeafe<TData> successor = null;
                    INodeLeafe<TData> p;

                    if (node.V == null || node.U == null)
                    {
                        successor = node;
                    }
                    else
                    {
                        // Successor
                        successor = GetSuccessor(node);

                        node.Key = successor.Key;
                        (node as ANodeLeafe<TData>).Balance = (successor as ANodeLeafe<TData>).Balance;
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

                    (successor as ANodeLeafe<TData>).Balance = CalculateBalance(successor);
                    if (successor.V != null) (successor.V as ANodeLeafe<TData>).Balance = CalculateBalance(successor.V);
                    if (successor.U != null) (successor.U as ANodeLeafe<TData>).Balance = CalculateBalance(successor.U);

                    ReBalanceToRoot(successor.P);
                }

            }
        }

        /// <summary>
        /// Calculate the balance of the node
        /// </summary>
        /// <param name="node">The node on which we need to calculate the balance</param>
        /// <returns>The balance value</returns>
        public int CalculateBalance(INodeLeafe<TData> node)
        {
            return Height(node.U) - Height(node.V);
        }

        /// <summary>
        /// Calculates the height of the node
        /// </summary>
        /// <param name="node">The node of which we calculate the height</param>
        /// <returns>The calculated value</returns>
        public int Height(INodeLeafe<TData> node)
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
        private void ReBalanceToRoot(INodeLeafe<TData> node)
        {
            if (node == null)
                return;

            INodeLeafe<TData> oldP = node.P;
            ReBalance(node);
            ReBalanceToRoot(oldP);
        }
        /// <summary>
        /// Rebalance from a node
        /// </summary>
        /// <param name="node">The noode which we should rebalance</param>
        private void ReBalance(INodeLeafe<TData> node)
        {

            int Balance = CalculateBalance(node);
            (node as ANodeLeafe<TData>).Balance = Balance;
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
        public INodeLeafe<TData> GetSuccessor(INodeLeafe<TData> node)
        {
            if (node.U != null)
            {
                return GetMinimum(node.U);
            }
            else
            {
                INodeLeafe<TData> successorNode = node.P;

                while (successorNode != null && node == successorNode.U)
                {
                    node = successorNode;
                    successorNode = successorNode.P;
                }

                return successorNode;
            }
        }

        /// <inheritdoc/>
        public override INodeLeafe<TData> GetMinimum()
        {
            return GetMinimum(RootNode);
        }

        /// <summary>
        /// Returns the node with the minimum key value from the overgiven node <paramref name="node"/>
        /// </summary>
        /// <param name="node">The node from to look up the node with the minimum key value</param>
        /// <returns>The node with the lowest key</returns>
        protected INodeLeafe<TData> GetMinimum(INodeLeafe<TData> node)
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
        public override INodeLeafe<TData> GetMaximum()
        {
            return GetMaximum(RootNode);
        }
        /// <summary>
        /// Returns the node with the maximum key value from the overgiven node <paramref name="node"/>
        /// </summary>
        /// <param name="node">The node from to look up the node with the maximum key value</param>
        /// <returns>The node with the maximum key value</returns>
        protected INodeLeafe<TData> GetMaximum(INodeLeafe<TData> node)
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
        protected INodeLeafe<TData> RotateLeft(INodeLeafe<TData> n)
        {
            INodeLeafe<TData> newRoot = n.U;
            INodeLeafe<TData> oldRoot = n;

            newRoot.P = oldRoot.P;
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

            oldRoot.U = newRoot.V;
            if (oldRoot.U != null)
                oldRoot.U.P = oldRoot;

            newRoot.V = oldRoot;
            oldRoot.P = newRoot;

            (newRoot as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot);
            if (newRoot.V != null) (newRoot.V as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot.V);
            if (newRoot.U != null) (newRoot.U as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot.U);

            return newRoot;
        }

        /// <summary>
        /// Operates a right rotation on the overgiven <paramref name="n"/> node
        /// </summary>
        /// <param name="n">The node to rotate</param>
        /// <returns>The root node of the subtree</returns>
        protected INodeLeafe<TData> RotateRight(INodeLeafe<TData> n)
        {
            INodeLeafe<TData> newRoot = n.V;
            INodeLeafe<TData> oldRoot = n;

            newRoot.P = oldRoot.P;
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

            oldRoot.V = newRoot.U;
            if (oldRoot.V != null)
                oldRoot.V.P = oldRoot;

            newRoot.U = oldRoot;
            oldRoot.P = newRoot;

            (newRoot as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot);
            if (newRoot.V != null) (newRoot.V as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot.V);
            if (newRoot.U != null) (newRoot.U as ANodeLeafe<TData>).Balance = CalculateBalance(newRoot.U);

            return newRoot;
        }
        /// <summary>
        /// Checks the balance of the overgiven node <paramref name="node"/>
        /// </summary>
        /// <param name="node">The node to check for balance</param>
        /// <returns>True if the balance is fine, otherwise false</returns>
        private bool CheckBalance(INodeLeafe<TData> node)
        {
            bool flag = true;

            int hl = Height(node.V);
            int hr = Height(node.U);
            int Balance = hr - hl;
            if (Balance >= 2 || Balance <= -2)
            {
                return false;
            }
            if (Balance != (node as ANodeLeafe<TData>).Balance)
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
