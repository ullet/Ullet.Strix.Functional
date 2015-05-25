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
  public static partial class Fn
  {
    /// <summary>
    /// Compose <paramref name="outer"/> function with
    /// <paramref name="inner"/> function.
    /// </summary>
    /// <remarks>Alias for Compose.</remarks>
    public static Func<T, TOut2> After<T, TOut1, TOut2>(
      this Func<TOut1, TOut2> outer, Func<T, TOut1> inner)
    {
      return Compose(outer, inner);
    }

    /// <summary>
    /// Compose <paramref name="outer"/> function with
    /// <paramref name="inner"/> function.
    /// </summary>
    public static Func<T, TOut2> Compose<T, TOut1, TOut2>(
      this Func<TOut1, TOut2> outer, Func<T, TOut1> inner)
    {
      return t => outer(inner(t));
    }
    
    /// <summary>
    /// Compose <paramref name="outer"/> function with
    /// <paramref name="inner"/> function.
    /// </summary>
    /// <remarks>Alias for ComposeReverse.</remarks>
    public static Func<T, TOut2> Before<T, TOut1, TOut2>(
      this Func<T, TOut1> inner, Func<TOut1, TOut2> outer)
    {
      return ComposeReverse(inner, outer);
    }

    /// <summary>
    /// Compose <paramref name="outer"/> function with
    /// <paramref name="inner"/> function.
    /// </summary>
    public static Func<T, TOut2> ComposeReverse<T, TOut1, TOut2>(
      this Func<T, TOut1> inner, Func<TOut1, TOut2> outer)
    {
      return t => outer(inner(t));
    }

    /// <summary>
    /// Compose variable number of functions from right (inner) to left (outer).
    /// e.g. [f1, f2, f3] -> f1.f2.f3
    /// </summary>
    public static Func<T, T> Compose<T>(params Func<T, T>[] funcs)
    {
      return Compose((IEnumerable<Func<T, T>>)funcs);
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
    /// Compose variable number of functions from left (inner) to right (outer).
    /// e.g. [f1, f2, f3] -> f3.f2.f1
    /// </summary>
    public static Func<T, T> ComposeReverse<T>(params Func<T, T>[] funcs)
    {
      return ComposeReverse((IEnumerable<Func<T, T>>)funcs);
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
    /// Compose unary outer function with binary inner function.
    /// </summary>
    public static Func<T1, T2, T4> Compose<T1, T2, T3, T4>(
      this Func<T3, T4> outer, Func<T1, T2, T3> inner)
    {
      return (t1, t2) => outer(inner(t1, t2));
    }

    /// <summary>
    /// Compose unary outer function with ternary inner function.
    /// </summary>
    public static Func<T1, T2, T3, T5> Compose<T1, T2, T3, T4, T5>(
      this Func<T4, T5> outer, Func<T1, T2, T3, T4> inner)
    {
      return (t1, t2, t3) => outer(inner(t1, t2, t3));
    }

    /// <summary>
    /// Compose outer action with unary inner function.
    /// </summary>
    public static Action<T1> Compose<T1, T2>(
      this Action<T2> outer, Func<T1, T2> inner)
    {
      return t1 => outer(inner(t1));
    }

    /// <summary>
    /// Return unary function that returns the result of composing
    /// <paramref name="g"/> with its input function.
    /// </summary>
    /// <returns>
    /// Function that transforms function T3->T1 to new function T3->T2 as
    /// result of applying function <paramref name="g"/> T1->T2.
    /// </returns>
    public static Func<Func<T3, T1>, Func<T3, T2>> Result<T1, T2, T3>(
      Func<T1, T2> g)
    {
      return f => x => g(f(x));
    }
  }
}
