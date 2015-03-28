using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZitaAsteria
{
    /// <summary>
    /// Useful extension methods for Lists. [Alucard]
    /// WARNING : Validation checks are not performed.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Removes the first occurrence of the specified element from this list.
        /// </summary>
        /// <param name="source">Source list</param>
        /// <param name="o">Element to be removed</param>
        /// <returns>True if element removed</returns>
        public static bool FastRemove<T>(this List<T> source, T o)
        {
            int i = source.IndexOf(o);

            if (i != -1)
            {
                source[i] = source[(source.Count - 1)];
                source.RemoveAt(source.Count - 1);
                return true;
            }

            return false;
        }

        /// <summary>
        /// WARNING : Code not tested! [Alucard]
        /// Removes the first occurrence of each specified element from the list.
        /// </summary>
        /// <param name="source">Source list</param>
        /// <param name="c">Elements to remove</param>
        /// <returns>True if source list has been modified</returns>
        public static bool FastRemoveAll<T>(this List<T> source, List<T> c)
        {
            bool modified = false;
            int lastElement = (source.Count - 1);

            for (int i = 0; i < c.Count; i++)
            {
                for (int j = 0; j < source.Count; j++)
                {
                    if (c[i].Equals(source[j]))
                    {
                        if (j != (lastElement))
                        {
                            source[j] = source[(lastElement)];
                            source.RemoveAt(lastElement);
                            lastElement--;
                        }
                        else
                        {
                            source.RemoveAt(lastElement);
                            lastElement--;
                        }

                        modified = true;

                        break;
                    }
                }
            }

            return modified;
        }

        /// <summary>
        /// Removes and returns the element at the specified index in this list.
        /// </summary>
        /// <param name="source">Source list</param>
        /// <param name="index">Index of element</param>
        /// <returns>Element at specified index</returns>
        public static T FastRemoveAt<T>(this List<T> source, int index)
        {
            T o1 = source[index];
            source[index] = source[(source.Count - 1)];
            source.RemoveAt(source.Count - 1);
            return o1;
        }

        /// <summary>
        /// Removes and returns the the last element in this list.
        /// </summary>
        /// <param name="source">Source list</param>
        /// <returns>Element at specified index</returns>
        public static T FastRemoveLast<T>(this List<T> source)
        {
            T o1 = source[(source.Count - 1)];
            source.RemoveAt(source.Count - 1);
            return o1;
        }

        /// <summary>
        /// Removes all elements in the specified range from this list.
        /// </summary>
        /// <param name="source">Source list</param>
        /// <param name="index">Index of element to start removal at [including specified element]</param>
        /// <param name="count">Number of elements to remove</param>
        public static void FastRemoveRange<T>(this List<T> source, int index, int count)
        {
            for (int i = 0; i <= count; i++)
            {
                source[index] = source[(source.Count - 1)];
                source.RemoveAt(source.Count - 1);
                index++;
            }
        }

        /// <summary>
        /// Checks if list is empty.
        /// </summary>
        /// <param name="source">Source list</param>
        /// <returns>True if list is empty</returns>
        public static bool IsEmpty<T>(this List<T> source)
        {
            return source.Count == 0;
        }
    }
}
