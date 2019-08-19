using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="D">The Type of the Data which should be used in the DataStructure</typeparam>
    public interface IData<D>
    {
        D Value { get; set; }
    }
    //covariant for properties are not supported 
    //http://stackoverflow.com/questions/4348760/c-sharp-covariant-return-types-utilizing-generics

    /// <summary>
    /// Single Node Interfaces
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <typeparam name="T"></typeparam>
    public interface ISNode<D,T> : IInvariant<T>, IData<D>
        where T : ISNode<D, T>
    {
        T GetRight();
        void SetRight(T t);
    }
    public interface INode<D, T> : ISNode<D, T>
        where T : INode<D, T>
    {
        T GetLeft();
        void SetLeft(T t);
    }
    public class SingleNode<T> : ISNode<T, SingleNode<T>>
    {
        public SingleNode()
        {

        }
        public SingleNode(T data)
        {
            this.Value = data;
        }
        public SingleNode(T data, SingleNode<T> node)
            : this(data)
        {
            this.SetRight(node);
        }
        public virtual T Value
        {
            get;
            set;
        }

        public virtual SingleNode<T> GetRight()
        {
            throw new NotImplementedException();
        }

        public virtual void SetRight(SingleNode<T> t)
        {
            throw new NotImplementedException();
        }
    }
    public class Node<T> : SingleNode<T>, INode<T, Node<T>>
    {
        public Node()
            : base()
        {

        }
        public Node(T data)
            : base(data)
        {

        }
        public Node(T data, Node<T> left,Node<T> right)
            : base(data,left)
        {
            this.SetRight(right);
        }
        public virtual Node<T> GetLeft()
        {
            throw new NotImplementedException();
        }

        public virtual void SetLeft(Node<T> t)
        {
            throw new NotImplementedException();
        }

        public new Node<T> GetRight()
        {
            throw new NotImplementedException();
        }

        public void SetRight(Node<T> t)
        {
            throw new NotImplementedException();
        }
    }
}
