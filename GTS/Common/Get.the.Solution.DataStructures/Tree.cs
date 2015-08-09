using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    public class Tree<T> where T : IComparable
    {
        public delegate ITreeNode<T> getNodeDelegate(T value, ITreeNode<T> root);
        protected getNodeDelegate getNodeHandler;

        public Tree()
        {
            this.getNodeHandler = new getNodeDelegate(this.GetNodePrivate);
        }
        public Tree(getNodeDelegate getNode)
        {
            this.getNodeHandler = getNode;
        }

        public ITreeNode<T> Root
        {
            get;
            protected set;
        }
        public virtual bool Empty
        {
            get
            {
                return Root == null;
            }
        }

        public int Size
        {
            get;
            protected set;
        }
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
        public bool Exists(T val)
        {
            return this.GetNode(val) != null;
        }
        public int Height
        {
            get
            {
                return height(this.Root);
            }
        }

        protected virtual int height(ITreeNode<T> node)
        {
            if (node == null)
            {
                return -1;
            }
            return 1 + (Math.Max(height(node.Left), height(node.Right)));
        }

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
            Size = Size + 1;

            //TODO Save amount of subtree in node

        }

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
                //q hat 2 NAcfolger -> wird durch successor ersetzt, dieser wird entfernt
                r = Successor(q);
                //umhÃ¤ngen der daten von r nach q
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
            //hÃ¤nge p anstelle von r in den Baum ein
            if (r.Parent == null)
            {
                //r war Wurzel: neue wurzel ist p
                this.Root = p;
            }
            else
            {
                //hÃ¤nge p an der richtigen seite des vorgÃ¤ngerknotens von r ein
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

            Size = Size - 1;
            //TODO Save amount of subtree in node
        }
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
        public ITreeNode<T> Minimum(ITreeNode<T> p)
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
        public T valueAtPosition(int k)
        {
            ///ich habs ehrlich gesagt nicht genau gewusst. man sucht sich als erstes den wert und wenn man weiß wieviele kinder man hat (für den aktuellen knoten)
            ///dann weiß man auch an welcher position man sich in der inorder befindet und hätte so das element zurück geben können. 
            ///zumindest hab ich seine erklärung so verstanden. hatte den simon strassl


            return default(T);
        }
        /// <summary>
        ///     Liefert die Position des Wertes val in der InorderReihenfolge aller Werte
        ///  im Baum zuruck. Wiederum gilt f Â¨ ur diese Aufgabe, dass das Â¨ erste
        ///  Element an Position 0 steht. Falls das Element nicht im Baum vorhanden
        ///  ist, dann soll jene Position zuruckgeliefert werden an der es eingef Â¨
        ///  ugt werden w Â¨ urde
        /// </summary>
        /// <param name="value"></param>
        public int position(T value)
        {
            return 0;
        }



    }
}
