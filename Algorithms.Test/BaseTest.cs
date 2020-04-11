using DataStructures;
using System;
using System.Collections.Generic;

namespace Algorithms.Test
{
    public class BaseTest
    {
        public IList<int> RandomList<T>() where T : IComparable<T>
        {
            Random rand = new Random();
            List<int> result = new List<int>();
            HashSet<int> check = new HashSet<int>();
            for (Int32 i = 0; i < 300; i++)
            {
                int curValue = rand.Next(1, 100000);
                while (check.Contains(curValue))
                {
                    curValue = rand.Next(1, 100000);
                }
                result.Add(curValue);
                check.Add(curValue);
            }
            return result;
        }

    }
}
