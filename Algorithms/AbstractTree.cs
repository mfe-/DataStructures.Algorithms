﻿using DataStructures;
using System;
using System.Collections.Generic;

namespace Algorithms
{
    /// <summary>
    /// Tree
    /// </summary>
    /// <typeparam name="TData">The datatype to store</typeparam>
    public abstract class AbstractTree<TData>
    {

        /// <summary>
        /// Root node of AVL Tree
        /// </summary>
        public INodeLeafe<TData> RootNode;
        /// <summary>
        /// Factory for creating Nodes
        /// </summary>
        protected Func<IComparable, TData, INodeLeafe<TData>> FuncNodeFactory = null;
		/// <summary>
		/// Creates a empty <seealso cref="AbstractTree{TData}"/>
		/// </summary>
		public AbstractTree()
        {
            RootNode = null;
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
        public void Add(Func<TData,IComparable> funcReturnKey, TData data)
        {
            Add(funcReturnKey(data), data);
        }

        /// <summary>
        /// Removes the overgiven key if it exists
        /// </summary>
        /// <param name="k">Removes the overgiven key if it exists</param>
        public abstract void Remove(IComparable k);

        /// <summary>
        /// Get the node with the minimum key value of the current tree
        /// </summary>
        /// <returns>The Node with the minimum key of the tree</returns>
        public abstract INodeLeafe<TData> GetMinimum();

        /// <summary>
        /// Get the node with the maximum key value of the current tree
        /// </summary>
        /// <returns>The Node with the maximum key value of the tree</returns>
        public abstract INodeLeafe<TData> GetMaximum();

        /// <summary>
        /// Creates a IEnumerable using Inorder 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<INodeLeafe<TData>> Inorder()
        {
            IList<INodeLeafe<TData>> ret = new List<INodeLeafe<TData>>();

            Inorder(RootNode, ret);

            return ret;
        }

        /// <summary>
        /// Create a inorder using <paramref name="io"/> and <paramref name="n"/>
        /// </summary>
        /// <param name="n">Left nodes</param>
        /// <param name="io">Right nodes</param>
        protected virtual void Inorder(INodeLeafe<TData> n, ICollection<INodeLeafe<TData>> io)
        {
            if (n == null)
                return;

            Inorder(n.V, io);

            io.Add(n);

            Inorder(n.U, io);
        }
    }
}
