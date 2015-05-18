/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ullet.PD.Functional
{
  /// <summary>
  /// Extension methods to support a more functional style.
  /// </summary>
  public static class FunctionalExtensions
  {
    /// <summary>
    /// Partially apply function with all parameters fixed.
    /// </summary>
    public static Func<TOut> Partial<T, TOut>(this Func<T, TOut> fn, T t)
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
    public static Func<T2, TOut> Partial<T1, T2, TOut>(
      this Func<T1, T2, TOut> fn, T1 t1)
    {
      return t2 => fn(t1, t2);
    }

    /// <summary>
    /// Partially apply function with all parameters fixed.
    /// </summary>
    public static Func<TOut> Partial<T1, T2, TOut>(
      this Func<T1, T2, TOut> fn, T1 t1, T2 t2)
    {
      return () => fn(t1, t2);
    }

    /// <summary>
    /// Partially apply function with first parameter fixed.
    /// </summary>
    public static Func<T2, T3, TOut> Partial<T1, T2, T3, TOut>(
      this Func<T1, T2, T3, TOut> fn, T1 t1)
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
    public static Func<T3, TOut> Partial<T1, T2, T3, TOut>(
      this Func<T1, T2, T3, TOut> fn, T1 t1, T2 t2)
    {
      return t3 => fn(t1, t2, t3);
    }

    /// <summary>
    /// Partially apply function with all parameters fixed.
    /// </summary>
    public static Func<TOut> Partial<T1, T2, T3, TOut>(
      this Func<T1, T2, T3, TOut> fn, T1 t1, T2 t2, T3 t3)
    {
      return () => fn(t1, t2, t3);
    }

    /// <summary>
    /// Partially apply action with all parameters fixed.
    /// </summary>
    public static Action Partial<T>(this Action<T> action, T t)
    {
      return () => action(t);
    }

    /// <summary>
    /// Partially apply action with first parameter fixed.
    /// </summary>
    public static Action<T2> Partial<T1, T2>(this Action<T1, T2> action, T1 t1)
    {
      return t2 => action(t1, t2);
    }

    /// <summary>
    /// Partially apply action with all parameters fixed.
    /// </summary>
    public static Action Partial<T1, T2>(
      this Action<T1, T2> action, T1 t1, T2 t2)
    {
      return () => action(t1, t2);
    }

    /// <summary>
    /// Partially apply action with first parameter fixed.
    /// </summary>
    public static Action<T2, T3> Partial<T1, T2, T3>(
      this Action<T1, T2, T3> action, T1 t1)
    {
      return (t2, t3) => action(t1, t2, t3);
    }

    /// <summary>
    /// Partially apply action with first two parameters fixed.
    /// </summary>
    public static Action<T3> Partial<T1, T2, T3>(
      this Action<T1, T2, T3> action, T1 t1, T2 t2)
    {
      return t3 => action(t1, t2, t3);
    }

    /// <summary>
    /// Partially apply action with all parameters fixed.
    /// </summary>
    public static Action Partial<T1, T2, T3>(
      this Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
      return () => action(t1, t2, t3);
    }

    /// <summary>
    /// Compose <paramref name="outer"/> function with
    /// <paramref name="inner"/> function.
    /// </summary>
    public static Func<T, TOut2> After<T, TOut1, TOut2>(
      this Func<TOut1, TOut2> outer, Func<T, TOut1> inner)
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
    public static Func<T2, T1, TOut> Flip<T1, T2, TOut>(
      this Func<T1, T2, TOut> fn)
    {
      return (t2, t1) => fn(t1, t2);
    }

    /// <summary>
    /// Construct a new function taking same parameters as original but in
    /// reverse order.
    /// </summary>
    public static Func<T3, T2, T1, TOut> Flip<T1, T2, T3, TOut>(
      this Func<T1, T2, T3, TOut> fn)
    {
      return (t3, t2, t1) => fn(t1, t2, t3);
    }

    /// <summary>
    /// Convert function to curried form.
    /// </summary>
    public static Func<T1, Func<T2, TOut>> Curry<T1, T2, TOut>(
      this Func<T1, T2, TOut> fn)
    {
      return t1 => t2 => fn(t1, t2);
    }

    /// <summary>
    /// Convert function to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, TOut>>>
      Curry<T1, T2, T3, TOut>(this Func<T1, T2, T3, TOut> fn)
    {
      return t1 => t2 => t3 => fn(t1, t2, t3);
    }

    /// <summary>
    /// Convert function to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, Func<T4, TOut>>>>
      Curry<T1, T2, T3, T4, TOut>(this Func<T1, T2, T3, T4, TOut> fn)
    {
      return t1 => t2 => t3 => t4 => fn(t1, t2, t3, t4);
    }

    /// <summary>
    /// Convert function to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TOut>>>>>
      Curry<T1, T2, T3, T4, T5, TOut>(this Func<T1, T2, T3, T4, T5, TOut> fn)
    {
      return t1 => t2 => t3 => t4 => t5 => fn(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Convert curried function to non-curried form.
    /// </summary>
    public static Func<T1, T2, TOut> Uncurry<T1, T2, TOut>(
      this Func<T1, Func<T2, TOut>> fn)
    {
      return (t1, t2) => fn(t1)(t2);
    }

    /// <summary>
    /// Convert curried function to non-curried form.
    /// </summary>
    public static Func<T1, T2, T3, TOut> Uncurry<T1, T2, T3, TOut>(
      this Func<T1, Func<T2, Func<T3, TOut>>> fn)
    {
      return (t1, t2, t3) => fn(t1)(t2)(t3);
    }

    /// <summary>
    /// Convert curried function to non-curried form.
    /// </summary>
    public static Func<T1, T2, T3, T4, TOut> Uncurry<T1, T2, T3, T4, TOut>(
      this Func<T1, Func<T2, Func<T3, Func<T4, TOut>>>> fn)
    {
      return (t1, t2, t3, t4) => fn(t1)(t2)(t3)(t4);
    }

    /// <summary>
    /// Convert curried function to non-curried form.
    /// </summary>
    public static Func<T1, T2, T3, T4, T5, TOut>
      Uncurry<T1, T2, T3, T4, T5, TOut>(
      this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TOut>>>>> fn)
    {
      return (t1, t2, t3, t4, t5) => fn(t1)(t2)(t3)(t4)(t5);
    }
  }
}
