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
    /// Nest one action within another.
    /// </summary>
    /// <param name="outerAction">
    /// The outer <![CDATA[Action<Action>]]> to wrap
    /// <paramref name="innerAction"/>.
    /// </param>
    /// <param name="innerAction">
    /// The inner <![CDATA[Action<Action>]]> to nest inside
    /// <paramref name="outerAction"/>.
    /// </param>
    /// <returns>An <![CDATA[Action<Action>]]> delegate.</returns>
    /// <example>
    /// <![CDATA[
    /// var log = new List<string>();
    /// Action<Action> logBefore = a =>
    /// {
    ///   log.Add("Before");
    ///   a();
    /// };
    /// Action<Action> logAfter = a =>
    /// {
    ///   a();
    ///   log.Add("After");
    /// };
    /// Action action = () => log.Add("During");
    /// var logBeforeAndAfter = logAfter.Nest(logBefore);
    /// var loggedAction = logBeforeAndAfter.Nest(action.ToFunc());
    /// loggedAction(); // log -> ["Before", "During", "After"]
    /// ]]>
    /// </example>
    public static Func<Func<Unit>, Unit> Nest(
      this Action<Action> outerAction, Action<Action> innerAction)
    {
      Func<Func<Unit>, Unit> outerFunc =
        f => outerAction.ToFunc()(f.ToAction());
      Func<Func<Unit>, Unit> innerFunc =
        f => innerAction.ToFunc()(f.ToAction());
      return outerFunc.Nest(innerFunc);
    }

    /// <summary>
    /// Nest one action within another.
    /// </summary>
    /// <param name="outerAction">
    /// The outer <![CDATA[Action<Action>]]> to wrap
    /// <paramref name="innerAction"/>.
    /// </param>
    /// <param name="innerAction">
    /// The inner <see cref="Action"/> to nest inside
    /// <paramref name="outerAction"/>.
    /// </param>
    /// <returns>An <see cref="Action"/> delegate.</returns>
    public static Func<Unit> Nest(
      this Action<Action> outerAction, Action innerAction)
    {
      Func<Func<Unit>, Unit> outerFunc =
        f => outerAction.ToFunc()(f.ToAction());
      return outerFunc.Nest(innerAction.ToFunc());
    }

    /// <summary>
    /// Nest one Func within another.
    /// </summary>
    /// <param name="outerFunc">
    /// The outer <![CDATA[Func<Func<T>, T>]]> to wrap
    /// <paramref name="innerFunc"/>.
    /// </param>
    /// <param name="innerFunc">
    /// The inner <![CDATA[Func<Func<T>, T>]]> to nest inside
    /// <paramref name="outerFunc"/>.
    /// </param>
    /// <returns>A <![CDATA[Func<Func<T>, T>]]> delegate.</returns>
    public static Func<Func<TA>, TC> Nest<TA, TB, TC>(
      this Func<Func<TB>, TC> outerFunc, Func<Func<TA>, TB> innerFunc)
    {
      return fn => outerFunc(() => innerFunc(fn));
    }

    /// <summary>
    /// Nest one Func within another.
    /// </summary>
    /// <param name="outerFunc">
    /// The outer <![CDATA[Func<Func<T>, T>]]> to wrap
    /// <paramref name="innerFunc"/>.
    /// </param>
    /// <param name="innerFunc">
    /// The inner <see cref="Func{T}"/> to nest inside
    /// <paramref name="outerFunc"/>.
    /// </param>
    /// <returns>A <see cref="Func{T}"/> delegate.</returns>
    public static Func<TB> Nest<TA, TB>(
      this Func<Func<TA>, TB> outerFunc, Func<TA> innerFunc)
    {
      return () => outerFunc(innerFunc);
    }
  }
}
