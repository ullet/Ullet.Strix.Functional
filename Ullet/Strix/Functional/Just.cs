/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional
{
  /// <summary>
  /// A data type. <see cref="Nothing{T}"/> and Just are components of the
  /// <see cref="Maybe{T}"/> optional type.
  /// </summary>
  public class Just<T> : Maybe<T>
  {
    private readonly T _value;

    /// <summary>
    /// Create instance with specified value.
    /// </summary>
    public Just(T value)
    {
      _value = value;
    }

    /// <summary>
    /// Get the value.
    /// </summary>
    public override T Value
    {
      get
      {
        return _value;
      }
    }

    /// <summary>
    /// Always true.
    /// </summary>
    public override bool HasValue
    {
      get
      {
        return true;
      }
    }
  }
}
