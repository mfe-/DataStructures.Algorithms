using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{

    /// <summary>
    /// A Linked List implementation of the <see cref="ICollection"/> interface.
    /// </summary>
    /// <remarks>
    /// The <see cref="LinkedList"/> can be used with all objects
    /// <para>This class is not guaranteed to be thread safe.</para>
    /// </remarks>
    /// <typeparam name="T">Type of Value which should be keept.</typeparam>
    public class LinkedList<T> : ICollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the LinkedList<T> class.
        /// </summary>
        public LinkedList()
        {
            count = 0;
        }
        protected ISingleNode<T> first;
        /// <summary>
        /// Gets or sets the first element of LinkedList<T> .
        /// </summary>
        public virtual ISingleNode<T> First
        {
            get
            {
                return first;
            }
            private set
            {
                first = value;
            }
        }
        protected ISingleNode<T> last;
        /// <summary>
        /// Gets or sets the last element of LinkedList<T> .
        /// </summary>
        public virtual ISingleNode<T> Last
        {
            get
            {
                return last;
            }
            private set
            {
                last = value;
            }
        }
        /// <summary>
        /// Adds an item to the ICollection<T>.
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(T item)
        {
            this.Count = ++this.Count;
            if (first == null)
            {
                first = new SingleNode<T>(item);
                last = first;
            }
            else
            {
                SingleNode<T> singleNode = new SingleNode<T>(item);
                last.Right = singleNode;
                last = last.Right;
            }
        }
        /// <summary>
        /// Removes all items from the ICollection<T>.
        /// </summary>
        public void Clear()
        {
            first = null;
            Count = 0;
        }
        /// <summary>
        /// Determines whether the ICollection<T> contains a specific value.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            ISingleNode<T> start = this.First;
            while (start != null)
            {
                if (object.Equals(start.Value, item))
                {
                    return true;
                }
                start = start.Right;
            }
            return false;
        }
        /// <summary>
        /// Copies the elements of the ICollection<T> to an Array, starting at a particular Array index.
        /// <see href="https://msdn.microsoft.com/en-US/library/0efx51xw(v=vs.110).aspx">More informations regarding CopyTo</see>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        protected int count;
        /// <summary>
        /// Gets the number of elements contained in the ICollection<T>.
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
            protected set
            {
                count = value;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the ICollection<T> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Removes the first occurrence of a specific <see cref="object"/>  from the <see cref="LinkedList"/>.
        /// <see href="https://msdn.microsoft.com/en-us/library/bye7h94w(v=vs.110).aspx">More informations regarding remove item.</see>
        /// </summary>
        /// <param name="item">The <see cref="object"/>  to remove from the LinkedList<T>.</param>
        /// <returns><value>true</value> if item was successfully removed from the <seealso cref="LinkedList<T>"/>; otherwise, <value>false</value>. This method also returns false if item is not found in the original <seealso cref="LinkedList<T>"/>.</returns>
        public virtual bool Remove(T value)
        {
            ISingleNode<T> start = this.First;
            ISingleNode<T> preview = null;

            while (start != null)
            {
                if (start.Value.Equals(value))
                {
                    if (object.Equals(start.Value, value))
                    {
                        if (start == this.First)
                        {
                            this.First = null;
                        }
                        else
                        {
                            preview.Right = start.Right;

                            if (Last == start)
                            {
                                Last = preview;
                            }
                        }
                        count--;
                        return true;
                    }
                }
                preview = start;
                start = start.Right;
            }
            return false;
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection. (Inherited from IEnumerable<T>.)
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            ISingleNode<T> start = this.First;
            while (start != null)
            {
                yield return start.Value;
                start = start.Right;
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection. (Inherited from IEnumerable<T>.)
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SingleListIEnumerator<T>(this.First);
        }
    }
}
