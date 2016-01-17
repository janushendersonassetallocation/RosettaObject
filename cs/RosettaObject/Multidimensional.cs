using System.Collections.Generic;

namespace RosettaObject
{
    public class Multidimensional
    {
        public static bool IncIndex(int[] index, int[] shape)
        {
            int i = index.Length;
            while (--i >= 0)
            {
                if (++index[i] < shape[i])
                {
                    return true;
                }
                index[i] = 0;
            }
            return false;
        }

        public static bool DecIndex(int[] index, int[] shape)
        {
            int i = index.Length;
            while (--i >= 0)
            {
                if (index[i] > 0)
                {
                    --index[i];
                    return true;
                }
                index[i] = shape[i] - 1;
            }
            return false;
        }

        public static IEnumerable<int[]> EnumerateIndexes(int[] shape)
        {
            int[] index = new int[shape.Length];
            do
            {
                var item = new int[shape.Length];
                index.CopyTo(item, 0);
                yield return item;
            } while (IncIndex(index, shape));
        }
    }
}
