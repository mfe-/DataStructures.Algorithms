using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure.Test
{
    /// <summary>
    /// Tested Tree Struct from Algorithm and Data Structs 
    /// Author: Claudia
    /// </summary>
    [Export("TreeTest",typeof(ITree<int>))]
    public class TreeTestStruct : ITree<int>
    {
        public TreeTestStruct leftnode = null;
        public TreeTestStruct rightnode = null;
        public int key = 0;
        public int Amount = -1;

        public bool isEmpty()
        {
            if (Amount == -1)
            {
                return true;
            }

            return false;
        }

        public bool Empty { get { return isEmpty(); } }

        public void Clear()
        {
            //if (items.IsReadOnly)
            //{
            //ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            //}

            this.leftnode = null;
        }

        public int size()
        {
            return (Amount == -1) ? 0 : Amount;
        }
        public int Length { get { return size(); } }

        public bool Exists(int val)
        {
            if (val == key)
            {
                return true;
            }
            else
            {
                if (key > val)
                {
                    if (this.leftnode != null)
                    {
                        return leftnode.Exists(val);
                    }
                }
                else if (key < val)
                {
                    if (this.rightnode != null)
                    {
                        return rightnode.Exists(val);
                    }
                }
            }

            return false;
        }

        public int Height { get { return height(); } }

        public int height()
        {
            if (isEmpty())
            {
                return -1;	//if tree is empty (angabe)
            }
            int anz = 0;		//if not empty low return 0 

            if (leftnode != null)
            {
                anz = leftnode.height() + 1;
            }

            int anzright = 0;
            if (rightnode != null)
            {
                anzright = rightnode.height() + 1;
            }

            if (anzright > anz)
            {
                anz = anzright;
            }

            return anz;
        }
        public void CopyTo(int[] array, int index)
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

            if (index != 0)
                throw new NotImplementedException();

            array = Inorder(this, 0, 0);
        }
        private bool insert_rek(int val)
        {
            bool is_ok = false;
            if (Amount == -1)
            {
                //Tester.printDebug("anzNodes==-1 generate new base node");
                this.key = val;
                this.Amount = 1;
            }
            else
            {
                if (key > val)
                {
                    if (this.leftnode != null)
                    {
                        is_ok = this.leftnode.insert_rek(val);
                        if (is_ok)
                        {
                            Amount++;
                        }
                    }
                    else
                    {
                        this.Amount++;
                        this.leftnode = new TreeTestStruct();
                        this.leftnode.key = val;
                        this.leftnode.Amount = 1;
                        return true;
                    }
                }
                else if (key < val)
                {
                    if (this.rightnode != null)
                    {
                        is_ok = this.rightnode.insert_rek(val);
                        if (is_ok)
                        {
                            Amount++;
                        }
                    }
                    else
                    {
                        this.Amount++;
                        this.rightnode = new TreeTestStruct();
                        this.rightnode.key = val;
                        this.rightnode.Amount = 1;
                        return true;
                    }
                }
                else if (key == val)
                {
                    return false;
                }
            }

            return is_ok;
        }

        public void Add(int val)
        {
            insert_rek(val);
        }

        public bool delete_rek(int val)
        {
            bool exists = false;

            if (Amount == -1)
            {
            } //dead end
            else if (key > val)
            {
                if (leftnode != null)
                {
                    if (leftnode.key != val)
                    {
                        exists = leftnode.delete_rek(val);

                        if (exists)
                        {
                            Amount--;
                        }

                        return exists;
                    }
                    else
                    {
                        this.Amount--;
                        if (leftnode.leftnode == null && leftnode.rightnode == null) //fall 1
                        {
                            this.leftnode = null;
                        }
                        else if (leftnode.leftnode != null && leftnode.rightnode == null) // fall 2
                        {
                            this.leftnode = leftnode.leftnode;
                        }
                        else if (leftnode.leftnode == null && leftnode.rightnode != null) // fall 2
                        {
                            this.leftnode = leftnode.rightnode;
                        }
                        else //fall 3
                        {
                            leftnode.Amount--;
                            TreeTestStruct help = leftnode.rightnode;
                            TreeTestStruct help2 = null;

                            while (help.leftnode != null)
                            {
                                help.Amount--;
                                help2 = help;
                                help = help.leftnode;
                            }

                            if (help2 == null) // es gab keinen linken konten
                            {
                                help.leftnode = this.leftnode.leftnode;
                                this.leftnode = help;
                            }
                            else
                            {
                                if (help.rightnode == null)
                                {
                                    help2.leftnode = null;
                                    help.leftnode = this.leftnode.leftnode;
                                    help.rightnode = this.leftnode.rightnode;
                                    help.Amount = this.leftnode.Amount;
                                    this.leftnode = help;
                                }
                                else
                                {
                                    help2.leftnode = help.rightnode;
                                    help.rightnode = this.leftnode.rightnode;
                                    help.leftnode = this.leftnode.leftnode;
                                    help.Amount = this.leftnode.Amount;
                                    this.leftnode = help;
                                }
                            }
                        }

                        return true;
                    }
                }
            }
            else if (key < val)
            {
                if (rightnode != null)
                {
                    if (rightnode.key != val)
                    {
                        exists = rightnode.delete_rek(val);
                        if (exists)
                        {
                            Amount--;
                        }

                        return exists;
                    }
                    else
                    {
                        this.Amount--;
                        if (rightnode.leftnode == null && rightnode.rightnode == null) //fall 1
                        {
                            this.rightnode = null;
                        }
                        else if (rightnode.leftnode != null && rightnode.rightnode == null) // fall 2
                        {
                            this.rightnode = rightnode.leftnode;
                        }
                        else if (rightnode.leftnode == null && rightnode.rightnode != null) // fall 2
                        {
                            this.rightnode = rightnode.rightnode;
                        }
                        else //fall 3
                        {
                            rightnode.Amount--;
                            TreeTestStruct help = rightnode.rightnode;
                            TreeTestStruct help2 = null;

                            while (help.leftnode != null)
                            {
                                help.Amount--;
                                help2 = help;
                                help = help.leftnode;
                            }

                            if (help2 == null) // es gibt keinen linken konten
                            {
                                help.leftnode = this.rightnode.leftnode;
                                this.rightnode = help;
                            }
                            else
                            {
                                if (help.rightnode == null)
                                {
                                    help2.leftnode = null;
                                    help.leftnode = this.rightnode.leftnode;
                                    help.rightnode = this.rightnode.rightnode;
                                    help.Amount = this.rightnode.Amount;
                                    this.rightnode = help;
                                }
                                else
                                {
                                    help2.leftnode = help.rightnode;
                                    help.rightnode = this.rightnode.rightnode;
                                    help.leftnode = this.rightnode.leftnode;
                                    help.Amount = this.rightnode.Amount;
                                    this.rightnode = help;
                                }
                            }
                        }

                        return true;
                    }
                }
            }
            else if (key == val)
            {

                if (leftnode == null && rightnode == null) //fall 1
                {
                    this.leftnode = null;
                    this.rightnode = null;
                    this.key = 0;
                    this.Amount = -1;
                }
                else if (leftnode != null && rightnode == null) // fall 2
                {
                    this.leftnode = leftnode.leftnode;
                    this.rightnode = leftnode.rightnode;
                    this.key = leftnode.key;
                    this.Amount = leftnode.Amount;
                }
                else if (leftnode == null && rightnode != null) // fall 2
                {
                    this.leftnode = rightnode.leftnode;
                    this.rightnode = rightnode.rightnode;
                    this.key = rightnode.key;
                    this.Amount = rightnode.Amount;
                }
                else //fall 3
                {
                    TreeTestStruct help = rightnode;
                    TreeTestStruct help2 = null;

                    while (help.leftnode != null)
                    {
                        help.Amount--;
                        help2 = help;
                        help = help.leftnode;
                    }

                    if (help2 == null) // es gibt keinen linken konten
                    {
                        help.leftnode = this.leftnode;
                        this.leftnode = help;

                        this.leftnode = help.leftnode;
                        this.rightnode = help.rightnode;
                        this.key = help.key;
                        this.Amount = help.Amount;
                    }
                    else
                    {
                        if (help.rightnode == null)
                        {
                            help2.leftnode = null;
                            help.leftnode = this.leftnode;
                            help.rightnode = this.rightnode;

                            this.leftnode = help.leftnode;
                            this.rightnode = help.rightnode;
                            this.key = help.key;
                            this.Amount = help.Amount;
                        }
                        else
                        {
                            help2.leftnode = help.rightnode;
                            help.rightnode = this.rightnode;
                            help.leftnode = this.leftnode;

                            this.leftnode = help.leftnode;
                            this.rightnode = help.rightnode;
                            this.key = help.key;
                            this.Amount = help.Amount;
                        }
                    }

                }
                return true;
            }

            return false;
        }

        /*
         Entfernt den Wert val und den dazugeh�origen Knoten
         aus dem Baum. Als Ersatzknoten (falls ben�otigt) wird der Knoten mit dem kleinsten
         Schl�ussel des rechten Unterbaumes (Successor) gew�ahlt. Falls val nicht im Baum
         enthalten ist, �andert sich der Baum nicht.
         */
        public void Remove(int val)
        {
            delete_rek(val);
        }

        /**
         * Liefert den Wert, der sich an der k-ten Stelle in der
         * Inorder-Durchmusterungsreihenfolge des Baumes befindet zur�uck. Das erste
         * Element steht an Position 0. Wird eine ung�ultige Position (negatives
         * oder zu gro�es k) �ubergeben, dann soll eine IllegalArgumentException mit
         * entsprechender Fehlermeldung geworfen werden.
         */
        public String Inorder(TreeTestStruct t)
        {
            String s = "";
            if (t != null)
            {
                s = Inorder(t.leftnode);
                s += t.key + ",";
                s += Inorder(t.rightnode);
            }

            return s;
        }

        private int[] Inorder(TreeTestStruct t, int k, int aktu)
        {
            int[] myArray = new int[2];
            myArray[0] = aktu;
            myArray[1] = 0;

            if (t != null && k >= 0)
            {
                if (t.leftnode != null)
                {
                    myArray = Inorder(t.leftnode, k, aktu);
                }

                if (myArray[1] == 1)
                {
                    return myArray;
                }

                aktu = myArray[0];

                if (aktu == k)
                {
                    myArray[0] = t.key;
                    myArray[1] = 1;
                    return myArray;
                }

                aktu += 1;
                myArray[0] = aktu;

                if (t.rightnode != null)
                {
                    myArray = Inorder(t.rightnode, k, aktu);
                }

                return (myArray);
            }

            throw new ArgumentException(" ");
            //return "ERROR";
        }

        public int IndexOf(int k)
        {
            int myorder = 0;
            try
            {
                myorder = Inorder(this, k, 0)[0];
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException("Value must be valid > 0 and < nodesize");
            }

            return myorder;
        }

        private int[] Inorder_pos(TreeTestStruct t, int k, int aktu)
        {
            int[] myArray = new int[2];
            myArray[0] = aktu;
            myArray[1] = 0;

            if (t != null)
            {
                if (t.leftnode != null)
                {
                    myArray = Inorder_pos(t.leftnode, k, aktu);
                }

                if (myArray[1] == 1)
                {
                    return myArray;
                }

                aktu = myArray[0];

                if (k <= t.key)
                {
                    myArray[1] = 1;
                    return myArray;
                }

                aktu += 1;
                myArray[0] = aktu;

                if (t.rightnode != null)
                {
                    myArray = Inorder_pos(t.rightnode, k, aktu);
                }

                return (myArray);
            }

            return myArray;
            //return "ERROR";
        }

        public int FindIndex(int val)
        {
            return Inorder_pos(this, val, 0)[0];
        }

    }
}
