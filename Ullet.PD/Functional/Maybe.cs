/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;

namespace Ullet.PD.Functional
{
  /// <summary>
  /// An optional type, that may or may not have a value.
  /// </summary>
  public abstract class Maybe<T>
  {
    /// <summary>
    /// Get the value, or throw exception if no value.
    /// </summary>
    /// <remarks>
    /// Value can be null for reference and Nullable types.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown if attempt to get value when has no value.
    /// </exception>
    public abstract T Value { get; }

    /// <summary>
    /// Check if there is a value.
    /// </summary>
    public abstract bool HasValue { get; }

    /// <summary>
    /// Explicit conversion from <see cref="Maybe{T}"/> to instance of type
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <see cref="Maybe{T}"/> instance has no value.
    /// </exception>
    public static explicit operator T(Maybe<T> maybe)
    {
      return maybe.Value;
    }

    /// <summary>
    /// Implicit conversion from instance of <typeparamref name="T"/> to an
    /// instance of <see cref="Maybe{T}"/>.
    /// </summary>
    public static implicit operator Maybe<T>(T value)
    {
      return new Just<T>(value);
    }
  }
}
