/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional
{
  /// <summary>
  /// A collection of functional functions.
  /// </summary>
  public static partial class Fn
  {
    private static readonly Unit UnitInstance = new Unit();

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

    /// <summary>
    /// Get the single valued Unit instance.
    /// </summary>
    public static Unit Unit
    {
      get
      {
        return UnitInstance;
      }
    }
  }

  /// <summary>
  /// Void-like type that can be used as an assignable type.
  /// </summary>
  public struct Unit { }
}
