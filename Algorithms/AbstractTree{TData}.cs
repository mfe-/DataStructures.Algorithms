using DataStructures;
using System;
using System.Collections.Generic;

namespace Algorithms
{
    /// <summary>
    /// Tree
    /// </summary>
    /// <typeparam name="TData">The datatype which is used for storing values</typeparam>
    public abstract class AbstractTree<TNode, TData> 
        where TNode : class, INodeTree<TNode>, INode<TNode>, IData<TData>
    {
        public TNode RootNode { get; protected set; }
        /// <summary>
        /// Factory for creating Nodes
        /// </summary>
        protected Func<IComparable, TData, TNode> FuncNodeFactory = null;
        /// <summary>
        /// Creates a empty <seealso cref="AbstractTree{TData}"/>
        /// </summary>
        protected AbstractTree()
        {
            RootNode = null;
        }
        /// <summary>
        /// Gets the value that indicates whether the tree is empty or not
        /// </summary>
        public virtual bool Empty
        {
            get
            {
                return RootNode == null;
            }
        }
        /// <summary>
        /// Gets the amount of nodes of the tree
        /// </summary>
        public int Count
        {
            get;
            protected set;
        }

        /// <summary>
        /// Inserts data with a key
        /// </summary>
        /// <param name="k">The key</param>
        /// <param name="data">The data which is associated with the key</param>
        /// <exception cref="ArgumentException">If the key already exists</exception>
        public abstract void Add(IComparable k, TData data);
        /// <summary>
        /// Inserts data with a key
        /// </summary>
        /// <param name="data">The data</param>
        /// <param name="funcReturnKey">Function whichs computes the key from <typeparamref name="TData"/></param>
        /// <exception cref="ArgumentException">If the key already exists</exception>
        public void Add(Func<TData, IComparable> funcReturnKey, TData data)
        {
            Add(funcReturnKey(data), data);
        }

        /// <summary>
        /// Removes the overgiven key if it exists
        /// </summary>
        /// <param name="k">Removes the overgiven key if it exists</param>
        public abstract void Remove(IComparable k);

        /// <summary>
        /// Gets the node with the correspondening value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual TNode GetNode(IComparable key, Action<TNode> actionCurrentNode = null)
        {
            TNode p = RootNode;
            //5.CompareTo(6) = -1      First int is smaller.
            //6.CompareTo(5) =  1      First int is larger.
            //5.CompareTo(5) =  0      Ints are equal.
            while (p != null)
            {
                actionCurrentNode?.Invoke(p);
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
        /// Get the node with the minimum key value of the current tree
        /// </summary>
        /// <returns>The Node with the minimum key of the tree</returns>
        public abstract TNode GetMinimum();

        /// <summary>
        /// Get the node with the maximum key value of the current tree
        /// </summary>
        /// <returns>The Node with the maximum key value of the tree</returns>
        public abstract TNode GetMaximum();

        public TNode Find(Func<TData, bool> funcFind)
        {
            return Find(new Func<TNode, bool>((node) => funcFind(node.Value)));
        }
        public TNode Find(Func<TNode, bool> funcFind)
        {
            Stack<TNode> s = new Stack<TNode>();
            TNode current = RootNode;

            // traverse the tree  
            while (current != null || s.Count > 0)
            {
                // Reach the left most Node of the  curr Node 
                while (current != null)
                {
                    // place pointer to a tree node on the stack before traversing  
                    // the node's left subtree
                    s.Push(current);
                    current = current.V;
                }
                current = s.Pop();
                if (funcFind.Invoke(current))
                {
                    return current;
                }
                // we have visited the node and its left subtree. 
                // Now, it's right subtree's turn
                current = current.U;
            }
            return null;
        }
        public TNode Find(TData data)
        {
            return Find((current) => (data.Equals(current.Value)));
        }

        /// <summary>
        /// Creates a IEnumerable using Inorder.
        /// In inorder, the root is visited in the middle
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> Inorder()
        {
            Stack<TNode> s = new Stack<TNode>();
            TNode current = RootNode;

            // traverse the tree  
            while (current != null || s.Count > 0)
            {
                // Reach the left most Node of the  curr Node 
                while (current != null)
                {
                    // place pointer to a tree node on the stack before traversing  
                    // the node's left subtree
                    s.Push(current);
                    current = current.V;
                }
                current = s.Pop();
                yield return current;
                // we have visited the node and its left subtree. 
                // Now, it's right subtree's turn
                current = current.U;
            }
        }

        /// <summary>
        /// Create a inorder using <paramref name="inOrderList"/> and <paramref name="n"/>
        /// </summary>
        /// <remarks>Call for example with  Inorder(RootNode, new List<INodeLeafe<TData>>());</remarks>
        /// <param name="n">Left nodes</param>
        /// <param name="inOrderList">Right nodes</param>
        protected virtual void Inorder(TNode n, ICollection<TNode> inOrderList)
        {
            if (n == null)
                return;

            Inorder(n.V, inOrderList);

            inOrderList.Add(n);

            Inorder(n.U, inOrderList);
        }
    }
}
