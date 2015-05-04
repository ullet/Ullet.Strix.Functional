using System;

namespace Ullet.PD.Functional
{
  /// <summary>
  /// </summary>
  public class Nothing<T> : Maybe<T>
  {
    /// <summary>
    /// </summary>
    public override T Value
    {
      get
      {
        throw new InvalidOperationException("Nothing has no value");
      }
    }

    /// <summary>
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
