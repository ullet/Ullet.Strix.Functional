/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;

namespace Ullet.PD.Functional
{
  /// <summary>
  /// A "no value" data type. Nothing and <see cref="Just{T}"/> are
  /// components of the <see cref="Maybe{T}"/> optional type.
  /// </summary>
  public class Nothing<T> : Maybe<T>
  {
    /// <summary>
    /// Always throws <see cref="InvalidOperationException"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Always thrown.
    /// </exception>
    public override T Value
    {
      get
      {
        throw new InvalidOperationException("Nothing has no value");
      }
    }

    /// <summary>
    /// Always false.  Nothing has no value.
    /// </summary>
    public override bool HasValue
    {
      get
      {
        return false;
      }
    }
  }
}
