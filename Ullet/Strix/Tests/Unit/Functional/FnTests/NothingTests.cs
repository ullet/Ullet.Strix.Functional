/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional.Tests.Unit.FnTests
{
  using NUnit.Framework;

  [TestFixture]
  public class NothingTests
  {
    [Test]
    public void NothingIsAMaybe()
    {
      Assert.That(Fn.Nothing<int>(), Is.InstanceOf<Maybe<int>>());
    }

    [Test]
    public void NothingHasNoValue()
    {
      Assert.That(Fn.Nothing<int>().HasValue, Is.False);
    }
  }
}
