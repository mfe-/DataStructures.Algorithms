using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Single Node Interfaces
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <typeparam name="T"></typeparam>
    public interface ISingleNode<D, T> : ICovariant<T>, IData<D>
        where T : ISingleNode<D, T>
    {
        T GetRight();
        void SetRight(T t);
        //nicht erlaubt ist ein generischer typ mit out (out darf nur zurück gegeben werden) der ein object returned auch zu setzen siehe auch http://stackoverflow.com/questions/2541467/why-does-c-sharp-4-0-not-allow-co-and-contravariance-in-generic-class-types
        // out erlaubt nur refernez typen (also keine int, string usw.)


        //zuerst t mit out versehen und dann set right auf interface abändern. wenn das nicht geht typ T der ja ISingleNode entspricht durch interface ersetzen sodass nur noch ein generischer typ für den datentyp benötigt wird
    }
}
