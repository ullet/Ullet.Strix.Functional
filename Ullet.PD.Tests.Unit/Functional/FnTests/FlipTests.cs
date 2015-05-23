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
    public void FlipParameterOrderForTwoParameters()
    {
      Func<int, double, double> divide = (a, b) => a/b;

      Func<double, int, double> flipped = Fn.Flip(divide);

      Assert.That(flipped(4.0, 1), Is.EqualTo(0.25));
    }

    [Test]
    public void CanCallFlipAsExtensionMethod()
    {
      Func<int, double, double> divide = (a, b) => a / b;

      Func<double, int, double> flipped = divide.Flip();

      Assert.That(flipped(4.0, 1), Is.EqualTo(0.25));
    }

    [Test]
    public void FlipParameterOrderForThreeParameters()
    {
      // (list, seed, aggregator) => result
      Func<IEnumerable<int>, int, Func<int, int, int>, int>
        aggregate = System.Linq.Enumerable.Aggregate;

      // (aggregator, seed, list) => result
      Func<Func<int, int, int>, int, IEnumerable<int>, int>
        flippedAggregate = Fn.Flip(aggregate);

      var list = new[] {2, 3, 5};
      const int seed = 210;
      Func<int, int, int> aggregator = (acc, x) => acc/x;
      Assert.That(
        flippedAggregate(aggregator, seed, list),
        Is.EqualTo(aggregate(list, seed, aggregator)));
      Assert.That(flippedAggregate(aggregator, seed, list), Is.EqualTo(7));
    }

    [Test]
    public void CanCallFlipForThreeParametersAsExtensionMethod()
    {
      // (list, seed, aggregator) => result
      Func<IEnumerable<int>, int, Func<int, int, int>, int>
        aggregate = System.Linq.Enumerable.Aggregate;

      // (aggregator, seed, list) => result
      Func<Func<int, int, int>, int, IEnumerable<int>, int>
        flippedAggregate = aggregate.Flip();

      var list = new[] { 2, 3, 5 };
      const int seed = 210;
      Func<int, int, int> aggregator = (acc, x) => acc / x;
      Assert.That(
        flippedAggregate(aggregator, seed, list),
        Is.EqualTo(aggregate(list, seed, aggregator)));
      Assert.That(flippedAggregate(aggregator, seed, list), Is.EqualTo(7));
    }
  }
}
