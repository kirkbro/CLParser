using System;

namespace CLParser
{
    internal static class ArrayExtensions
    {
        internal static T[] Slice<T>(this T[] array, int from) => array.Slice(from, array.Length);

        internal static T[] Slice<T>(this T[] array, int from, int to)
        {
            T[] slice = new T[to - from];
            Array.Copy(array, from, slice, 0, slice.Length);
            return slice;
        }

        internal static T[] Delete<T>(this T[] array, int from, int to)
        {
            T[] rest = new T[array.Length - (to - from)];
            Array.Copy(array, 0, rest, 0, from);
            Array.Copy(array, to, rest, from, array.Length - to);
            return rest;
        }
    }
}
