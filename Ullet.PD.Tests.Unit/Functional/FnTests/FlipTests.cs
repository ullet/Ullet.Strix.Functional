/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.FnTests
{
  [TestFixture]
  public class FlipTests
  {
    [Test]
    public void FlipOrderFirstTwoOfTwoParameters()
    {
      Func<int, double, double> divide = (a, b) => a/b;

      Func<double, int, double> flipped = Fn.Flip(divide);

      Assert.That(flipped(4.0, 1), Is.EqualTo(0.25));
    }

    [Test]
    public void FlipOrderFirstTwoOfTwoParametersAsExtensionMethod()
    {
      Func<int, double, double> divide = (a, b) => a / b;

      Func<double, int, double> flipped = divide.Flip();

      Assert.That(flipped(4.0, 1), Is.EqualTo(0.25));
    }

    [Test]
    public void FlipOrderFirstTwoOfThreeParameters()
    {
      // (list, seed, aggregator) => result
      Func<IEnumerable<int>, int, Func<int, int, int>, int>
        aggregate = System.Linq.Enumerable.Aggregate;

      // (aggregator, seed, list) => result
      Func<int, IEnumerable<int>, Func<int, int, int>, int>
        flippedAggregate = Fn.Flip(aggregate);

      var list = new[] { 2, 3, 5 };
      const int seed = 210;
      Func<int, int, int> aggregator = (acc, x) => acc / x;
      Assert.That(
        flippedAggregate(seed, list, aggregator),
        Is.EqualTo(aggregate(list, seed, aggregator)));
      Assert.That(flippedAggregate(seed, list, aggregator), Is.EqualTo(7));
    }

    [Test]
    public void FlipOrderFirstTwoOfThreeParametersAsExtensionMethod()
    {
      // (list, seed, aggregator) => result
      Func<IEnumerable<int>, int, Func<int, int, int>, int>
        aggregate = System.Linq.Enumerable.Aggregate;

      // (aggregator, seed, list) => result
      Func<int, IEnumerable<int>, Func<int, int, int>, int>
        flippedAggregate = Fn.Flip(aggregate);

      var list = new[] { 2, 3, 5 };
      const int seed = 210;
      Func<int, int, int> aggregator = (acc, x) => acc / x;
      Assert.That(
        flippedAggregate(seed, list, aggregator),
        Is.EqualTo(aggregate(list, seed, aggregator)));
      Assert.That(flippedAggregate(seed, list, aggregator), Is.EqualTo(7));
    }

    [Test]
    public void FlipOrderFirstTwoOfFourParameters()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, char, char, char, string> flippedConcat = Fn.Flip(concat);

      Assert.That(flippedConcat('a', 'b', 'c', 'd'), Is.EqualTo("bacd"));
    }

    [Test]
    public void FlipOrderFirstTwoOfFourParametersAsExtensionMethod()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, char, char, char, string> flippedConcat = concat.Flip();

      Assert.That(flippedConcat('a', 'b', 'c', 'd'), Is.EqualTo("bacd"));
    }

    [Test]
    public void FlipOrderFirstTwoOfFiveParameters()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, char, char, char, string> flippedConcat =
        Fn.Flip(concat);

      Assert.That(flippedConcat('a', 'b', 'c', 'd', 'e'), Is.EqualTo("bacde"));
    }

    [Test]
    public void FlipOrderFirstTwoOfFiveParametersAsExtensionMethod()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, char, char, char, string> flippedConcat = concat.Flip();

      Assert.That(flippedConcat('a', 'b', 'c', 'd', 'e'), Is.EqualTo("bacde"));
    }

    [Test]
    public void FlipOrderAllOfTwoParameters()
    {
      Func<int, double, double> divide = (a, b) => a / b;

      Func<double, int, double> flipped = Fn.FlipAll(divide);

      Assert.That(flipped(4.0, 1), Is.EqualTo(0.25));
    }

    [Test]
    public void FlipOrderAllOfTwoParametersAsExtensionMethod()
    {
      Func<int, double, double> divide = (a, b) => a / b;

      Func<double, int, double> flipped = divide.FlipAll();

      Assert.That(flipped(4.0, 1), Is.EqualTo(0.25));
    }

    [Test]
    public void FlipOrderAllOfThreeParameters()
    {
      // (list, seed, aggregator) => result
      Func<IEnumerable<int>, int, Func<int, int, int>, int>
        aggregate = System.Linq.Enumerable.Aggregate;

      // (aggregator, seed, list) => result
      Func<Func<int, int, int>, int, IEnumerable<int>, int>
        flippedAggregate = Fn.FlipAll(aggregate);

      var list = new[] {2, 3, 5};
      const int seed = 210;
      Func<int, int, int> aggregator = (acc, x) => acc/x;
      Assert.That(
        flippedAggregate(aggregator, seed, list),
        Is.EqualTo(aggregate(list, seed, aggregator)));
      Assert.That(flippedAggregate(aggregator, seed, list), Is.EqualTo(7));
    }

    [Test]
    public void FlipOrderAllOfThreeParametersAsExtensionMethod()
    {
      // (list, seed, aggregator) => result
      Func<IEnumerable<int>, int, Func<int, int, int>, int>
        aggregate = System.Linq.Enumerable.Aggregate;

      // (aggregator, seed, list) => result
      Func<Func<int, int, int>, int, IEnumerable<int>, int>
        flippedAggregate = aggregate.FlipAll();

      var list = new[] { 2, 3, 5 };
      const int seed = 210;
      Func<int, int, int> aggregator = (acc, x) => acc / x;
      Assert.That(
        flippedAggregate(aggregator, seed, list),
        Is.EqualTo(aggregate(list, seed, aggregator)));
      Assert.That(flippedAggregate(aggregator, seed, list), Is.EqualTo(7));
    }

    [Test]
    public void FlipOrderAllOfFourParameters()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, char, char, char, string> flippedConcat = Fn.FlipAll(concat);

      Assert.That(flippedConcat('a', 'b', 'c', 'd'), Is.EqualTo("dcba"));
    }

    [Test]
    public void FlipOrderAllOfFourParametersAsExtensionMethod()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, char, char, char, string> flippedConcat = concat.FlipAll();

      Assert.That(flippedConcat('a', 'b', 'c', 'd'), Is.EqualTo("dcba"));
    }

    [Test]
    public void FlipOrderAllOfFiveParameters()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, char, char, char, string> flippedConcat =
        Fn.FlipAll(concat);

      Assert.That(flippedConcat('a', 'b', 'c', 'd', 'e'), Is.EqualTo("edcba"));
    }

    [Test]
    public void FlipOrderAllOfFiveParametersAsExtensionMethod()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, char, char, char, string> flippedConcat =
        concat.FlipAll();

      Assert.That(flippedConcat('a', 'b', 'c', 'd', 'e'), Is.EqualTo("edcba"));
    }
  }
}
