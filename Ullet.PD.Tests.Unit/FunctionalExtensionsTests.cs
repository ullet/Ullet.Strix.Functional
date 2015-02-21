/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Ullet.PD.Tests.Unit
{
  [TestFixture]
  public class FunctionalExtensionsTests
  {
    [Test]
    public void PartialFromOneParameterFunctionAndOneParameter()
    {
      Func<int, string> toString = i => i.ToString();

      var string123 = toString.Partial(123);

      Assert.That(string123(), Is.EqualTo("123"));
    }

    [Test]
    public void PartialFromTwoParameterFunctionAndOneParameter()
    {
      Func<int, int, int> subtract = (a, b) => a - b;

      var subtractFrom100 = subtract.Partial(100);

      Assert.That(subtractFrom100(10), Is.EqualTo(90));
    }

    [Test]
    public void PartialFromTwoParameterFunctionAndTwoParameters()
    {
      Func<int, int, double> divide = (a, b) => (double)a / b;

      var threeQuarters = divide.Partial(3, 4);

      Assert.That(threeQuarters(), Is.EqualTo(0.75));
    }

    [Test]
    public void PartialFromThreeParameterFunctionAndOneParameter()
    {
      Func<string, string, string, string> replace =
        (oldValue, newValue, s) => s.Replace(oldValue, newValue);

      var replaceAllBs = replace.Partial("b");

      Assert.That(replaceAllBs("g", "wibble"), Is.EqualTo("wiggle"));
    }

    [Test]
    public void PartialFromThreeParameterFunctionAndTwoParameters()
    {
      Func<int, int, int, double> expression = (a, b, c) => (a + b)/(double)c;

      var divide150ByX = expression.Partial(100, 50);

      Assert.That(divide150ByX(5), Is.EqualTo(30));
    }

    [Test]
    public void PartialFromThreeParameterFunctionAndThreeParameters()
    {
      Func<char, char, char, string> concat = (a, b, c) => a.ToString() + b + c;

      var oneTwoThree = concat.Partial('1', '2', '3');

      Assert.That(oneTwoThree(), Is.EqualTo("123"));
    }

    [Test]
    public void ConstructedPartialFunctionNotEvaluatedUntilCalled()
    {
      Func<int[], int[]> times2 = numbers => numbers.Select(n => n*2).ToArray();
      var mutable = new[] {1};

      Func<int[]> oneTimes2 = times2.Partial(mutable);
      mutable[0] = 3;

      Assert.That(oneTimes2(), Is.EqualTo(new[] {6}));
    }

    [Test]
    public void CompositeOfLeftAfterRight()
    {
      Func<int, int> square = x => x*x;
      Func<int[], int> sum = a => a.Sum();

      var squareOfSum = square.After(sum);

      Assert.That(squareOfSum(new []{1,2,3,4}), Is.EqualTo(100));
    }

    [Test]
    public void CompositeOfMultipleLeftAfterRight()
    {
      Func<int, int> add1 = x => x + 1;
      Func<int, int> times2 = x => x*2;
      Func<int, int> square = x => x*x;
      Func<int, int> subtract10 = x => x - 10;

      Func<int, int> expectedComposite = x => (((x - 10) * (x - 10)) * 2) + 1;

      var composite = (new[] {add1, times2, square, subtract10}).Compose();

      Assert.That(composite(12), Is.EqualTo(expectedComposite(12)));
      Assert.That(composite(12), Is.EqualTo(9));
    }

    [Test]
    public void CompositeOfMultipleRightAfterLeft()
    {
      Func<int, int> add1 = x => x + 1;
      Func<int, int> times2 = x => x * 2;
      Func<int, int> square = x => x * x;
      Func<int, int> subtract10 = x => x - 10;

      Func<int, int> expectedComposite = x => (((x + 1)*2)*((x + 1)*2)) - 10;

      var composite = 
        (new[] { add1, times2, square, subtract10 }).ComposeReverse();

      Assert.That(composite(12), Is.EqualTo(expectedComposite(12)));
      Assert.That(composite(12), Is.EqualTo(666));
    }

    [Test]
    public void FlipParameterOrderForTwoParameters()
    {
      Func<int, double, double> divide = (a, b) => a/b;

      Func<double, int, double> flipped = divide.Flip();

      Assert.That(flipped(4.0, 1), Is.EqualTo(0.25));
    }

    [Test]
    public void FlipParameterOrderForThreeParameters()
    {
      // (list, seed, aggregator) => result
      Func<IEnumerable<int>, int, Func<int, int, int>, int>
        aggregate = Enumerable.Aggregate;

      // (aggregator, seed, list) => result
      Func<Func<int, int, int>, int, IEnumerable<int>, int>
        flippedAggregate = aggregate.Flip();

      var list = new[] {2, 3, 5};
      const int seed = 210;
      Func<int, int, int> aggregator = (acc, x) => acc/x;
      Assert.That(
        flippedAggregate(aggregator, seed, list),
        Is.EqualTo(aggregate(list, seed, aggregator)));
      Assert.That(flippedAggregate(aggregator, seed, list), Is.EqualTo(7));
    }
  }
}
