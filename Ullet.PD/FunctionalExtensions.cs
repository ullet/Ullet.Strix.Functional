/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ullet.PD
{
  /// <summary>
  /// Extension methods to support a more functional style.
  /// </summary>
  public static class FunctionalExtensions
  {
    /// <summary>
    /// Partially apply function with all parameters fixed.
    /// </summary>
    public static Func<TOut> Partial<TIn, TOut>(this Func<TIn, TOut> fn, TIn t)
    {
      return () => fn(t);
    }

    /// <summary>
    /// Partially apply function with first parameter fixed.
    /// </summary>
    /*
     * e.g. Func<int, int> plus = (x, y) => x + y
     *      Func<int> plus3 = plus.Partial(3)
     *      // plus3(4) = 4 + 3 = 7
     */
    public static Func<TIn2, TOut> Partial<TIn1, TIn2, TOut>(
      this Func<TIn1, TIn2, TOut> fn, TIn1 t1)
    {
      return t2 => fn(t1, t2);
    }

    /// <summary>
    /// Partially apply function with all parameter fixed.
    /// </summary>
    public static Func<TOut> Partial<TIn1, TIn2, TOut>(
      this Func<TIn1, TIn2, TOut> fn, TIn1 t1, TIn2 t2)
    {
      return () => fn(t1, t2);
    }

    /// <summary>
    /// Partially apply function with first parameter fixed.
    /// </summary>
    public static Func<TIn2, TIn3, TOut> Partial<TIn1, TIn2, TIn3, TOut>(
      this Func<TIn1, TIn2, TIn3, TOut> fn, TIn1 t1)
    {
      return (t2, t3) => fn(t1, t2, t3);
    }

    /// <summary>
    /// Partially apply function with first two parameters fixed.
    /// </summary>
    /*
     * e.g. Func<string, string, string, string> replace =
     *        (newValue, oldValue, input) => input.Replace(oldValue, newValue)
     *      Func<string, string> alterReality = replace.Partial("dogs", "cats")
     *      // alterReality("I like cats") -> "I like dogs"
     */
    public static Func<TIn3, TOut> Partial<TIn1, TIn2, TIn3, TOut>(
      this Func<TIn1, TIn2, TIn3, TOut> fn, TIn1 t1, TIn2 t2)
    {
      return t3 => fn(t1, t2, t3);
    }

    /// <summary>
    /// Partially apply function with all parameters fixed.
    /// </summary>
    public static Func<TOut> Partial<TIn1, TIn2, TIn3, TOut>(
      this Func<TIn1, TIn2, TIn3, TOut> fn, TIn1 t1, TIn2 t2, TIn3 t3)
    {
      return () => fn(t1, t2, t3);
    }

    /// <summary>
    /// Compose <paramref name="outer"/> function with
    /// <paramref name="inner"/> function.
    /// </summary>
    public static Func<TIn, TOut2> After<TIn, TOut1, TOut2>(
      this Func<TOut1, TOut2> outer, Func<TIn, TOut1> inner)
    {
      return t => outer(inner(t));
    }

    
    /// <summary>
    /// Compose enumerable of functions from right (inner) to left (outer).
    /// e.g. [f1, f2, f3] -> f1.f2.f3
    /// </summary>
    public static Func<T, T> Compose<T>(this IEnumerable<Func<T, T>> funcs)
    {
      return funcs.Aggregate((Func<T, T>)(t => t), After);
    }

    /// <summary>
    /// Compose enumerable of functions from left (inner) to right (outer).
    /// e.g. [f1, f2, f3] -> f3.f2.f1
    /// </summary>
    public static Func<T, T>
      ComposeReverse<T>(this IEnumerable<Func<T, T>> funcs)
    {
      return funcs.Reverse().Compose();
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but in
    /// reverse order.
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
     *   select = Enumerable.Select;
     * var selectSquares = select.Flip().Partial(x => x*x);
     * var squares = selectSquares(new[] {1, 2, 3}); //=> enumerable [1, 4, 9]
     */
    public static Func<TIn2, TIn1, TOut> Flip<TIn1, TIn2, TOut>(
      this Func<TIn1, TIn2, TOut> fn)
    {
      return (t2, t1) => fn(t1, t2);
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but in
    /// reverse order.
    /// </summary>
    public static Func<TIn3, TIn2, TIn1, TOut> Flip<TIn1, TIn2, TIn3, TOut>(
      this Func<TIn1, TIn2, TIn3, TOut> fn)
    {
      return (t3, t2, t1) => fn(t1, t2, t3);
    }
  }
}
