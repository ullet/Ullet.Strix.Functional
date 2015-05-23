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
  }
}
