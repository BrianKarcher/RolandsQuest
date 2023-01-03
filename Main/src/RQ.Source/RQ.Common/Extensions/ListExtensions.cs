using System.Collections.Generic;
using RQ.Messaging;
using RQ.Serialization;
using UnityEngine;
namespace RQ.Extensions
{
    public static class ListExtensions
    {
        public static T FirstOrDefault<T>(this IList<T> list) where T : class
        {
            if (list == null || list.Count == 0)
                return null;
            return list[0];
            //return (obj ?? string.Empty).ToString();
        }

        public static T[] RemoveAt<T>(this T[] indicesArray, int removeAt)
        {
            T[] newIndicesArray = new T[indicesArray.Length - 1];

            int i = 0;
            int j = 0;
            while (i < indicesArray.Length)
            {
                if (i != removeAt)
                {
                    newIndicesArray[j] = indicesArray[i];
                    j++;
                }

                i++;
            }
            indicesArray = newIndicesArray;
            return newIndicesArray;
        }

        //public static T Contains<T>(this T[] list)
        //{
        //    if (list == null || list.Length == 0)
        //        return default(T);
        //    for (int i = 0; i < )
        //    //return list[0];
        //    //return (obj ?? string.Empty).ToString();
        //}
    }
}
