/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015, 2016
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

namespace Ullet.Strix.Functional.Tests.Unit
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using NUnit.Framework;

  [TestFixture]
  public class OptionTests
  {
    [Test]
    public void SomeIsSome() => Assert.That(Option.Some(42).IsSome, Is.True);

    [Test]
    public void NoneIsNone() => Assert.That(Option.None<int>().IsNone, Is.True);

    [Test]
    public void TwoWithSameValueAreEqual()
      => Assert.That(Option.Some(42).Equals(Option.Some(42)), Is.True);

    [Test]
    public void TwoWithNullValueAreEqual()
      => Assert.That(
        Option.Some<string>(null).Equals(Option.Some<string>(null)), Is.True);

    [Test]
    public void TwoWithDifferentValuesAreNotEqual()
      => Assert.That(Option.Some(42).Equals(Option.Some(41)), Is.False);

    [Test]
    public void TwoWithDifferentTypesAreNotEqual()
      // ReSharper disable once SuspiciousTypeConversion.Global
      => Assert.That(Option.Some(42).Equals(Option.Some<long>(42)), Is.False);

    [Test]
    public void TwoNonesOfSameTypeAreEqual()
      => Assert.That(Option.None<int>().Equals(Option.None<int>()), Is.True);

    [Test]
    public void TwoNonesOfDifferentTypeAreNotEqual()
      // ReSharper disable once SuspiciousTypeConversion.Global
      => Assert.That(
        Option.None<int>().Equals(Option.None<string>()), Is.False);

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

    [TestCaseSource(nameof(SomeAndNoneOfSameContainedTypeTestsCases))]
    public void SomeAndNoneOfSameContainedTypeAreNotEqual(
      dynamic testCase)
      => Assert.That(testCase.Some, Is.Not.EqualTo(testCase.None));

    [TestCaseSource(nameof(EqualitySymmetricalTestCases))]
    public void EqualityIsSymmetrical(dynamic testCase)
      => Assert.That(
        testCase.Left.Equals(testCase.Right),
        Is.EqualTo(testCase.Right.Equals(testCase.Left)));

    [Test]
    public void IsImplicitlyConvertibleFromTypeOfValue()
    {
      Option<int> some = 42;
      Assert.That(some, Is.EqualTo(Option.Some(42)));
    }

    [Test]
    public void TwoWithSameValueHaveSameHashCode()
      => Assert.That(
        Option.Some(42).GetHashCode(),
        Is.EqualTo(Option.Some(42).GetHashCode()));

    [Test]
    public void TwoNonesWithSameTypeHaveSameHashCode()
      => Assert.That(
        Option.None<int>().GetHashCode(),
        Is.EqualTo(Option.None<int>().GetHashCode()));

    [Test]
    public void TwoWithDifferentValueHaveDifferentHashCodes()
      => Assert.That(
        Option.Some(42).GetHashCode(),
        Is.Not.EqualTo(Option.Some(41).GetHashCode()));

    [Test]
    public void TwoSomesWithDifferentTypesHaveDifferentHashCodes()
      => Assert.That(
        Option.Some(42).GetHashCode(),
        Is.Not.EqualTo(Option.Some<long>(42).GetHashCode()));

    [Test]
    public void TwoNonesWithDifferentTypesHaveDifferentHashCodes()
      => Assert.That(
        Option.None<int>().GetHashCode(),
        Is.Not.EqualTo(Option.None<long>().GetHashCode()));

    [TestCaseSource(nameof(SomeAndNoneOfSameContainedTypeTestsCases))]
    public void SomeAndNoneOfSameContainedTypeHaveDifferentHashCodes(
      dynamic testCase)
      => Assert.That(
        testCase.Some.GetHashCode(),
        Is.Not.EqualTo(testCase.None.GetHashCode()));

    [Test]
    public void StringRepresentationOfSomeContainsStringRepresentationOfValue()
      => Assert.That(Option.Some(42).ToString(), Is.StringContaining("42"));

    [Test]
    public void StringRepresentationOfNoneContainsTheWordNone()
      => Assert.That(
        Option.None<int>().ToString(), Is.StringContaining("<none>"));

    [TestCaseSource(nameof(OptionTestCases))]
    public void StringRepresentationContainsNameOfContainedType(
      dynamic testCase)
      => Assert.That(
        testCase.Option.ToString(), Is.StringContaining(testCase.Type.Name));

    [Test]
    public void GetOrElseReturnsValueIfIsSome()
      => Assert.That(Option.Some(123).GetOrElse(0), Is.EqualTo(123));

    [Test]
    public void GetOrElseReturnsFallbackIfIsNone()
      => Assert.That(Option.None<int>().GetOrElse(0), Is.EqualTo(0));

    [Test]
    public void GetOrElseReturnsEvaluatedFallbackFunctionIfIsNone()
      => Assert.That(Option.None<int>().GetOrElse(() => 0), Is.EqualTo(0));

    [TestFixture(typeof (int), typeof (double))]
    [TestFixture(typeof (double), typeof (string))]
    [TestFixture(typeof (long), typeof (long))]
    [TestFixture(typeof (object), typeof (Array))]
    public class NoneAlwaysMapsToNone<TInput, TResult>
    {
      private static Func<TInput, TResult> MapFunc =>
        (Func<TInput, TResult>) MapFuncs[
          Tuple.Create(typeof (TInput), typeof (TResult))];

      [Test]
      public void Test()
        => Assert.True(Option.None<TInput>().Map(MapFunc).IsNone);
    }

    [TestFixture(typeof(int), typeof(string))]
    [TestFixture(typeof(double), typeof(int?))]
    [TestFixture(typeof(long), typeof(object))]
    [TestFixture(typeof(float), typeof(Array))]
    public class FuncResultOfNullAlwaysMapsToNone<TInput, TResult>
    {
      private static Func<TInput, TResult> MapFunc =>
        (Func<TInput, TResult>)MapToNullFuncs[
          Tuple.Create(typeof(TInput), typeof(TResult))];

      [Test]
      public void Test()
        => Assert.True(Option.Some(default(TInput)).Map(MapFunc).IsNone);
    }

    [TestFixture(typeof(int), typeof(string))]
    [TestFixture(typeof(double), typeof(int?))]
    [TestFixture(typeof(long), typeof(object))]
    [TestFixture(typeof(float), typeof(Array))]
    public class NonNullFuncResultAlwaysMapsToSome<TInput, TResult>
    {
      private static Func<TInput, TResult> MapFunc =>
        (Func<TInput, TResult>)MapToNonNullFuncs[
          Tuple.Create(typeof(TInput), typeof(TResult))];

      [Test]
      public void Test()
        => Assert.True(Option.Some(default(TInput)).Map(MapFunc).IsSome);
    }

    [TestFixture(typeof(int), typeof(string))]
    [TestFixture(typeof(double), typeof(int?))]
    [TestFixture(typeof(long), typeof(object))]
    [TestFixture(typeof(float), typeof(Array))]
    public class MappedOptionHasSameValueAsResultOfFunc<TInput, TResult>
    {
      private static Tuple<Delegate, object, object> TestCase =>
        MapToSomeTestCases[Tuple.Create(typeof(TInput), typeof(TResult))];

      private static Func<TInput, TResult> MapFunc
        => (Func<TInput, TResult>)TestCase.Item1;

      private static TInput Input => (TInput)TestCase.Item2;

      private static TResult ExpectedValue => (TResult)TestCase.Item3;

      [Test]
      public void Test()
        => Assert.That(
          Option.Some(Input).Map(MapFunc).GetOrElse(default(TResult)),
          Is.EqualTo(ExpectedValue));
    }

    [TestFixture(typeof (int), 73)]
    [TestFixture(typeof (string), null)]
    [TestFixture(typeof(double), 7.3)]
    [TestFixture(typeof(long?), null)]
    public class OfIsReturnOperationForOption<T>
    {
      private readonly T _value;

      public OfIsReturnOperationForOption(T value)
      {
        _value = value;
      }

      [Test]
      public void Test()
        => Assert.That(new Option<T>(_value), Is.EqualTo(Option.Of(_value)));
    }

    [TestFixture(typeof(int), typeof(double))]
    [TestFixture(typeof(double), typeof(string))]
    [TestFixture(typeof(long), typeof(long))]
    [TestFixture(typeof(object), typeof(Array))]
    public class NoneAlwaysBindsToNone<TInput, TResult>
    {
      private static Func<TInput, Option<TResult>> BindFunc =>
        (Func<TInput, Option<TResult>>)BindFuncs[
          Tuple.Create(typeof(TInput), typeof(TResult))];

      [Test]
      public void Test()
        => Assert.True(Option.None<TInput>().Bind(BindFunc).IsNone);
    }

    [TestFixture(typeof (int), typeof (string))]
    [TestFixture(typeof (double), typeof (int?))]
    [TestFixture(typeof (long), typeof (object))]
    [TestFixture(typeof (float), typeof (decimal))]
    public class BindReturnsResultsOfFunc<TInput, TResult>
    {
      private static Tuple<Delegate, object, object> TestCase =>
        BindTestCases[Tuple.Create(typeof (TInput), typeof (TResult))];

      private static Func<TInput, Option<TResult>> BindFunc
        => (Func<TInput, Option<TResult>>) TestCase.Item1;

      private static TInput Input => (TInput) TestCase.Item2;

      private static Option<TResult> ExpectedOption
        => (Option<TResult>) TestCase.Item3;

      [Test]
      public void Test()
        => Assert.That(
          Option.Some(Input).Bind(BindFunc), Is.EqualTo(ExpectedOption));
    }

    private static readonly Dictionary<Tuple<Type, Type>, Delegate>
      MapFuncs = new Dictionary<Tuple<Type, Type>, Delegate>
      {
        [Tuple.Create(typeof (int), typeof (double))]
          = (Func<int, double>) (x => x),
        [Tuple.Create(typeof (double), typeof (string))]
          = (Func<double, string>) (x => $"{x}"),
        [Tuple.Create(typeof (long), typeof (long))]
          = (Func<long, long>) (x => x),
        [Tuple.Create(typeof (object), typeof (Array))]
          = (Func<object, Array>) (_ => new object[] {})
      };

    private static readonly Dictionary<Tuple<Type, Type>, Delegate>
      MapToNullFuncs = new Dictionary<Tuple<Type, Type>, Delegate>
      {
        [Tuple.Create(typeof(int), typeof(string))]
          = (Func<int, string>)(_ => null),
        [Tuple.Create(typeof(double), typeof(int?))]
          = (Func<double, int?>)(_ => null),
        [Tuple.Create(typeof(long), typeof(object))]
          = (Func<long, object>)(_ => null),
        [Tuple.Create(typeof(float), typeof(Array))]
          = (Func<float, Array>)(_ => null)
      };

    private static readonly Dictionary<Tuple<Type, Type>, Delegate>
      MapToNonNullFuncs = new Dictionary<Tuple<Type, Type>, Delegate>
      {
        [Tuple.Create(typeof (int), typeof (string))]
          = (Func<int, string>) (x => $"{x}"),
        [Tuple.Create(typeof (double), typeof (int?))]
          = (Func<double, int?>) (x => (int) Math.Floor(x)),
        [Tuple.Create(typeof (long), typeof (object))]
          = (Func<long, object>) (x => x),
        [Tuple.Create(typeof (float), typeof (Array))]
          = (Func<float, Array>) (x => new[] {x})
      };

    private static readonly Dictionary<
      Tuple<Type, Type>, Tuple<Delegate, object, object>> MapToSomeTestCases =
        new Dictionary<Tuple<Type, Type>, Tuple<Delegate, object, object>>
        {
          [Tuple.Create(typeof (int), typeof (string))] = Tuple.Create(
            (Delegate) (Func<int, string>) (x => $"{x}"), // map func
            (object) 7,                                   // input value
            (object) "7"),                                // expected result
          [Tuple.Create(typeof (double), typeof (int?))] = Tuple.Create(
            (Delegate) (Func<double, int?>) (x => (int) Math.Floor(x)),
            (object) 7.3,
            (object) 7),
          [Tuple.Create(typeof (long), typeof (object))] = Tuple.Create(
            (Delegate) (Func<long, object>) (x => x),
            (object) 3L,
            (object) 3L),
          [Tuple.Create(typeof (float), typeof (Array))] = Tuple.Create(
            (Delegate) (Func<float, Array>) (x => new[] {x}),
            (object) 1.2f,
            (object) new[] {1.2f})
        };

    private static readonly Dictionary<Tuple<Type, Type>, Delegate>
      BindFuncs = new Dictionary<Tuple<Type, Type>, Delegate>
      {
        [Tuple.Create(typeof (int), typeof (double))]
          = (Func<int, Option<double>>) (x => Option.Of<double>(x)),
        [Tuple.Create(typeof (double), typeof (string))]
          = (Func<double, Option<string>>) (x => Option.Of($"{x}")),
        [Tuple.Create(typeof (long), typeof (long))]
          = (Func<long, Option<long>>) Option.Of,
        [Tuple.Create(typeof (object), typeof (Array))]
          = (Func<object, Option<Array>>) (
            _ => Option.Of<Array>(new object[] {}))
      };

    private static readonly Dictionary<
      Tuple<Type, Type>, Tuple<Delegate, object, object>> BindTestCases =
        new Dictionary<Tuple<Type, Type>, Tuple<Delegate, object, object>>
        {
          [Tuple.Create(typeof(int), typeof(string))] = Tuple.Create(
            (Delegate) (Func<int, Option<string>>)(x => Option.Of($"{x}")),
            (object)7,
            (object)Option.Of("7")),
          [Tuple.Create(typeof(double), typeof(int?))] = Tuple.Create(
            (Delegate)(Func<double, Option<int?>>)(
              x => Option.Of<int?>((int)Math.Floor(x))),
            (object)7.3,
            (object)Option.Of<int?>(7)),
          [Tuple.Create(typeof(long), typeof(object))] = Tuple.Create(
            (Delegate)(Func<long, Option<object>>)(x => Option.Of<object>(x)),
            (object)3L,
            (object)Option.Of<object>(3L)),
          [Tuple.Create(typeof(float), typeof(decimal))] = Tuple.Create(
            (Delegate)(Func<float, Option<decimal>>)(
              x => Option.Of((decimal)x)),
            (object)1.2f,
            (object)Option.Of(1.2m))
        };

    private static IEnumerable<dynamic> OptionTestCases
      => NoneTestCases.Union(SomeTestCases);

    private static IEnumerable<dynamic>
      SomeAndNoneOfSameContainedTypeTestsCases
      // Assumed types are in same order for zipped enumerables.
      => SomeTestCases.Zip(
        NoneTestCases, (s, n) => new {Some = s.Option, None = n.Option});

    private static IEnumerable<dynamic> EqualitySymmetricalTestCases
    {
      get
      {
        var options = OptionTestCases.Select(x => x.Option).ToArray();
        return options.SelectMany(
          x => options.Select(other => new {Left = x, Right = other}));
      }
    }

    private static IEnumerable<dynamic> NoneTestCases
      => new[]
      {
        NoneTestCase<int>(),
        NoneTestCase<string>(),
        NoneTestCase<double>()
      };

    private static IEnumerable<dynamic> SomeTestCases
      => new[]
      {
        SomeTestCase(3),
        SomeTestCase("a string"),
        SomeTestCase(3.0)
      };

    private static dynamic NoneTestCase<T>()
      => new {Option = Option.None<T>(), Type = typeof (T)};

    private static dynamic SomeTestCase<T>(T value)
      => new {Option = Option.Some(value), Type = typeof (T)};
  }
}
