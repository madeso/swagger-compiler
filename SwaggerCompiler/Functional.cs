namespace SwaggerCompiler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary> Class with functional like functions </summary>
    public static class Functional
    {
        /// <summary>
        /// Keeps items in the enumerable. 
        /// </summary>
        /// <typeparam name="T">
        /// The enumeration type 
        /// </typeparam>
        /// <param name="e">
        /// The enumeration 
        /// </param>
        /// <param name="isValid">
        /// callback that returns true if the item should be kept 
        /// </param>
        /// <returns>
        /// The resulting items 
        /// </returns>
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> e, Func<T, bool> isValid)
        {
            return e.Where( isValid );
        }

        /// <summary>
        /// Convert item from one type to another 
        /// </summary>
        /// <typeparam name="TReturn">
        /// The type of the output 
        /// </typeparam>
        /// <typeparam name="TArgument">
        /// The type of the argument 
        /// </typeparam>
        /// <param name="e">
        /// The actual input 
        /// </param>
        /// <param name="c">
        /// converter function for converting from Argument to Return 
        /// </param>
        /// <returns>
        /// The converted items 
        /// </returns>
        public static IEnumerable<TReturn> Map<TReturn, TArgument>(this IEnumerable<TArgument> e, Func<TArgument, TReturn> c)
        {
            return e.Select( c );
        }

        /// <summary>
        /// Convert item from object to another 
        /// </summary>
        /// <typeparam name="TReturn">
        /// The type of the output 
        /// </typeparam>
        /// <param name="e">
        /// The actual input 
        /// </param>
        /// <param name="c">
        /// converter function for converting from object to Return 
        /// </param>
        /// <returns>
        /// The converted items 
        /// </returns>
        public static IEnumerable<TReturn> Map<TReturn>(this IEnumerable e, Func<object, TReturn> c )
        {
            return from object a in e
                   select c( a );
        }

        /// <summary>
        /// Gets the first items
        /// </summary>
        /// <typeparam name="T">The type of the input</typeparam>
        /// <param name="e">The input</param>
        /// <param name="count">Maximum number of items</param>
        /// <param name="last">the item to end with if count is exceeded</param>
        /// <returns>the first items</returns>
        public static IEnumerable<T> First<T>(this IEnumerable<T> e, int count, T last)
        {
            var index = 0;
            foreach (var t in e)
            {
                ++index;
                if (index > count)
                {
                    yield return last;
                    yield break;
                }

                yield return t;
            }
        }
    }
}
