using NUnit.Framework;

namespace Ullet.Strix.Functional.Tests.Unit
{
  [TestFixture]
  public class JustTests
  {
    [Test]
    public void IsAMaybe()
    {
      Assert.That(new Just<int>(42), Is.InstanceOf<Maybe<int>>());
    }

    [Test]
    public void HasAValue()
    {
      Assert.That(new Just<int>(42).HasValue, Is.True);
    }

    [Test]
    public void HasSetValue()
    {
      Assert.That(new Just<int>(42).Value, Is.EqualTo(42));
    }

    [Test]
    public void TwoWithSameValueAreEqual()
    {
      // ReSharper disable EqualExpressionComparison
      Assert.That(new Just<int>(42).Equals(new Just<int>(42)), Is.True);
      // ReSharper restore EqualExpressionComparison
    }

    [Test]
    public void TwoWithNullValueAreEqual()
    {
      // ReSharper disable EqualExpressionComparison
      Assert.That(
        new Just<string>(null).Equals(new Just<string>(null)), Is.True);
      // ReSharper restore EqualExpressionComparison
    }

    [Test]
    public void TwoWithDifferentValuesAreNotEqual()
    {
      Assert.That(new Just<int>(42).Equals(new Just<int>(41)), Is.False);
    }

    [Test]
    public void TwoWithDifferentTypesAreNotEqual()
    {
      // ReSharper disable SuspiciousTypeConversion.Global
      Assert.That(new Just<int>(42).Equals(new Just<long>(42)), Is.False);
      // ReSharper restore SuspiciousTypeConversion.Global
    }

    [Test]
    public void InstanceIsEqualToItself()
    {
      var instance = new Just<int>(42);
      // ReSharper disable EqualExpressionComparison
      Assert.That(instance.Equals(instance), Is.True);
      // ReSharper restore EqualExpressionComparison
    }

    [Test]
    public void IsExplicitlyConvertibleToTypeOfValue()
    {
      Assert.That((int)(new Just<int>(42)), Is.EqualTo(42));
    }

    [Test]
    public void IsImplicitlyConvertibleFromTypeOfValue()
    {
      Maybe<int> just = 42;
      Assert.That(just, Is.EqualTo(new Just<int>(42)));
    }

    [Test]
    public void TwoWithSameValueHaveSameHashCode()
    {
      Assert.That(
        new Just<int>(42).GetHashCode(),
        Is.EqualTo(new Just<int>(42).GetHashCode()));
    }

    [Test]
    public void TwoWithDifferentValueHaveDifferentHashCodes()
    {
      Assert.That(
        new Just<int>(42).GetHashCode(),
        Is.Not.EqualTo(new Just<int>(41).GetHashCode()));
    }

    [Test]
    public void TwoWithDifferentTypesHaveDifferentHashCodes()
    {
      Assert.That(
        new Just<int>(42).GetHashCode(),
        Is.Not.EqualTo(new Just<long>(42).GetHashCode()));
    }

    [Test]
    public void StringRepresentationIsStringRepresentationOfValue()
    {
      Assert.That(new Just<int>(42).ToString(), Is.EqualTo("42"));
    }

    [Test]
    public void NullValueHasStringRepresentation()
    {
      Assert.That(new Just<string>(null).ToString(), Is.EqualTo("<null>"));
    }
  }
}
