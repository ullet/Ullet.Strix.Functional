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
    /// Convert action to Func returning Unit.
    /// </summary>
    public static Func<Unit> ToFunc(this Action action)
      => () => { action(); return Unit; };

    /// <summary>
    /// Convert action to Func returning Unit.
    /// </summary>
    public static Func<T, Unit> ToFunc<T>(this Action<T> action)
      => a => ToFunc(() => action(a))();

    /// <summary>
    /// Convert action to Func returning Unit.
    /// </summary>
    public static Func<TA, TB, Unit> ToFunc<TA, TB>(this Action<TA, TB> action)
      => (a, b) => ToFunc(() => action(a, b))();

    /// <summary>
    /// Convert action to Func returning Unit.
    /// </summary>
    public static Func<TA, TB, TC, Unit> ToFunc<TA, TB, TC>(
      this Action<TA, TB, TC> action)
      => (a, b, c) => ToFunc(() => action(a, b, c))();

    /// <summary>
    /// Convert action to Func returning Unit.
    /// </summary>
    public static Func<TA, TB, TC, TD, Unit> ToFunc<TA, TB, TC, TD>(
      this Action<TA, TB, TC, TD> action)
      => (a, b, c, d) => ToFunc(() => action(a, b, c, d))();

    /// <summary>
    /// Convert action to Func returning Unit.
    /// </summary>
    public static Func<TA, TB, TC, TD, TE, Unit> ToFunc<TA, TB, TC, TD, TE>(
      this Action<TA, TB, TC, TD, TE> action)
      => (a, b, c, d, e) => ToFunc(() => action(a, b, c, d, e))();
  }
}
