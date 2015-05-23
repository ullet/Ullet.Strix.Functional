/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Linq;
using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.FnTests
{
  [TestFixture]
  public class PartialTests
  {
    [Test]
    public void PartialFromOneParameterFunctionAndOneParameter()
    {
      Func<int, string> toString = i => i.ToString();

      var string123 = Fn.Partial(toString, 123);

      Assert.That(string123(), Is.EqualTo("123"));
    }

    [Test]
    public void PartialFromTwoParameterFunctionAndOneParameter()
    {
      Func<int, int, int> subtract = (a, b) => a - b;

      var subtractFrom100 = Fn.Partial(subtract, 100);

      Assert.That(subtractFrom100(10), Is.EqualTo(90));
    }

    [Test]
    public void PartialFromTwoParameterFunctionAndTwoParameters()
    {
      Func<int, int, double> divide = (a, b) => (double) a/b;

      var threeQuarters = Fn.Partial(divide, 3, 4);

      Assert.That(threeQuarters(), Is.EqualTo(0.75));
    }

    [Test]
    public void PartialFromThreeParameterFunctionAndOneParameter()
    {
      Func<string, string, string, string> replace =
        (oldValue, newValue, s) => s.Replace(oldValue, newValue);

      var replaceAllBs = Fn.Partial(replace, "b");

      Assert.That(replaceAllBs("g", "wibble"), Is.EqualTo("wiggle"));
    }

    [Test]
    public void PartialFromThreeParameterFunctionAndTwoParameters()
    {
      Func<int, int, int, double> expression = (a, b, c) => (a + b)/(double) c;

      var divide150ByX = Fn.Partial(expression, 100, 50);

      Assert.That(divide150ByX(5), Is.EqualTo(30));
    }

    [Test]
    public void PartialFromThreeParameterFunctionAndThreeParameters()
    {
      Func<char, char, char, string> concat = (a, b, c) => a.ToString() + b + c;

      var oneTwoThree = Fn.Partial(concat, '1', '2', '3');

      Assert.That(oneTwoThree(), Is.EqualTo("123"));
    }

    [Test]
    public void ConstructedPartialFunctionNotEvaluatedUntilCalled()
    {
      Func<int[], int[]> times2 = numbers => numbers.Select(n => n*2).ToArray();
      var mutable = new[] {1};

      Func<int[]> oneTimes2 = Fn.Partial(times2, mutable);
      mutable[0] = 3;

      Assert.That(oneTimes2(), Is.EqualTo(new[] {6}));
    }

    [Test]
    public void PartialFromOneParameterActionAndOneParameter()
    {
      int theVar = 0;
      Action<int> initVar = x => theVar = x;

      var initVarTo123 = Fn.Partial(initVar, 123);

      initVarTo123();
      Assert.That(theVar, Is.EqualTo(123));
    }

    [Test]
    public void PartialFromTwoParameterActionAndOneParameter()
    {
      int theVar = 0;
      Action<int, int> initToDiff = (a, b) => theVar = a - b;

      var initSubtractingFrom100 = Fn.Partial(initToDiff, 100);

      initSubtractingFrom100(10);
      Assert.That(theVar, Is.EqualTo(90));
    }

    [Test]
    public void PartialFromTwoParameterActionAndTwoParameters()
    {
      double theVar = 0;
      Action<int, int> initToAdivB = (a, b) => theVar = (double)a / b;

      var initToThreeQuarters = Fn.Partial(initToAdivB, 3, 4);

      initToThreeQuarters();
      Assert.That(theVar, Is.EqualTo(0.75));
    }

    [Test]
    public void PartialFromThreeParameterActionAndOneParameter()
    {
      string theVar = null;
      Action<string, string, string> initWithReplace =
        (oldValue, newValue, s) => theVar = s.Replace(oldValue, newValue);

      var initWithReplaceAllBs = Fn.Partial(initWithReplace, "b");

      initWithReplaceAllBs("g", "wibble");
      Assert.That(theVar, Is.EqualTo("wiggle"));
    }

    [Test]
    public void PartialFromThreeParameterActionAndTwoParameters()
    {
      double theVar = 0;
      Action<int, int, int> initFromExpression =
        (a, b, c) => theVar = (a + b)/(double) c;

      var initByDivide150ByX = Fn.Partial(initFromExpression, 100, 50);

      initByDivide150ByX(5);
      Assert.That(theVar, Is.EqualTo(30));
    }

    [Test]
    public void PartialFromThreeParameterActionAndThreeParameters()
    {
      string theVar = null;
      Action<char, char, char> initFromConcat =
        (a, b, c) => theVar = a.ToString() + b + c;

      var initOneTwoThree = Fn.Partial(initFromConcat, '1', '2', '3');

      initOneTwoThree();
      Assert.That(theVar, Is.EqualTo("123"));
    }

    [Test]
    public void ConstructedPartialActionNotEvaluatedUntilCalled()
    {
      int[] theVar = null;
      Action<int[]> initTimes2 =
        numbers => theVar = numbers.Select(n => n * 2).ToArray();
      var mutable = new[] { 1 };

      Action initOneTimes2 = Fn.Partial(initTimes2, mutable);
      mutable[0] = 3;

      initOneTimes2();
      Assert.That(theVar, Is.EqualTo(new[] { 6 }));
    }

    [Test]
    public void PartialFromOneParameterFunctionAndOneParameterAsExtMethod()
    {
      Func<int, string> toString = i => i.ToString();

      var string123 = toString.Partial(123);

      Assert.That(string123(), Is.EqualTo("123"));
    }

    [Test]
    public void PartialFromTwoParameterFunctionAndOneParameterAsExtMethod()
    {
      Func<int, int, int> subtract = (a, b) => a - b;

      var subtractFrom100 = subtract.Partial(100);

      Assert.That(subtractFrom100(10), Is.EqualTo(90));
    }

    [Test]
    public void PartialFromTwoParameterFunctionAndTwoParametersAsExtMethod()
    {
      Func<int, int, double> divide = (a, b) => (double)a / b;

      var threeQuarters = divide.Partial(3, 4);

      Assert.That(threeQuarters(), Is.EqualTo(0.75));
    }

    [Test]
    public void PartialFromThreeParameterFunctionAndOneParameterAsExtMethod()
    {
      Func<string, string, string, string> replace =
        (oldValue, newValue, s) => s.Replace(oldValue, newValue);

      var replaceAllBs = replace.Partial("b");

      Assert.That(replaceAllBs("g", "wibble"), Is.EqualTo("wiggle"));
    }

    [Test]
    public void PartialFromThreeParameterFunctionAndTwoParametersAsExtMethod()
    {
      Func<int, int, int, double> expression = (a, b, c) => (a + b) / (double)c;

      var divide150ByX = expression.Partial(100, 50);

      Assert.That(divide150ByX(5), Is.EqualTo(30));
    }

    [Test]
    public void PartialFromThreeParameterFunctionAndThreeParametersAsExtMethod()
    {
      Func<char, char, char, string> concat = (a, b, c) => a.ToString() + b + c;

      var oneTwoThree = concat.Partial('1', '2', '3');

      Assert.That(oneTwoThree(), Is.EqualTo("123"));
    }

    [Test]
    public void ConstructedPartialFunctionNotEvaluatedUntilCalledAsExtMethod()
    {
      Func<int[], int[]> times2 = numbers => numbers.Select(n => n * 2).ToArray();
      var mutable = new[] { 1 };

      Func<int[]> oneTimes2 = times2.Partial(mutable);
      mutable[0] = 3;

      Assert.That(oneTimes2(), Is.EqualTo(new[] { 6 }));
    }

    [Test]
    public void PartialFromOneParameterActionAndOneParameterAsExtMethod()
    {
      int theVar = 0;
      Action<int> initVar = x => theVar = x;

      var initVarTo123 = initVar.Partial(123);

      initVarTo123();
      Assert.That(theVar, Is.EqualTo(123));
    }

    [Test]
    public void PartialFromTwoParameterActionAndOneParameterAsExtMethod()
    {
      int theVar = 0;
      Action<int, int> initToDiff = (a, b) => theVar = a - b;

      var initSubtractingFrom100 = initToDiff.Partial(100);

      initSubtractingFrom100(10);
      Assert.That(theVar, Is.EqualTo(90));
    }

    [Test]
    public void PartialFromTwoParameterActionAndTwoParametersAsExtMethod()
    {
      double theVar = 0;
      Action<int, int> initToAdivB = (a, b) => theVar = (double)a / b;

      var initToThreeQuarters = initToAdivB.Partial(3, 4);

      initToThreeQuarters();
      Assert.That(theVar, Is.EqualTo(0.75));
    }

    [Test]
    public void PartialFromThreeParameterActionAndOneParameterAsExtMethod()
    {
      string theVar = null;
      Action<string, string, string> initWithReplace =
        (oldValue, newValue, s) => theVar = s.Replace(oldValue, newValue);

      var initWithReplaceAllBs = initWithReplace.Partial("b");

      initWithReplaceAllBs("g", "wibble");
      Assert.That(theVar, Is.EqualTo("wiggle"));
    }

    [Test]
    public void PartialFromThreeParameterActionAndTwoParametersAsExtMethod()
    {
      double theVar = 0;
      Action<int, int, int> initFromExpression =
        (a, b, c) => theVar = (a + b) / (double)c;

      var initByDivide150ByX = initFromExpression.Partial(100, 50);

      initByDivide150ByX(5);
      Assert.That(theVar, Is.EqualTo(30));
    }

    [Test]
    public void PartialFromThreeParameterActionAndThreeParametersAsExtMethod()
    {
      string theVar = null;
      Action<char, char, char> initFromConcat =
        (a, b, c) => theVar = a.ToString() + b + c;

      var initOneTwoThree = initFromConcat.Partial('1', '2', '3');

      initOneTwoThree();
      Assert.That(theVar, Is.EqualTo("123"));
    }

    [Test]
    public void ConstructedPartialActionNotEvaluatedUntilCalledAsExtMethod()
    {
      int[] theVar = null;
      Action<int[]> initTimes2 =
        numbers => theVar = numbers.Select(n => n * 2).ToArray();
      var mutable = new[] { 1 };

      Action initOneTimes2 = initTimes2.Partial(mutable);
      mutable[0] = 3;

      initOneTimes2();
      Assert.That(theVar, Is.EqualTo(new[] { 6 }));
    }
  }
}
