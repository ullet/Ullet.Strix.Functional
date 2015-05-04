namespace Ullet.PD.Functional
{
  /// <summary> 
  /// </summary>
  public abstract class Maybe<T>
  {
    /// <summary>
    /// </summary>
    public abstract T Value { get; }

    /// <summary> 
    /// </summary>
    public abstract bool HasValue { get; }

    /// <summary> 
    /// </summary>
    public static explicit operator T(Maybe<T> maybe)
    {
      return maybe.Value;
    }

    /// <summary> 
    /// </summary>
    public static implicit operator Maybe<T>(T value)
    {
      return new Just<T>(value);
    }
  }
}
