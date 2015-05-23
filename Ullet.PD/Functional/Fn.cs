/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.PD.Functional
{
  /// <summary>
  /// A collection of functional functions.
  /// </summary>
  public static partial class Fn
  {
    /// <summary>
    /// Create a <see cref="Maybe{T}"/> instance without a value.
    /// </summary>
    public static Maybe<T> Nothing<T>()
    {
      return new Nothing<T>();
    }

    /// <summary>
    /// Create a <see cref="Maybe{T}"/> instance with a specific value.
    /// </summary>
    public static Maybe<T> Just<T>(T value)
    {
      return new Just<T>(value);
    }
  }
}
