/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional
{
  using System;

  /// <summary>
  /// Static Maybe methods.
  /// </summary>
  public static class Maybe
  {
    /// <summary>
    /// Create a <see cref="Maybe{T}"/> instance without a value.
    /// </summary>
    public static Maybe<T> Nothing<T>()
    {
      return new Maybe<T>();
    }

    /// <summary>
    /// Create a <see cref="Maybe{T}"/> instance with a specific value.
    /// </summary>
    public static Maybe<T> Just<T>(T value)
    {
      return value;
    }

    /// <summary>
    /// Return value if Just otherwise fallback value.
    /// </summary>
    public static T GetOrElse<T>(this Maybe<T> maybe, T fallback)
    {
      return maybe.GetOrElse(() => fallback);
    }

    /// <summary>
    /// Return value if Just otherwise value of evaluated fallback function.
    /// </summary>
    public static T GetOrElse<T>(this Maybe<T> maybe, Func<T> fallback)
    {
      return maybe.Match(just: value => value, nothing: fallback);
    }
  }

  /// <summary>
  /// An optional type, that may or may not have a value.
  /// </summary>
  public struct Maybe<T>
  {
    /// <summary>
    /// Construct Maybe instance. Will be "Just" is contained type is a value
    /// type or given value is not null, otherwise "Nothing".
    /// </summary>
    /// <param name="value">Value to initialize with.</param>
    /// <remarks>
    /// Maybe is a struct so can never be null. Value is not allowed to be null
    /// so that never have to worry about null if using Maybe.
    /// </remarks>
    public Maybe(T value)
      : this()
    {
      Value = value;
      IsJust = null != value;
    }

    internal T Value { get; private set; }

    /// <summary>
    /// True if there is a value.
    /// </summary>
    public bool IsJust { get; private set; }

    /// <summary>
    /// True if there is no value.
    /// </summary>
    public bool IsNothing { get { return !IsJust; } }

    /// <summary>
    /// Convert Maybe to type <typeparamref name="TReturn"/> by applying
    /// function <paramref name="just"/> if instance IsJust, otherwise by
    /// applying function <paramref name="nothing"/>.
    /// </summary>
    /// <typeparam name="TReturn">Type to convert to.</typeparam>
    /// <param name="just">Convertion function when value to convert.</param>
    /// <param name="nothing">Fallback function.</param>
    /// <returns>An instance of <typeparamref name="TReturn"/>.</returns>
    public TReturn Match<TReturn>(Func<T, TReturn> just, Func<TReturn> nothing)
    {
      return IsJust ? just(Value) : nothing();
    }

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object"/> is equal
    /// to the current <see cref="T:Maybe"/>.
    /// </summary>
    public override bool Equals(object obj)
    {
      return obj is Maybe<T> && Equals((Maybe<T>)obj);
    }

    /// <summary>
    /// Determines whether the specified <see cref="T:Maybe"/> is equal to the
    /// current <see cref="T:Maybe"/>.
    /// </summary>
    public bool Equals(Maybe<T> other)
    {
      return Match(
        just: value => other.Match(
          just: otherValue => Equals(value, otherValue),
          nothing: () => false),
        nothing: () => other.Match(
          just: _ => false,
          nothing: () => true)
        );
    }

    /// <summary>
    /// Serves as a hash function for the type.
    /// </summary>
    public override int GetHashCode()
    {
      return Match(
        just: value => (value.GetHashCode() * 397) ^ typeof(T).GetHashCode(),
        nothing: () => -typeof(T).GetHashCode());
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    public override string ToString()
    {
      return
        string.Format("Maybe<{0}> : {1}",
          typeof(T).Name,
          Match(just: value => value.ToString(), nothing: () => "<nothing>"));
    }

    /// <summary>
    /// Implicit conversion from instance of <typeparamref name="T"/> to an
    /// instance of <see cref="Maybe{T}"/>. For nullable types, null will result
    /// in Nothing, all other values are Just.
    /// </summary>
    public static implicit operator Maybe<T>(T value)
    {
      return new Maybe<T>(value);
    }
  }
}
