using System;
using System.Collections.Generic;
using System.Text;

namespace Nimboid
{
    static class Utils
    {

        static public String ConcatArray(object[] paramarray, int from, int to = -1)
        {
            if (to == -1) to = paramarray.Length - 1;

            object[] array = new object[to - from + 1];
            System.Array.ConstrainedCopy(paramarray, from, array, 0, array.Length);

            return String.Concat(array);

            

        }

    }
}
