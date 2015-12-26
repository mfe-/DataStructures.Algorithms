using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure.Test
{
    /// <summary>
    /// Use this class to test tree strucutres
    /// Implemention was validated in the lva aglodat ss15, and executes the test instanzen successfull
    /// </summary>
    /// <remarks>
    /// The use of array to implement the tree strucutre is not recommended and should be solved in an other way.
    /// </remarks>
    public class TreeSet : ITree<int>
    {
        /**
     * save our root node
     */
    public Node Root;

    /**
     * Liefert zuruck, ob der Baum leer ist.
     *
     * @return
     */
    public bool isEmpty() {
        return Root == null;
    }
    private int Size = 0;

    /**
     * Liefert die GroÃŸe in Form der Anzahl der Knoten des Baumes zur Â¨ uck.
     *
     * @return
     */
    public int size() {
        return Size;
    }

    /**
     * Liefert zuruck, ob der Wert Â¨ val im Baum abgespeichert ist.
     *
     * @param val
     * @return
     */
    public bool exists(int val) {
        return getNode(val) != null;
    }

    /**
     * Returns
     *
     * @param val
     * @return
     */
    public Node getNode(int val) {
        Node p = this.Root;

        while (p != null /*&& p.Key != val*/) {
            if (val < p.Key) {
                p = p.Left;
            } else if (p.Key == val) {
                return p;
            } else {
                p = p.Right;
            }
        }

        return null;
    }

    /**
     * Liefert die Hohe des Baumes zur Â¨ uck. Die H Â¨ ohe wird f Â¨ ur diese Auf-
     * Â¨ gabe folgendermaÃŸen definiert: Ein leerer Baum hat die Hohe -1, ein
     * Baum mit einem Â¨ Knoten die Hohe 0. Mit jeder Stufe von Nachfolgern erh Â¨
     * oht sich die H Â¨ ohe um 1.
     *
     * @return
     */
    public int height() {
        return height(this.Root);
    }

    private int height(Node node) {
        if (node == null) {
            return -1;
        }
        return 1 + (Math.Max(height(node.Left), height(node.Right)));
    }

    /**
     * Fugt den Â¨ ubergebenen Wert Â¨ val in den Baum ein. Falls der Wert schon
     * vorhanden ist, dann wird er nicht neu eingefugt. Ein Wert kann daher Â¨
     * nur einmal im Baum vorkommen.
     *
     * @param val
     */
    public void insert(int val) {
        //einfÃ¼gen skriptum
//        if (this.exists(val)) {
//            return;
//        }

        Node q = new Node(val);
        Node r = null; //r wird vorgÃ¤nger von q
        Node p = this.Root;

        while (p != null) {
            r = p;
            if (q.Key < p.Key) {
                p = p.Left;
            } else if (p.Key == val) {
                return; //if key already exists
            } else {
                p = p.Right; //gleiche SchlÃ¼ssel kommen nach rechts
            }
        }
        q.Parent = r;
        q.Left = null;
        q.Right = null;

        if (r == null) {
            this.Root = q;
        } else {
            if (q.Key < r.Key) {
                r.Left = q;
            } else {
                r.Right = q;
            }
        }
        //increase size of tree;
        Size = Size + 1;
        
        //TODO Save amount of subtree in node

    }

    /**
     * : Entfernt den Wert val und den dazugehorigen Knoten Â¨ aus dem Baum. Als
     * Ersatzknoten (falls benotigt) wird der Knoten mit dem kleinsten Â¨
     * Schlussel des rechten Unterbaumes (Successor) gew Â¨ ahlt. Falls Â¨ val
     * nicht im Baum enthalten ist, andert sich der Baum nicht. O(h(T))
     *
     * @param val
     */
    public void delete(int val) {
        if (isEmpty()) {
            return;
        }
        Node r = null;
        Node root = this.Root;
        Node q = getNode(val);
        Node p = null;

        if (q.Left == null || q.Right == null) {   //q hat max 1 NaChfolger --> wird selbst entfernt
            r = q;
        } else {
            //q hat 2 NAcfolger -> wird durch successor ersetzt, dieser wird entfernt
            r = Successor(q);
            //umhÃ¤ngen der daten von r nach q
            q.Key = r.Key;
        }
        //lasse p auf kind von r verweisen (p=null, falls r keine kinder hat)
        if (r.Left != null) {
            p = r.Left;
        } else {
            p = r.Right;
        }
        if (p != null) {
            p.Parent = r.Parent;
            //erzeuge einen verweis von p auf seinen neuen vorgÃ¤nger ( den vorgÃ¤nger von r)
        }
        //hÃ¤nge p anstelle von r in den Baum ein
        if (r.Parent == null) {
            //r war Wurzel: neue wurzel ist p
            this.Root = p;
        } else {
            //hÃ¤nge p an der richtigen seite des vorgÃ¤ngerknotens von r ein
            if (r == r.Parent.Left) {
                r.Parent.Left = p; //p sei linker nachfolger
            } else {
                r.Parent.Right = p;
            }
        }
        r = null;

        Size = Size - 1;
        //TODO Save amount of subtree in node
    }


    /**
     * Nacfolger von Knoten p!=null in der Inorder-Durchmusterungsreihenfolge
     *
     * @param p
     * @return
     */
    public Node Successor(Node p) {
        Node q = null;
        if (p.Right != null) {
            return Minimum(p.Right);
        } else {
            q = p.Parent;
            while (q != null && p == q.Right) {
                p = q;
                q = q.Parent;
            }
            return q;
        }
    }

    public Node Minimum(Node p) {
        if (p == null) {
            return null;
        }
        while (p.Left != null) {
            p = p.Left;
        }
        return p;
    }

    /**
     * Liefert den Wert, der sich an der k-ten Stelle in der
     * Inorder-Durchmusterungsreihenfolge des Baumes befindet zuruck. Das erste
     * Ele- Â¨ ment steht an Position 0. Wird eine ungultige Position (negatives
     * oder zu groÃŸes Â¨ k) ubergeben, dann soll eine IllegalArgumentException
     * mit entsprechender Fehlermel- Â¨ dung geworfen werden.
     *
     * @param k
     * @return
     */
    private List<Node> listInOrderCached = new List<Node>(); //        //TODO remove this - not needed when implementing with amount of subtrees

    public int valueAtPosition(int k) {
        if (k > Size) {
            throw new ArgumentException((k.ToString()));
        }

        if (listInOrderCached == null || listInOrderCached.Count() != size()) {
            listInOrderCached.Clear();
            Stack<Node> stack = new Stack<Node>();
            Node current = this.Root;
            while (current != null || !(stack.Count()==0)) {
                if (current != null) {
                    stack.Push(current);
                    current = current.Left;
                } else if (!(stack.Count()==0)) {
                    current = stack.Pop();
                    listInOrderCached.Add(current);
                    current = current.Right;
                }
            }
        }
        return listInOrderCached[(k)].Key;
    }

    /**
     *
     * @param p
     * @param list
     * @return
     */
    public List<Node> InOrder(Node p, List<Node> list) {
        //inorder skriptum
        if (p != null) {
            InOrder(p.Left, list);
            list.Add(p);
            InOrder(p.Right, list);
        }
        return list;
    }

    /**
     * Liefert die Position des Wertes val in der InorderReihenfolge aller Werte
     * im Baum zuruck. Wiederum gilt f Â¨ ur diese Aufgabe, dass das Â¨ erste
     * Element an Position 0 steht. Falls das Element nicht im Baum vorhanden
     * ist, dann soll jene Position zuruckgeliefert werden an der es eingef Â¨
     * ugt werden w Â¨ urde
     *
     * @param val
     * @return
     */
    public int position(int val) {
        if (isEmpty()) {
            return 0;
        }
        
        //TODO use amount of subtree to calculate position
        
        if (listInOrderCached == null || listInOrderCached.Count() != size()) {
            listInOrderCached.Clear();
            Stack<Node> stack;
            stack = new Stack<Node>();
            Node current = this.Root;
            while (current != null || !(stack.Count()==0)) {
                if (current != null) {
                    stack.Push(current);
                    current = current.Left;
                } else if (!(stack.Count()==0)) {
                    current = stack.Pop();
                    listInOrderCached.Add(current);
                    current = current.Right;
                }
            }
        }
        List<Node> list = listInOrderCached;
        int l = 0;
        int h = list.Count() - 1;
        //binary search skriptum
        while (l <= h) {
            int m = (l + h) / 2;
            if (val > list[(m)].Key) {
                l = m + 1;
            } else if (val < list[(m)].Key) {
                h = m - 1;
            } else {
                return m;
            }
        }
        return l;
    }

    /**
     * Liefert alle Werte zwischen lo (inklusive) und hi (inklusive) zuruck.
     * Falls Â¨ lo > hi, dann soll die Methode auch korrekt funktionieren, d.h.
     * es werden alle Werte groÃŸer oder gleich Â¨ lo oder kleiner oder gleich hi
     * zuruckgeliefert. Nur f Â¨ ur diese Aufgabe d Â¨ urfen Sie eine zus atzliche
     * Â¨ Datenstruktur aus dem Java-Collection-Framework verwenden. Es sollte
     * moglich sein, Â¨ uber diese Datenstruktur zu iterieren (z.B. Liste) und
     * alle Werte auszugeben.
     */
    public IEnumerable<int> values(int lo, int hi) {
        if (lo > hi) {
            return values(lo, hi, this.Root, new List<int>(), true);
        } else {
            return values(lo, hi, this.Root, new List<int>(), false);
        }
    }

    public IEnumerable<int> values(int lo, int hi, Node p, List<int> list, bool revert) {
        if (p != null) {
            values(lo, hi, p.Left, list, revert);
            if (revert == false) {
                if (p.Key >= lo && p.Key <= hi) {
                    list.Add(p.Key);
                }
            } else {
                //
                if (p.Key <= hi) {
                    list.Add(p.Key);
                } else if (p.Key >= lo) {
                    list.Add(p.Key);
                }
            }
            values(lo, hi, p.Right, list, revert);
        }
        return list;
    }

    /**
     * Wenn diese Methode aufgerufen wird, dann wird der gesamte Baum
     * balanciert, sodass die Hohendifferenz zwischen allen Bl Â¨ attern maximal
     * Â¨ eins ist. Dazu durfen Sie aber keine Rotationsoperationen verwenden,
     * sondern sollen Â¨ sich ein Verfahren uberlegen, das die Methode Â¨
     * KeyAtPosition ausnutzt und damit zunachst einen zweiten Baum aufbaut.
     * Dieser ersetzt letztlich den urspr Â¨ unglichen Â¨ Baum. Hinweis: Bedenken
     * Sie dabei, dass die Werte im ursprunglichen Suchbaum aufstei- Â¨ gend
     * gespeichert werden. Wenn man von allen Werten den Median ermittelt und
     * diesen als Wurzel des neuen Baumes wahlt, dann hat man f Â¨ ur den linken
     * und den rechten Â¨ Teilbaum dieser Wurzel in etwa gleich viele Werte zur
     * Verfugung
     */
    public void simpleBalance() {
        TreeSet balancedTree = new TreeSet();
        simpleBalance(balancedTree, 0, size() - 1);
        this.Root = balancedTree.Root;
    }

    private void simpleBalance(TreeSet balancedTree, int start, int end)
    {
        //wir holen aus inorder array root node Ã¼ber valueatposition und bauen so schritt fÃ¼r schritt den neuen tree
        int mid = (start + end) / 2;
        balancedTree.insert(valueAtPosition(mid));
        if (mid == end) {
            return;
        }
        simpleBalance(balancedTree, mid + 1, end);
        if (mid == 0) {
            return;
        }
        simpleBalance(balancedTree, start, mid - 1);
    }

    /**
     * Represents a node node
     */
    public class Node {

        public Node(int val) {
            this.Key = val;
        }
        public int Key;
        public Node Left;
        public Node Right;
        public Node Parent;
        public int AmountSubTree;


    }

    public bool Empty
    {
        get { return isEmpty(); }
    }

    public int Length
    {
        get { return size(); }
    }

    public int Height
    {
        get { return height(); }
    }

    public int FindIndex(int k)
    {
        return position(k);
    }

    public int IndexOf(int t)
    {
        return valueAtPosition(t);
    }

    public void Add(int t)
    {
        insert(t);
    }

    public void Remove(int val)
    {
        delete(val);
    }

    public bool Exists(int val)
    {
        return exists(val);
    }

    public void Clear()
    {
        this.Root = null;
    }

    public void CopyTo(int[] array, int index)
    {
        array = InOrder(this.Root, new List<Node>()).Select(a=>a.Key).ToArray<int>();
    }
    }
}
