using System.Collections.Generic;

namespace RQ.Editor.Common
{
    public static class ListFunctions
    {
        //private static void SwapListItems<T>(List<T> list, int v1, int v2) //where T : List<POINTS>
        //{
        //    var temp = list[v1];
        //    list[v1] = list[v2];
        //    list[v2] = temp;
        //    //Dirty();
        //}

        public static void Swap<TSource>(this IList<TSource> source, int v1, int v2) //where T : List<POINTS>
        {
            //source.
            //source[1];
            var temp = source[v1];
            source[v1] = source[v2];
            source[v2] = temp;
            //Dirty();
        }
    }
}
