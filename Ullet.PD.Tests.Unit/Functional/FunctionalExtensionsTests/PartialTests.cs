/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Linq;
using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.FunctionalExtensionsTests
{
  [TestFixture]
  public class PartialTests
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
      Func<int, int, double> divide = (a, b) => (double) a/b;

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
      Func<int, int, int, double> expression = (a, b, c) => (a + b)/(double) c;

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
  }
}
