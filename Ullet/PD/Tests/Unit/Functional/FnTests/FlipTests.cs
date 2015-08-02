/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Ullet.PD.Functional.Tests.Unit.FnTests
{
  [TestFixture]
  public class FlipTests
  {
    [Test]
    public void FlipOrderFirstTwoOfTwoParameters()
    {
      Func<int, double, double> divide = (a, b) => a / b;

      // ReSharper disable once InvokeAsExtensionMethod
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
      // ReSharper disable once InvokeAsExtensionMethod
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
      // ReSharper disable once InvokeAsExtensionMethod
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

      // ReSharper disable once InvokeAsExtensionMethod
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

      // ReSharper disable once InvokeAsExtensionMethod
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

      // ReSharper disable once InvokeAsExtensionMethod
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
      // ReSharper disable once InvokeAsExtensionMethod
      Func<Func<int, int, int>, int, IEnumerable<int>, int>
        flippedAggregate = Fn.FlipAll(aggregate);

      var list = new[] { 2, 3, 5 };
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

      // ReSharper disable once InvokeAsExtensionMethod
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

      // ReSharper disable once InvokeAsExtensionMethod
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

    [Test]
    public void FlipOrderFirstTwoOfTwoParametersForCurriedFunction()
    {
      Func<int, Func<double, double>> divide = a => b => a / b;

      // ReSharper disable once InvokeAsExtensionMethod
      Func<double, Func<int, double>> flipped = Fn.Flip(divide);

      Assert.That(flipped(4.0)(1), Is.EqualTo(0.25));
    }

    [Test]
    public void FlipOrderFirstTwoOfThreeParametersForCurriedFunction()
    {
      Func<byte, Func<int, Func<long, double>>> sum =
        a => b => c => (double) a + b + c;

      // ReSharper disable once InvokeAsExtensionMethod
      Func<int, Func<byte, Func<long, double>>> flipped = Fn.Flip(sum);

      Assert.That(flipped(256)(1)(33L), Is.EqualTo(290));
    }

    [Test]
    public void FlipOrderFirstTwoOfTwentyParametersForCurriedFunction()
    {
      /*
       * Ridiculous example to demonstrate no limit on number of parameters for
       * a function in curried form without having to implement lots of method
       * overloads.
       */

      Func<int, Func<int, Func<int, Func<int, Func<int, Func<int, Func<int,
        Func<int, Func<int, Func<int, Func<int, Func<int, Func<int, Func<int,
          Func<int, Func<int, Func<int, Func<int, Func<int, Func<int,
            string>>>>>>>>>>>>>>>>>>>> concat =
              a => b => c => d => e => f => g => h => i => j =>
                k => l => m => n => o => p => q => r => s => t =>
                  a.ToString() + b + c + d + e + f + g + h + i + j +
                  k + l + m + n + o + p + q + r + s + t;

      var flipped = Fn.Flip(concat);

      Assert.That(
        flipped(1)(2)(3)(4)(5)(6)(7)(8)(9)(0)(0)(0)(0)(0)(0)(0)(0)(0)(0)(0),
        Is.EqualTo("21345678900000000000"));
    }

    [Test]
    public void FlipOrderFirstTwoOfTwoParametersForCurriedFunctionAsExtMethod()
    {
      Func<int, Func<double, double>> divide = a => b => a / b;

      Func<double, Func<int, double>> flipped = divide.Flip();

      Assert.That(flipped(4.0)(1), Is.EqualTo(0.25));
    }

    [Test]
    public void FlipOrderFirst2Of3ParametersForCurriedFunctionAsExtMethod()
    {
      Func<byte, Func<int, Func<long, double>>> sum =
        a => b => c => (double)a + b + c;

      Func<int, Func<byte, Func<long, double>>> flipped = sum.Flip();

      Assert.That(flipped(256)(1)(33L), Is.EqualTo(290));
    }
  }
}
