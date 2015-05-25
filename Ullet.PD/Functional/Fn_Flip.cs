/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;

namespace Ullet.PD.Functional
{
  public static partial class Fn
  {
    /// <summary>
    /// Construct a new function taking same parameters as original but with 
    /// order of first two parameters reversed.
    /// </summary>
    /*
     * Many C# methods, particularly Linq extension methods, are not well
     * suited to partial application because the first parameter is usually the
     * one that typically want to leave unbound.  For example the
     * Enumerable.Select method takes the list to enumerate as the first
     * parameter and the function to execute on the list as the second.
     * Flipping the parameter order allow partial application by fixing the 
     * function and leaving the list unbound.
     * e.g.
     * Func<IEnumerable<int>, Func<int, int>, IEnumerable<int>> 
     *   select = System.Linq.Enumerable.Select;
     * var selectSquares = select.Flip().Partial(x => x*x);
     * var squares = selectSquares(new[] {1, 2, 3}); //=> enumerable [1, 4, 9]
     */
    public static Func<T2, T1, TOut> Flip<T1, T2, TOut>(
      this Func<T1, T2, TOut> fn)
    {
      return (t2, t1) => fn(t1, t2);
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but with 
    /// order of first two parameters reversed.
    /// </summary>
    public static Func<T2, T1, T3, TOut> Flip<T1, T2, T3, TOut>(
      this Func<T1, T2, T3, TOut> fn)
    {
      return (t2, t1, t3) => fn(t1, t2, t3);
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but with 
    /// order of first two parameters reversed.
    /// </summary>
    public static Func<T2, T1, T3, T4, TOut> Flip<T1, T2, T3, T4, TOut>(
      this Func<T1, T2, T3, T4, TOut> fn)
    {
      return (t2, t1, t3, t4) => fn(t1, t2, t3, t4);
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but with 
    /// order of first two parameters reversed.
    /// </summary>
    public static Func<T2, T1, T3, T4, T5, TOut>
      Flip<T1, T2, T3, T4, T5, TOut>(
      this Func<T1, T2, T3, T4, T5, TOut> fn)
    {
      return (t2, t1, t3, t4, t5) => fn(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but in
    /// reverse order.
    /// </summary>
    public static Func<T2, T1, TOut> FlipAll<T1, T2, TOut>(
      this Func<T1, T2, TOut> fn)
    {
      // FlipAll same as Flip [first two] for only two parameters.
      return fn.Flip();
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but in
    /// reverse order.
    /// </summary>
    public static Func<T3, T2, T1, TOut> FlipAll<T1, T2, T3, TOut>(
      this Func<T1, T2, T3, TOut> fn)
    {
      return (t3, t2, t1) => fn(t1, t2, t3);
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but in
    /// reverse order.
    /// </summary>
    public static Func<T4, T3, T2, T1, TOut> FlipAll<T1, T2, T3, T4, TOut>(
      this Func<T1, T2, T3, T4, TOut> fn)
    {
      return (t4, t3, t2, t1) => fn(t1, t2, t3, t4);
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but in
    /// reverse order.
    /// </summary>
    public static Func<T5, T4, T3, T2, T1, TOut>
      FlipAll<T1, T2, T3, T4, T5, TOut>(this Func<T1, T2, T3, T4, T5, TOut> fn)
    {
      return (t5, t4, t3, t2, t1) => fn(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Construct a new curried function taking same parameters as original
    /// curried function but with order of first two parameters reversed.
    /// </summary>
    public static Func<T2, Func<T1, TOut>> Flip<T1, T2, TOut>(
      this Func<T1, Func<T2, TOut>> fn)
    {
      return t2 => t1 => fn(t1)(t2);
    }
  }
}
