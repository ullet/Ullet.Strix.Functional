using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.FnTests
{
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
