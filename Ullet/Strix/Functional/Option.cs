/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional
{
  using System;

  /// <summary>
  /// Static Option methods.
  /// </summary>
  public static class Option
  {
    /// <summary>
    /// Create a <see cref="Option"/> instance without a value.
    /// </summary>
    public static Option<T> None<T>() => new Option<T>();

    /// <summary>
    /// Create a <see cref="Option"/> instance with a specific value.
    /// </summary>
    public static Option<T> Some<T>(T value) => value;

    /// <summary>
    /// Return value if Some otherwise fallback value.
    /// </summary>
    public static T GetOrElse<T>(this Option<T> option, T fallback)
      => option.GetOrElse(() => fallback);

    /// <summary>
    /// Return value if Some otherwise value of evaluated fallback function.
    /// </summary>
    public static T GetOrElse<T>(this Option<T> option, Func<T> fallback)
      => option.Match(some: value => value, none: fallback);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="option"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Option<TReturn> Map<T, TReturn>(
      this Option<T> option, Func<T, TReturn> func)
      => option.Match(some: v => Some(func(v)), none: None<TReturn>);
  }

  /// <summary>
  /// An optional type, that may or may not have a value.
  /// </summary>
  public struct Option<T>
  {
    /// <summary>
    /// Construct Option instance. Will be "Some" is contained type is a value
    /// type or given value is not null, otherwise "None".
    /// </summary>
    /// <param name="value">Value to initialize with.</param>
    /// <remarks>
    /// Option is a struct so can never be null. Value is not allowed to be null
    /// so that never have to worry about null if using Option.
    /// </remarks>
    public Option(T value)
      : this()
    {
      Value = value;
      IsSome = null != value;
    }

    internal T Value { get; }

    /// <summary>
    /// True if there is a value.
    /// </summary>
    public bool IsSome { get; }

    /// <summary>
    /// True if there is no value.
    /// </summary>
    public bool IsNone => !IsSome;

    /// <summary>
    /// Convert Option to type <typeparamref name="TReturn"/> by applying
    /// function <paramref name="some"/> if instance IsSome, otherwise by
    /// applying function <paramref name="none"/>.
    /// </summary>
    /// <typeparam name="TReturn">Type to convert to.</typeparam>
    /// <param name="some">Convertion function when value to convert.</param>
    /// <param name="none">Fallback function.</param>
    /// <returns>An instance of <typeparamref name="TReturn"/>.</returns>
    public TReturn Match<TReturn>(Func<T, TReturn> some, Func<TReturn> none)
      => IsSome ? some(Value) : none();

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object"/> is equal
    /// to the current <see cref="T:Option"/>.
    /// </summary>
    public override bool Equals(object obj)
      => obj is Option<T> && Equals((Option<T>)obj);

    /// <summary>
    /// Determines whether the specified <see cref="T:Option"/> is equal to the
    /// current <see cref="T:Option"/>.
    /// </summary>
    public bool Equals(Option<T> other)
      => Match(
        some: value => other.Match(
          some: otherValue => Equals(value, otherValue),
          none: () => false),
        none: () => other.Match(
          some: _ => false,
          none: () => true)
        );

    /// <summary>
    /// Serves as a hash function for the type.
    /// </summary>
    public override int GetHashCode()
      => Match(
        some: value => (value.GetHashCode()*397) ^ typeof (T).GetHashCode(),
        none: () => -typeof (T).GetHashCode());

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    public override string ToString()
      => $"Option<{typeof (T).Name}> : " +
         $"{Match(some: value => value.ToString(), none: () => "<none>")}";

    /// <summary>
    /// Implicit conversion from instance of <typeparamref name="T"/> to an
    /// instance of <see cref="Option"/>. For nullable types, null will result
    /// in None, all other values are Some.
    /// </summary>
    public static implicit operator Option<T>(T value) => new Option<T>(value);
  }
}
