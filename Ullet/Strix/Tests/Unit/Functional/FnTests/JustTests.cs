/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional.Tests.Unit.FnTests
{
  using NUnit.Framework;

  [TestFixture]
  public class JustTests
  {
    [Test]
    public void JustIsAMaybe()
    {
      Assert.That(Fn.Just(42), Is.InstanceOf<Maybe<int>>());
    }

    [Test]
    public void JustHasAValue()
    {
      Assert.That(Fn.Just(42).HasValue, Is.True);
    }

    [Test]
    public void JustHasSetValue()
    {
      Assert.That(Fn.Just(42).Value, Is.EqualTo(42));
    }
  }
}
