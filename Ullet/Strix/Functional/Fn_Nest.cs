/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;

namespace Ullet.Strix.Functional
{
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
    /// <remarks>
    /// Particularly useful for nesting exception handler delegates.
    /// </remarks>
    public static Action<Action> Nest(
      this Action<Action> outerAction, Action<Action> innerAction)
    {
      return action => outerAction(() => innerAction(action));
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
    public static Action Nest(
      this Action<Action> outerAction, Action innerAction)
    {
      return () => outerAction(innerAction);
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
    /// <returns>An <![CDATA[Func<Func<T>, T>]]> delegate.</returns>
    /// <remarks>
    /// Particularly useful for nesting exception handler delegates.
    /// </remarks>
    public static Func<Func<T>, T> Nest<T>(
      this Func<Func<T>, T> outerFunc, Func<Func<T>, T> innerFunc)
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
    /// <returns>An <see cref="Func{T}"/> delegate.</returns>
    public static Func<T> Nest<T>(
      this Func<Func<T>, T> outerFunc, Func<T> innerFunc)
    {
      return () => outerFunc(innerFunc);
    }
  }
}
