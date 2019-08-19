using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    //http://www.codeproject.com/Articles/2836/Doubly-Linked-List-Implementation
    public class SingleListIEnumerator<T> : IEnumerator
    {
        protected ISingleNode<T> initSingleNode;

        public SingleListIEnumerator(ISingleNode<T> singlenode)
        {
            initSingleNode = singlenode;
            Current = initSingleNode;
        }

        public object Current
        {
            get;
            private set;
        }

        public virtual bool MoveNext()
        {
            bool moveSuccessful = false;

            Current = initSingleNode.Right;

            if (Current != initSingleNode)
                moveSuccessful = true;

            return moveSuccessful;
        }

        public virtual void Reset()
        {
            Current = initSingleNode;
        }
    }
}
