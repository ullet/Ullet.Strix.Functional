/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional
{
  using System;

  public static partial class Fn
  {
    /// <summary>
    /// Convert function to curried form.
    /// </summary>
    /// <remarks>
    /// A unary function is already in curried form, so Curry in this case is
    /// simply the identity function.
    /// </remarks>
    public static Func<T, TOut> Curry<T, TOut>(this Func<T, TOut> fn) => fn;

    /// <summary>
    /// Convert function to curried form.
    /// </summary>
    public static Func<T1, Func<T2, TOut>> Curry<T1, T2, TOut>(
      this Func<T1, T2, TOut> fn)
      => t1 => t2 => fn(t1, t2);

    /// <summary>
    /// Convert function to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, TOut>>>
      Curry<T1, T2, T3, TOut>(this Func<T1, T2, T3, TOut> fn)
      => t1 => t2 => t3 => fn(t1, t2, t3);

    /// <summary>
    /// Convert function to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, Func<T4, TOut>>>>
      Curry<T1, T2, T3, T4, TOut>(this Func<T1, T2, T3, T4, TOut> fn)
      => t1 => t2 => t3 => t4 => fn(t1, t2, t3, t4);

    /// <summary>
    /// Convert function to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TOut>>>>>
      Curry<T1, T2, T3, T4, T5, TOut>(this Func<T1, T2, T3, T4, T5, TOut> fn)
      => t1 => t2 => t3 => t4 => t5 => fn(t1, t2, t3, t4, t5);

    /// <summary>
    /// Convert action to curried form.
    /// </summary>
    /// <remarks>
    /// A unary action is already in curried form, so Curry in this case is
    /// simply the identity function.
    /// (Except with twist that always want to return a Func, so returning an
    /// equivalent function with Unit return type).
    /// </remarks>
    public static Func<T, Unit> Curry<T>(this Action<T> a) => a.ToFunc();

    /// <summary>
    /// Convert action to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Unit>> Curry<T1, T2>(this Action<T1, T2> a)
      => a.ToFunc().Curry();

    /// <summary>
    /// Convert action to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, Unit>>> Curry<T1, T2, T3>(
      this Action<T1, T2, T3> a)
      => a.ToFunc().Curry();

    /// <summary>
    /// Convert action to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, Func<T4, Unit>>>>
      Curry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> a)
      => a.ToFunc().Curry();

    /// <summary>
    /// Convert action to curried form.
    /// </summary>
    public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Unit>>>>>
      Curry<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> a)
      => a.ToFunc().Curry();

    /// <summary>
    /// Convert function to non-curried form.
    /// </summary>
    /// <remarks>
    /// A unary function that doesn't return another function is already in
    /// non-curried form, so Uncurry in this case is simply the identity
    /// function.
    /// </remarks>
    public static Func<T, TOut> Uncurry<T, TOut>(this Func<T, TOut> fn) => fn;

    /// <summary>
    /// Convert function to non-curried form.
    /// </summary>
    public static Func<T1, T2, TOut> Uncurry<T1, T2, TOut>(
      this Func<T1, Func<T2, TOut>> fn)
      => (t1, t2) => fn(t1)(t2);

    /// <summary>
    /// Convert function to non-curried form.
    /// </summary>
    public static Func<T1, T2, T3, TOut> Uncurry<T1, T2, T3, TOut>(
      this Func<T1, Func<T2, Func<T3, TOut>>> fn)
      => (t1, t2, t3) => fn(t1)(t2)(t3);

    /// <summary>
    /// Convert function to non-curried form.
    /// </summary>
    public static Func<T1, T2, T3, T4, TOut> Uncurry<T1, T2, T3, T4, TOut>(
      this Func<T1, Func<T2, Func<T3, Func<T4, TOut>>>> fn)
      => (t1, t2, t3, t4) => fn(t1)(t2)(t3)(t4);

    /// <summary>
    /// Convert function to non-curried form.
    /// </summary>
    public static Func<T1, T2, T3, T4, T5, TOut>
      Uncurry<T1, T2, T3, T4, T5, TOut>(
      this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TOut>>>>> fn)
      => (t1, t2, t3, t4, t5) => fn(t1)(t2)(t3)(t4)(t5);
  }
}
