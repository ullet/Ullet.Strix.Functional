using System;
using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional
{
  [TestFixture]
  public class NothingTests
  {
    [Test]
    public void IsAMaybe()
    {
      Assert.That(new Nothing<int>(), Is.InstanceOf<Maybe<int>>());
    }

    [Test]
    public void HasNoValue()
    {
      Assert.That(new Nothing<int>().HasValue, Is.False);
    }

    [Test]
    public void ThrowsExceptionIfAttemptAccessValue()
    {
      Assert.Throws<InvalidOperationException>(
        () => ((Func<int>)(() => new Nothing<int>().Value))());
    }

    [Test]
    public void TwoNothingsOfSameTypeAreEqual()
    {
      // ReSharper disable EqualExpressionComparison
      Assert.That(
        new Nothing<int>().Equals(new Nothing<int>()), Is.True);
      // ReSharper restore EqualExpressionComparison
    }

    [Test]
    public void TwoNothingsOfDifferentTypeAreNotEqual()
    {
      // ReSharper disable SuspiciousTypeConversion.Global
      Assert.That(
        new Nothing<int>().Equals(new Nothing<string>()), Is.False);
      // ReSharper restore SuspiciousTypeConversion.Global
    }

    [Test]
    public void InstanceIsEqualToItself()
    {
      var instance = new Nothing<int>();
      // ReSharper disable EqualExpressionComparison
      Assert.That(instance.Equals(instance), Is.True);
      // ReSharper restore EqualExpressionComparison
    }

    [Test]
    public void TwoWithSameTypeHaveSameHashCode()
    {
      Assert.That(
        new Nothing<int>().GetHashCode(),
        Is.EqualTo(new Nothing<int>().GetHashCode()));
    }

    [Test]
    public void TwoWithDifferentTypesHaveDifferentHashCodes()
    {
      Assert.That(
        new Nothing<int>().GetHashCode(),
        Is.Not.EqualTo(new Nothing<long>().GetHashCode()));
    }

    [Test]
    public void HasStringRepresentationContainingType()
    {
      Assert.That(
        new Nothing<int>().ToString(), Is.EqualTo("<nothing<Int32>>"));
      Assert.That(
        new Nothing<string>().ToString(), Is.EqualTo("<nothing<String>>"));
    }
  }
}
