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
  public class OptionTests
  {
    [Test]
    public void SomeIsSome()
    {
      Assert.That(Option.Some(42).IsSome, Is.True);
    }
    [Test]
    public void NoneIsNone()
    {
      Assert.That(Option.None<int>().IsNone, Is.True);
    }

    [Test]
    public void TwoWithSameValueAreEqual()
    {
      Assert.That(Option.Some(42).Equals(Option.Some(42)), Is.True);
    }

    [Test]
    public void TwoWithNullValueAreEqual()
    {
      Assert.That(
        Option.Some<string>(null).Equals(Option.Some<string>(null)), Is.True);
    }

    [Test]
    public void TwoWithDifferentValuesAreNotEqual()
    {
      Assert.That(Option.Some(42).Equals(Option.Some(41)), Is.False);
    }

    [Test]
    public void TwoWithDifferentTypesAreNotEqual()
    {
      // ReSharper disable once SuspiciousTypeConversion.Global
      Assert.That(Option.Some(42).Equals(Option.Some<long>(42)), Is.False);
    }

    [Test]
    public void TwoNonesOfSameTypeAreEqual()
    {
      Assert.That(Option.None<int>().Equals(Option.None<int>()), Is.True);
    }

    [Test]
    public void TwoNonesOfDifferentTypeAreNotEqual()
    {
      // ReSharper disable once SuspiciousTypeConversion.Global
      Assert.That(
        Option.None<int>().Equals(Option.None<string>()), Is.False);
    }

    [Test]
    public void SomeInstanceIsEqualToItself()
    {
      var instance = Option.Some(42);
      Assert.That(instance.Equals(instance), Is.True);
    }

    [Test]
    public void NoneInstanceIsEqualToItself()
    {
      var instance = Option.None<int>();
      Assert.That(instance.Equals(instance), Is.True);
    }

    [TestCaseSource("SomeAndNoneOfSameContainedTypeTestsCases")]
    public void SomeAndNoneOfSameContainedTypeAreNotEqual(
      dynamic testCase)
    {
      Assert.That(testCase.Some, Is.Not.EqualTo(testCase.None));
    }

    [Test]
    public void IsImplicitlyConvertibleFromTypeOfValue()
    {
      Option<int> some = 42;
      Assert.That(some, Is.EqualTo(Option.Some(42)));
    }

    [Test]
    public void TwoWithSameValueHaveSameHashCode()
    {
      Assert.That(
        Option.Some(42).GetHashCode(),
        Is.EqualTo(Option.Some(42).GetHashCode()));
    }

    [Test]
    public void TwoNonesWithSameTypeHaveSameHashCode()
    {
      Assert.That(
        Option.None<int>().GetHashCode(),
        Is.EqualTo(Option.None<int>().GetHashCode()));
    }

    [Test]
    public void TwoWithDifferentValueHaveDifferentHashCodes()
    {
      Assert.That(
        Option.Some(42).GetHashCode(),
        Is.Not.EqualTo(Option.Some(41).GetHashCode()));
    }

    [Test]
    public void TwoSomesWithDifferentTypesHaveDifferentHashCodes()
    {
      Assert.That(
        Option.Some(42).GetHashCode(),
        Is.Not.EqualTo(Option.Some<long>(42).GetHashCode()));
    }

    [Test]
    public void TwoNonesWithDifferentTypesHaveDifferentHashCodes()
    {
      Assert.That(
        Option.None<int>().GetHashCode(),
        Is.Not.EqualTo(Option.None<long>().GetHashCode()));
    }

    [TestCaseSource("SomeAndNoneOfSameContainedTypeTestsCases")]
    public void SomeAndNoneOfSameContainedTypeHaveDifferentHashCodes(
      dynamic testCase)
    {
      Assert.That(
        testCase.Some.GetHashCode(),
        Is.Not.EqualTo(testCase.None.GetHashCode()));
    }

    [Test]
    public void StringRepresentationOfSomeContainsStringRepresentationOfValue()
    {
      Assert.That(Option.Some(42).ToString(), Is.StringContaining("42"));
    }

    [Test]
    public void StringRepresentationOfNoneContainsTheWordNone()
    {
      Assert.That(
        Option.None<int>().ToString(), Is.StringContaining("<none>"));
    }

    [TestCaseSource("OptionTestCases")]
    public void StringRepresentationContainsNameOfContainedType(
      dynamic testCase)
    {
      Assert.That(
        testCase.Option.ToString(), Is.StringContaining(testCase.Type.Name));
    }

    private static IEnumerable<dynamic> OptionTestCases
    {
      get
      {
        return NoneTestCases.Union(SomeTestCases);
      }
    }

    private static IEnumerable<dynamic>
      SomeAndNoneOfSameContainedTypeTestsCases
    {
      get
      {
        // Assumed types are in same order for zipped enumerables.
        return SomeTestCases.Zip(
          NoneTestCases, (s, n) => new {Some = s.Option, None = n.Option});
      }
    }

    private static IEnumerable<dynamic> NoneTestCases
    {
      get
      {
        return new[]
        {
          NoneTestCase<int>(),
          NoneTestCase<string>(),
          NoneTestCase<double>()
        };
      }
    }

    private static IEnumerable<dynamic> SomeTestCases
    {
      get
      {
        return new[]
        {
          SomeTestCase(3),
          SomeTestCase("a string"),
          SomeTestCase(3.0)
        };
      }
    }

    private static dynamic NoneTestCase<T>()
    {
      return new {Option = Option.None<T>(), Type = typeof (T)};
    }

    private static dynamic SomeTestCase<T>(T value)
    {
      return new {Option = Option.Some(value), Type = typeof (T)};
    }
  }
}
