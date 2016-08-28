/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional
{
  using System;

  public partial class Fn
  {
    /// <summary>
    /// Convert Func to Action throwing away return value.
    /// </summary>
    /// <remarks>
    /// Particularly intended for use when have a Func returning Unit but need
    /// to pass an Action.
    /// </remarks>
    public static Action ToAction<T>(this Func<T> f) => () => { f(); };

    /// <summary>
    /// Convert Func to Action throwing away return value.
    /// </summary>
    /// <remarks>
    /// Particularly intended for use when have a Func returning Unit but need
    /// to pass an Action.
    /// </remarks>
    public static Action<TA> ToAction<TA, T>(this Func<TA, T> f)
      => a => { f(a); };

    /// <summary>
    /// Convert Func to Action throwing away return value.
    /// </summary>
    /// <remarks>
    /// Particularly intended for use when have a Func returning Unit but need
    /// to pass an Action.
    /// </remarks>
    public static Action<TA, TB> ToAction<TA, TB, T>(this Func<TA, TB, T> f)
      => (a, b) => { f(a, b); };

    /// <summary>
    /// Convert Func to Action throwing away return value.
    /// </summary>
    /// <remarks>
    /// Particularly intended for use when have a Func returning Unit but need
    /// to pass an Action.
    /// </remarks>
    public static Action<TA, TB, TC> ToAction<TA, TB, TC, T>(
      this Func<TA, TB, TC, T> f)
      => (a, b, c) => { f(a, b, c); };

    /// <summary>
    /// Convert Func to Action throwing away return value.
    /// </summary>
    /// <remarks>
    /// Particularly intended for use when have a Func returning Unit but need
    /// to pass an Action.
    /// </remarks>
    public static Action<TA, TB, TC, TD> ToAction<TA, TB, TC, TD, T>(
      this Func<TA, TB, TC, TD, T> f)
      => (a, b, c, d) => { f(a, b, c, d); };

    /// <summary>
    /// Convert Func to Action throwing away return value.
    /// </summary>
    /// <remarks>
    /// Particularly intended for use when have a Func returning Unit but need
    /// to pass an Action.
    /// </remarks>
    public static Action<TA, TB, TC, TD, TE> ToAction<TA, TB, TC, TD, TE, T>(
      this Func<TA, TB, TC, TD, TE, T> f)
      => (a, b, c, d, e) => { f(a, b, c, d, e); };
  }
}
