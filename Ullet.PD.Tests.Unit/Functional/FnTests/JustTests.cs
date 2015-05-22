﻿using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.FnTests
{
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
