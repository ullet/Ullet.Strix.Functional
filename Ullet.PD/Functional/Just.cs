namespace Ullet.PD.Functional
{
  /// <summary>
  /// </summary>
  public class Just<T> : Maybe<T>
  {
    private readonly T _value;

    /// <summary>
    /// </summary>
    public Just(T value)
    {
      _value = value;
    }

    /// <summary>
    /// </summary>
    public override T Value
    {
      get
      {
        return _value;
      }
    }

    /// <summary>
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
