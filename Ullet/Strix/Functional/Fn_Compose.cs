/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public static partial class Fn
  {
    /// <summary>
    /// Compose <paramref name="outer"/> unary function with
    /// <paramref name="inner"/> unary function.
    /// </summary>
    /// <remarks>Alias for Compose.</remarks>
    public static Func<TA, TC> After<TA, TB, TC>(
      this Func<TB, TC> outer, Func<TA, TB> inner)
    {
      return Compose(outer, inner);
    }

    /// <summary>
    /// Compose <paramref name="outer"/> unary function with
    /// <paramref name="inner"/> nonary function.
    /// </summary>
    /// <remarks>Alias for Compose.</remarks>
    public static Func<TB> After<TA, TB>(
      this Func<TA, TB> outer, Func<TA> inner)
    {
      return Compose(outer, inner);
    }

    /// <summary>
    /// Compose <paramref name="outer"/> unary function with
    /// <paramref name="inner"/> unary function.
    /// </summary>
    public static Func<TA, TC> Compose<TA, TB, TC>(
      this Func<TB, TC> outer, Func<TA, TB> inner)
    {
      return a => outer(inner(a));
    }

    /// <summary>
    /// Compose <paramref name="outer"/> unary function with
    /// <paramref name="inner"/> nonary function.
    /// </summary>
    public static Func<TB> Compose<TA, TB>(
      this Func<TA, TB> outer, Func<TA> inner)
    {
      return () => outer(inner());
    }

    /// <summary>
    /// Compose <paramref name="outer"/> unary function with
    /// <paramref name="inner"/> unary function.
    /// </summary>
    /// <remarks>Alias for ComposeReverse.</remarks>
    public static Func<TA, TC> Before<TA, TB, TC>(
      this Func<TA, TB> inner, Func<TB, TC> outer)
    {
      return ComposeReverse(inner, outer);
    }

    /// <summary>
    /// Compose <paramref name="outer"/> unary function with
    /// <paramref name="inner"/> nonary function.
    /// </summary>
    /// <remarks>Alias for ComposeReverse.</remarks>
    public static Func<TB> Before<TA, TB>(
      this Func<TA> inner, Func<TA, TB> outer)
    {
      return ComposeReverse(inner, outer);
    }

    /// <summary>
    /// Compose <paramref name="outer"/> unary function with
    /// <paramref name="inner"/> unary function.
    /// </summary>
    public static Func<TA, TC> ComposeReverse<TA, TB, TC>(
      this Func<TA, TB> inner, Func<TB, TC> outer)
    {
      return outer.Compose(inner);
    }

    /// <summary>
    /// Compose <paramref name="outer"/> unary function with
    /// <paramref name="inner"/> nonary function.
    /// </summary>
    public static Func<TB> ComposeReverse<TA, TB>(
      this Func<TA> inner, Func<TA, TB> outer)
    {
      return outer.Compose(inner);
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
    public static Func<TA, TB, TD> Compose<TA, TB, TC, TD>(
      this Func<TC, TD> outer, Func<TA, TB, TC> inner)
    {
      return (a, b) => outer(inner(a, b));
    }

    /// <summary>
    /// Compose unary outer function with ternary inner function.
    /// </summary>
    public static Func<TA, TB, TC, TE> Compose<TA, TB, TC, TD, TE>(
      this Func<TD, TE> outer, Func<TA, TB, TC, TD> inner)
    {
      return (a, b, c) => outer(inner(a, b, c));
    }

    /// <summary>
    /// Compose outer action with unary inner function.
    /// </summary>
    public static Func<TA, Unit> Compose<TA, TB>(
      this Action<TB> outer, Func<TA, TB> inner)
    {
      return outer.ToFunc().Compose(inner);
    }

    /// <summary>
    /// Return unary function that returns the result of composing
    /// <paramref name="g"/> with its input function.
    /// </summary>
    /// <returns>
    /// Function that transforms function TC->TA to new function TC->TB as
    /// result of applying function <paramref name="g"/> TA->TB.
    /// </returns>
    public static Func<Func<TC, TA>, Func<TC, TB>> Result<TA, TB, TC>(
      Func<TA, TB> g)
    {
      return f => x => g(f(x));
    }
  }
}
