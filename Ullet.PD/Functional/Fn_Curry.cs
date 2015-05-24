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
    /// Convert action to curried form.
    /// </summary>
    public static Func<T1, Action<T2>> Curry<T1, T2>(this Action<T1, T2> a)
    {
      return t1 => t2 => a(t1, t2);
    }

    /// <summary>
    /// Convert action to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Action<T3>>> Curry<T1, T2, T3>(
      this Action<T1, T2, T3> a)
    {
      return t1 => t2 => t3 => a(t1, t2, t3);
    }

    /// <summary>
    /// Convert action to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, Action<T4>>>>
      Curry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> a)
    {
      return t1 => t2 => t3 => t4 => a(t1, t2, t3, t4);
    }

    /// <summary>
    /// Convert action to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, Func<T4, Action<T5>>>>>
      Curry<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> a)
    {
      return t1 => t2 => t3 => t4 => t5 => a(t1, t2, t3, t4, t5);
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

    /// <summary>
    /// Convert curried action to non-curried form.
    /// </summary>
    public static Action<T1, T2> Uncurry<T1, T2>(this Func<T1, Action<T2>> fn)
    {
      return (t1, t2) => fn(t1)(t2);
    }

    /// <summary>
    /// Convert curried action to non-curried form.
    /// </summary>
    public static Action<T1, T2, T3> Uncurry<T1, T2, T3>(
      this Func<T1, Func<T2, Action<T3>>> fn)
    {
      return (t1, t2, t3) => fn(t1)(t2)(t3);
    }

    /// <summary>
    /// Convert curried action to non-curried form.
    /// </summary>
    public static Action<T1, T2, T3, T4> Uncurry<T1, T2, T3, T4>(
      this Func<T1, Func<T2, Func<T3, Action<T4>>>> fn)
    {
      return (t1, t2, t3, t4) => fn(t1)(t2)(t3)(t4);
    }

    /// <summary>
    /// Convert curried action to non-curried form.
    /// </summary>
    public static Action<T1, T2, T3, T4, T5> Uncurry<T1, T2, T3, T4, T5>(
      this Func<T1, Func<T2, Func<T3, Func<T4, Action<T5>>>>> fn)
    {
      return (t1, t2, t3, t4, t5) => fn(t1)(t2)(t3)(t4)(t5);
    }
  }
}
