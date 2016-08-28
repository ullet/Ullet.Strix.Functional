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
    /// <summary>
    /// Get the single valued Unit instance.
    /// </summary>
    public static Unit Unit { get; } = new Unit();
  }

  /// <summary>
  /// Void-like type that can be used as an assignable type.
  /// </summary>
  public struct Unit { }
}
