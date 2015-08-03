/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;

namespace Ullet.Strix.Functional
{
  /// <summary>
  /// Extension methods to support a more functional style.
  /// </summary>
  public static partial class Fn
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
    /// Partially apply function with first parameter fixed.
    /// </summary>
    public static Func<T2, T3, T4, TOut> Partial<T1, T2, T3, T4, TOut>(
      this Func<T1, T2, T3, T4, TOut> fn, T1 t1)
    {
      return (t2, t3, t4) => fn(t1, t2, t3, t4);
    }

    /// <summary>
    /// Partially apply function with first two parameters fixed.
    /// </summary>
    public static Func<T3, T4, TOut> Partial<T1, T2, T3, T4, TOut>(
      this Func<T1, T2, T3, T4, TOut> fn, T1 t1, T2 t2)
    {
      return (t3, t4) => fn(t1, t2, t3, t4);
    }

    /// <summary>
    /// Partially apply function with first three parameters fixed.
    /// </summary>
    public static Func<T4, TOut> Partial<T1, T2, T3, T4, TOut>(
      this Func<T1, T2, T3, T4, TOut> fn, T1 t1, T2 t2, T3 t3)
    {
      return t4 => fn(t1, t2, t3, t4);
    }

    /// <summary>
    /// Partially apply function with all parameters fixed.
    /// </summary>
    public static Func<TOut> Partial<T1, T2, T3, T4, TOut>(
      this Func<T1, T2, T3, T4, TOut> fn, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      return () => fn(t1, t2, t3, t4);
    }

    /// <summary>
    /// Partially apply function with first parameter fixed.
    /// </summary>
    public static Func<T2, T3, T4, T5, TOut> Partial<T1, T2, T3, T4, T5, TOut>(
      this Func<T1, T2, T3, T4, T5, TOut> fn, T1 t1)
    {
      return (t2, t3, t4, t5) => fn(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Partially apply function with first two parameters fixed.
    /// </summary>
    public static Func<T3, T4, T5, TOut> Partial<T1, T2, T3, T4, T5, TOut>(
      this Func<T1, T2, T3, T4, T5, TOut> fn, T1 t1, T2 t2)
    {
      return (t3, t4, t5) => fn(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Partially apply function with first three parameters fixed.
    /// </summary>
    public static Func<T4, T5, TOut> Partial<T1, T2, T3, T4, T5, TOut>(
      this Func<T1, T2, T3, T4, T5, TOut> fn, T1 t1, T2 t2, T3 t3)
    {
      return (t4, t5) => fn(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Partially apply function with first four parameters fixed.
    /// </summary>
    public static Func<T5, TOut> Partial<T1, T2, T3, T4, T5, TOut>(
      this Func<T1, T2, T3, T4, T5, TOut> fn, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      return t5=> fn(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Partially apply function with all parameters fixed.
    /// </summary>
    public static Func<TOut> Partial<T1, T2, T3, T4, T5, TOut>(
      this Func<T1, T2, T3, T4, T5, TOut> fn, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      return () => fn(t1, t2, t3, t4, t5);
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
    /// Partially apply action with first parameter fixed.
    /// </summary>
    public static Action<T2, T3, T4> Partial<T1, T2, T3, T4>(
      this Action<T1, T2, T3, T4> action, T1 t1)
    {
      return (t2, t3, t4) => action(t1, t2, t3, t4);
    }

    /// <summary>
    /// Partially apply action with first two parameters fixed.
    /// </summary>
    public static Action<T3, T4> Partial<T1, T2, T3, T4>(
      this Action<T1, T2, T3, T4> action, T1 t1, T2 t2)
    {
      return (t3, t4) => action(t1, t2, t3, t4);
    }

    /// <summary>
    /// Partially apply action with first three parameters fixed.
    /// </summary>
    public static Action<T4> Partial<T1, T2, T3, T4>(
      this Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3)
    {
      return t4 => action(t1, t2, t3, t4);
    }

    /// <summary>
    /// Partially apply action with all parameters fixed.
    /// </summary>
    public static Action Partial<T1, T2, T3, T4>(
      this Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      return () => action(t1, t2, t3, t4);
    }

    /// <summary>
    /// Partially apply action with first parameter fixed.
    /// </summary>
    public static Action<T2, T3, T4, T5> Partial<T1, T2, T3, T4, T5>(
      this Action<T1, T2, T3, T4, T5> action, T1 t1)
    {
      return (t2, t3, t4, t5) => action(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Partially apply action with first two parameters fixed.
    /// </summary>
    public static Action<T3, T4, T5> Partial<T1, T2, T3, T4, T5>(
      this Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2)
    {
      return (t3, t4, t5) => action(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Partially apply action with first three parameters fixed.
    /// </summary>
    public static Action<T4, T5> Partial<T1, T2, T3, T4, T5>(
      this Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3)
    {
      return (t4, t5) => action(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Partially apply action with first four parameters fixed.
    /// </summary>
    public static Action<T5> Partial<T1, T2, T3, T4, T5>(
      this Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      return t5 => action(t1, t2, t3, t4, t5);
    }

    /// <summary>
    /// Partially apply action with all parameters fixed.
    /// </summary>
    public static Action Partial<T1, T2, T3, T4, T5>(
      this Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      return () => action(t1, t2, t3, t4, t5);
    }
  }
}
