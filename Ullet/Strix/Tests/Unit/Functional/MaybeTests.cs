/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional.Tests.Unit
{
  using System.Collections.Generic;
  using System.Linq;
  using NUnit.Framework;

  [TestFixture]
  public class MaybeTests
  {
    [Test]
    public void JustIsJust()
    {
      Assert.That(Maybe.Just(42).IsJust, Is.True);
    }
    [Test]
    public void NothingIsNothing()
    {
      Assert.That(Maybe.Nothing<int>().IsNothing, Is.True);
    }

    [Test]
    public void TwoWithSameValueAreEqual()
    {
      Assert.That(Maybe.Just(42).Equals(Maybe.Just(42)), Is.True);
    }

    [Test]
    public void TwoWithNullValueAreEqual()
    {
      Assert.That(
        Maybe.Just<string>(null).Equals(Maybe.Just<string>(null)), Is.True);
    }

    [Test]
    public void TwoWithDifferentValuesAreNotEqual()
    {
      Assert.That(Maybe.Just(42).Equals(Maybe.Just(41)), Is.False);
    }

    [Test]
    public void TwoWithDifferentTypesAreNotEqual()
    {
      // ReSharper disable once SuspiciousTypeConversion.Global
      Assert.That(Maybe.Just(42).Equals(Maybe.Just<long>(42)), Is.False);
    }

    [Test]
    public void TwoNothingsOfSameTypeAreEqual()
    {
      Assert.That(Maybe.Nothing<int>().Equals(Maybe.Nothing<int>()), Is.True);
    }

    [Test]
    public void TwoNothingsOfDifferentTypeAreNotEqual()
    {
      // ReSharper disable once SuspiciousTypeConversion.Global
      Assert.That(
        Maybe.Nothing<int>().Equals(Maybe.Nothing<string>()), Is.False);
    }

    [Test]
    public void JustInstanceIsEqualToItself()
    {
      var instance = Maybe.Just(42);
      Assert.That(instance.Equals(instance), Is.True);
    }

    [Test]
    public void NothingInstanceIsEqualToItself()
    {
      var instance = Maybe.Nothing<int>();
      Assert.That(instance.Equals(instance), Is.True);
    }

    [TestCaseSource("JustAndNothingOfSameContainedTypeTestsCases")]
    public void JustAndNothingOfSameContainedTypeAreNotEqual(
      dynamic testCase)
    {
      Assert.That(testCase.Just, Is.Not.EqualTo(testCase.Nothing));
    }

    [Test]
    public void IsImplicitlyConvertibleFromTypeOfValue()
    {
      Maybe<int> just = 42;
      Assert.That(just, Is.EqualTo(Maybe.Just(42)));
    }

    [Test]
    public void TwoWithSameValueHaveSameHashCode()
    {
      Assert.That(
        Maybe.Just(42).GetHashCode(),
        Is.EqualTo(Maybe.Just(42).GetHashCode()));
    }

    [Test]
    public void TwoNothingsWithSameTypeHaveSameHashCode()
    {
      Assert.That(
        Maybe.Nothing<int>().GetHashCode(),
        Is.EqualTo(Maybe.Nothing<int>().GetHashCode()));
    }

    [Test]
    public void TwoWithDifferentValueHaveDifferentHashCodes()
    {
      Assert.That(
        Maybe.Just(42).GetHashCode(),
        Is.Not.EqualTo(Maybe.Just(41).GetHashCode()));
    }

    [Test]
    public void TwoJustsWithDifferentTypesHaveDifferentHashCodes()
    {
      Assert.That(
        Maybe.Just(42).GetHashCode(),
        Is.Not.EqualTo(Maybe.Just<long>(42).GetHashCode()));
    }

    [Test]
    public void TwoNothingsWithDifferentTypesHaveDifferentHashCodes()
    {
      Assert.That(
        Maybe.Nothing<int>().GetHashCode(),
        Is.Not.EqualTo(Maybe.Nothing<long>().GetHashCode()));
    }

    [TestCaseSource("JustAndNothingOfSameContainedTypeTestsCases")]
    public void JustAndNothingOfSameContainedTypeHaveDifferentHashCodes(
      dynamic testCase)
    {
      Assert.That(
        testCase.Just.GetHashCode(),
        Is.Not.EqualTo(testCase.Nothing.GetHashCode()));
    }

    [Test]
    public void StringRepresentationOfJustContainsStringRepresentationOfValue()
    {
      Assert.That(Maybe.Just(42).ToString(), Is.StringContaining("42"));
    }

    [Test]
    public void StringRepresentationOfNothingContainsTheWordNothing()
    {
      Assert.That(
        Maybe.Nothing<int>().ToString(), Is.StringContaining("<nothing>"));
    }

    [TestCaseSource("MaybeTestCases")]
    public void StringRepresentationContainsNameOfContainedType(
      dynamic testCase)
    {
      Assert.That(
        testCase.Maybe.ToString(), Is.StringContaining(testCase.Type.Name));
    }

    private static IEnumerable<dynamic> MaybeTestCases
    {
      get
      {
        return NothingTestCases.Union(JustTestCases);
      }
    }

    private static IEnumerable<dynamic>
      JustAndNothingOfSameContainedTypeTestsCases
    {
      get
      {
        // Assumed types are in same order for zipped enumerables.
        return JustTestCases.Zip(
          NothingTestCases, (j, n) => new {Just = j.Maybe, Nothing = n.Maybe});
      }
    }

    private static IEnumerable<dynamic> NothingTestCases
    {
      get
      {
        return new[]
        {
          NothingTestCase<int>(),
          NothingTestCase<string>(),
          NothingTestCase<double>()
        };
      }
    }

    private static IEnumerable<dynamic> JustTestCases
    {
      get
      {
        return new[]
        {
          JustTestCase(3),
          JustTestCase("a string"),
          JustTestCase(3.0)
        };
      }
    }

    private static dynamic NothingTestCase<T>()
    {
      return new {Maybe = Maybe.Nothing<T>(), Type = typeof (T)};
    }

    private static dynamic JustTestCase<T>(T value)
    {
      return new { Maybe = Maybe.Just(value), Type = typeof(T) };
    }
  }
}
