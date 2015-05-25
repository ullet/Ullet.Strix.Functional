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
    public void PartialFromFourParameterFunctionAndOneParameter()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, char, char, string> concatToOne = Fn.Partial(concat, '1');

      Assert.That(concatToOne('2', '3', '4'), Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterFunctionAndTwoParameters()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, char, string> concatToOneTwo =
        Fn.Partial(concat, '1', '2');

      Assert.That(concatToOneTwo('3', '4'), Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterFunctionAndThreeParameters()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, string> concatToOneTwoThree =
        Fn.Partial(concat, '1', '2', '3');

      Assert.That(concatToOneTwoThree('4'), Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterFunctionAndFourParameters()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<string> oneTwoThreeFour =
        Fn.Partial(concat, '1', '2', '3', '4');

      Assert.That(oneTwoThreeFour(), Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndOneParameter()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, char, char, string> concatToOne =
        Fn.Partial(concat, '1');

      Assert.That(concatToOne('2', '3', '4', '5'), Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndTwoParameters()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, char, string> concatToOneTwo =
        Fn.Partial(concat, '1', '2');

      Assert.That(concatToOneTwo('3', '4', '5'), Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndThreeParameters()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, string> concatToOneTwoThree =
        Fn.Partial(concat, '1', '2', '3');

      Assert.That(concatToOneTwoThree('4', '5'), Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndFourParameters()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, string> concatToOneTwoThreeFour =
        Fn.Partial(concat, '1', '2', '3', '4');

      Assert.That(concatToOneTwoThreeFour('5'), Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndFiveParameters()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<string> oneTwoThreeFourFive =
        Fn.Partial(concat, '1', '2', '3', '4', '5');

      Assert.That(oneTwoThreeFourFive(), Is.EqualTo("12345"));
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
      int result = 0;
      Action<int> initVar = x => result = x;

      var initVarTo123 = Fn.Partial(initVar, 123);

      initVarTo123();
      Assert.That(result, Is.EqualTo(123));
    }

    [Test]
    public void PartialFromTwoParameterActionAndOneParameter()
    {
      int result = 0;
      Action<int, int> initToDiff = (a, b) => result = a - b;

      var initSubtractingFrom100 = Fn.Partial(initToDiff, 100);

      initSubtractingFrom100(10);
      Assert.That(result, Is.EqualTo(90));
    }

    [Test]
    public void PartialFromTwoParameterActionAndTwoParameters()
    {
      double result = 0;
      Action<int, int> initToAdivB = (a, b) => result = (double)a / b;

      var initToThreeQuarters = Fn.Partial(initToAdivB, 3, 4);

      initToThreeQuarters();
      Assert.That(result, Is.EqualTo(0.75));
    }

    [Test]
    public void PartialFromThreeParameterActionAndOneParameter()
    {
      string result = null;
      Action<string, string, string> initWithReplace =
        (oldValue, newValue, s) => result = s.Replace(oldValue, newValue);

      var initWithReplaceAllBs = Fn.Partial(initWithReplace, "b");

      initWithReplaceAllBs("g", "wibble");
      Assert.That(result, Is.EqualTo("wiggle"));
    }

    [Test]
    public void PartialFromThreeParameterActionAndTwoParameters()
    {
      double result = 0;
      Action<int, int, int> initFromExpression =
        (a, b, c) => result = (a + b)/(double) c;

      var initByDivide150ByX = Fn.Partial(initFromExpression, 100, 50);

      initByDivide150ByX(5);
      Assert.That(result, Is.EqualTo(30));
    }

    [Test]
    public void PartialFromThreeParameterActionAndThreeParameters()
    {
      string result = null;
      Action<char, char, char> initFromConcat =
        (a, b, c) => result = a.ToString() + b + c;

      var initOneTwoThree = Fn.Partial(initFromConcat, '1', '2', '3');

      initOneTwoThree();
      Assert.That(result, Is.EqualTo("123"));
    }

    [Test]
    public void PartialFromFourParameterActionAndOneParameter()
    {
      string result = null;
      Action<char, char, char, char> initFromConcat =
        (a, b, c, d) => result = a.ToString() + b + c + d;

      var initConcatToOne = Fn.Partial(initFromConcat, '1');

      initConcatToOne('2', '3', '4');
      Assert.That(result, Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterActionAndTwoParameters()
    {
      string result = null;
      Action<char, char, char, char> initFromConcat =
        (a, b, c, d) => result = a.ToString() + b + c + d;

      var initConcatToOneTwo = Fn.Partial(initFromConcat, '1', '2');

      initConcatToOneTwo('3', '4');
      Assert.That(result, Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterActionAndThreeParameters()
    {
      string result = null;
      Action<char, char, char, char> initFromConcat =
        (a, b, c, d) => result = a.ToString() + b + c + d;

      var initConcatToOneTwoThree = Fn.Partial(initFromConcat, '1', '2', '3');

      initConcatToOneTwoThree('4');
      Assert.That(result, Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterActionAndFourParameters()
    {
      string result = null;
      Action<char, char, char, char> initFromConcat =
        (a, b, c, d) => result = a.ToString() + b + c + d;

      var initToOneTwoThreeFour =
        Fn.Partial(initFromConcat, '1', '2', '3', '4');

      initToOneTwoThreeFour();
      Assert.That(result, Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndOneParameter()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initConcatToOne = Fn.Partial(initFromConcat, '1');

      initConcatToOne('2', '3', '4', '5');
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndTwoParameters()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initConcatToOneTwo = Fn.Partial(initFromConcat, '1', '2');

      initConcatToOneTwo('3', '4', '5');
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndThreeParameters()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initConcatToOneTwoThree = Fn.Partial(initFromConcat, '1', '2', '3');

      initConcatToOneTwoThree('4', '5');
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndFourParameters()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initConcatToOneTwoThreeFour =
        Fn.Partial(initFromConcat, '1', '2', '3', '4');

      initConcatToOneTwoThreeFour('5');
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndFiveParameters()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initToOneTwoThreeFourFive =
        Fn.Partial(initFromConcat, '1', '2', '3', '4', '5');

      initToOneTwoThreeFourFive();
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void ConstructedPartialActionNotEvaluatedUntilCalled()
    {
      int[] result = null;
      Action<int[]> initTimes2 =
        numbers => result = numbers.Select(n => n * 2).ToArray();
      var mutable = new[] { 1 };

      Action initOneTimes2 = Fn.Partial(initTimes2, mutable);
      mutable[0] = 3;

      initOneTimes2();
      Assert.That(result, Is.EqualTo(new[] { 6 }));
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
    public void PartialFromFourParameterFunctionAndOneParameterAsExtMethod()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, char, char, string> concatToOne = concat.Partial('1');

      Assert.That(concatToOne('2', '3', '4'), Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterFunctionAndTwoParametersAsExtMethod()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, char, string> concatToOneTwo = concat.Partial('1', '2');

      Assert.That(concatToOneTwo('3', '4'), Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterFunctionAndThreeParametersAsExtMethod()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<char, string> concatToOneTwoThree =
        concat.Partial('1', '2', '3');

      Assert.That(concatToOneTwoThree('4'), Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterFunctionAndFourParametersAsExtMethod()
    {
      Func<char, char, char, char, string> concat =
        (a, b, c, d) => a.ToString() + b + c + d;

      Func<string> oneTwoThreeFour =
        concat.Partial('1', '2', '3', '4');

      Assert.That(oneTwoThreeFour(), Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndOneParameterAsExtMethod()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, char, char, string> concatToOne =
        concat.Partial('1');

      Assert.That(concatToOne('2', '3', '4', '5'), Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndTwoParametersAsExtMethod()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, char, string> concatToOneTwo =
        concat.Partial('1', '2');

      Assert.That(concatToOneTwo('3', '4', '5'), Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndThreeParametersAsExtMethod()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, char, string> concatToOneTwoThree =
        concat.Partial('1', '2', '3');

      Assert.That(concatToOneTwoThree('4', '5'), Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndFourParametersAsExtMethod()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<char, string> concatToOneTwoThreeFour =
        concat.Partial('1', '2', '3', '4');

      Assert.That(concatToOneTwoThreeFour('5'), Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterFunctionAndFiveParametersAsExtMethod()
    {
      Func<char, char, char, char, char, string> concat =
        (a, b, c, d, e) => a.ToString() + b + c + d + e;

      Func<string> oneTwoThreeFourFive =
        concat.Partial('1', '2', '3', '4', '5');

      Assert.That(oneTwoThreeFourFive(), Is.EqualTo("12345"));
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
      int result = 0;
      Action<int> initVar = x => result = x;

      var initVarTo123 = initVar.Partial(123);

      initVarTo123();
      Assert.That(result, Is.EqualTo(123));
    }

    [Test]
    public void PartialFromTwoParameterActionAndOneParameterAsExtMethod()
    {
      int result = 0;
      Action<int, int> initToDiff = (a, b) => result = a - b;

      var initSubtractingFrom100 = initToDiff.Partial(100);

      initSubtractingFrom100(10);
      Assert.That(result, Is.EqualTo(90));
    }

    [Test]
    public void PartialFromTwoParameterActionAndTwoParametersAsExtMethod()
    {
      double result = 0;
      Action<int, int> initToAdivB = (a, b) => result = (double)a / b;

      var initToThreeQuarters = initToAdivB.Partial(3, 4);

      initToThreeQuarters();
      Assert.That(result, Is.EqualTo(0.75));
    }

    [Test]
    public void PartialFromThreeParameterActionAndOneParameterAsExtMethod()
    {
      string result = null;
      Action<string, string, string> initWithReplace =
        (oldValue, newValue, s) => result = s.Replace(oldValue, newValue);

      var initWithReplaceAllBs = initWithReplace.Partial("b");

      initWithReplaceAllBs("g", "wibble");
      Assert.That(result, Is.EqualTo("wiggle"));
    }

    [Test]
    public void PartialFromThreeParameterActionAndTwoParametersAsExtMethod()
    {
      double result = 0;
      Action<int, int, int> initFromExpression =
        (a, b, c) => result = (a + b) / (double)c;

      var initByDivide150ByX = initFromExpression.Partial(100, 50);

      initByDivide150ByX(5);
      Assert.That(result, Is.EqualTo(30));
    }

    [Test]
    public void PartialFromThreeParameterActionAndThreeParametersAsExtMethod()
    {
      string result = null;
      Action<char, char, char> initFromConcat =
        (a, b, c) => result = a.ToString() + b + c;

      var initOneTwoThree = initFromConcat.Partial('1', '2', '3');

      initOneTwoThree();
      Assert.That(result, Is.EqualTo("123"));
    }

    [Test]
    public void PartialFromFourParameterActionAndOneParameterAsExtMethod()
    {
      string result = null;
      Action<char, char, char, char> initFromConcat =
        (a, b, c, d) => result = a.ToString() + b + c + d;

      var initConcatToOne = initFromConcat.Partial('1');

      initConcatToOne('2', '3', '4');
      Assert.That(result, Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterActionAndTwoParametersAsExtMethod()
    {
      string result = null;
      Action<char, char, char, char> initFromConcat =
        (a, b, c, d) => result = a.ToString() + b + c + d;

      var initConcatToOneTwo = initFromConcat.Partial('1', '2');

      initConcatToOneTwo('3', '4');
      Assert.That(result, Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterActionAndThreeParametersAsExtMethod()
    {
      string result = null;
      Action<char, char, char, char> initFromConcat =
        (a, b, c, d) => result = a.ToString() + b + c + d;

      var initConcatToOneTwoThree = initFromConcat.Partial('1', '2', '3');

      initConcatToOneTwoThree('4');
      Assert.That(result, Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFourParameterActionAndFourParametersAsExtMethod()
    {
      string result = null;
      Action<char, char, char, char> initFromConcat =
        (a, b, c, d) => result = a.ToString() + b + c + d;

      var initToOneTwoThreeFour = initFromConcat.Partial('1', '2', '3', '4');

      initToOneTwoThreeFour();
      Assert.That(result, Is.EqualTo("1234"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndOneParameterAsExtMethod()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initConcatToOne = initFromConcat.Partial('1');

      initConcatToOne('2', '3', '4', '5');
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndTwoParametersAsExtMethod()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initConcatToOneTwo = initFromConcat.Partial('1', '2');

      initConcatToOneTwo('3', '4', '5');
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndThreeParametersAsExtMethod()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initConcatToOneTwoThree = initFromConcat.Partial('1', '2', '3');

      initConcatToOneTwoThree('4', '5');
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndFourParametersAsExtMethod()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initConcatToOneTwoThreeFour =
        initFromConcat.Partial('1', '2', '3', '4');

      initConcatToOneTwoThreeFour('5');
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void PartialFromFiveParameterActionAndFiveParametersAsExtMethod()
    {
      string result = null;
      Action<char, char, char, char, char> initFromConcat =
        (a, b, c, d, e) => result = a.ToString() + b + c + d + e;

      var initToOneTwoThreeFourFive =
        initFromConcat.Partial('1', '2', '3', '4', '5');

      initToOneTwoThreeFourFive();
      Assert.That(result, Is.EqualTo("12345"));
    }

    [Test]
    public void ConstructedPartialActionNotEvaluatedUntilCalledAsExtMethod()
    {
      int[] result = null;
      Action<int[]> initTimes2 =
        numbers => result = numbers.Select(n => n * 2).ToArray();
      var mutable = new[] { 1 };

      Action initOneTimes2 = initTimes2.Partial(mutable);
      mutable[0] = 3;

      initOneTimes2();
      Assert.That(result, Is.EqualTo(new[] { 6 }));
    }
  }
}
