/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Linq;
using NUnit.Framework;

namespace Ullet.Strix.Functional.Tests.Unit.FnTests
{
  [TestFixture]
  public class ComposeTests
  {
    [Test]
    public void ComposeTwoFunctions()
    {
      Func<int, int> square = x => x*x;
      Func<int[], int> sum = a => a.Sum();

      Func<int[], int> squareOfSum = square.Compose(sum);

      Assert.That(squareOfSum(new []{1,2,3,4}), Is.EqualTo(100));
    }

    [Test]
    public void ComposeTwoFunctionsRightAfterLeft()
    {
      Func<int, int> square = x => x * x;
      Func<int[], int> sum = a => a.Sum();

      Func<int[], int> squareOfSum = sum.ComposeReverse(square);

      Assert.That(squareOfSum(new[] { 1, 2, 3, 4 }), Is.EqualTo(100));
    }

    [Test]
    public void AfterIsAliasForCompose()
    {
      Func<int, int> square = x => x * x;
      Func<int[], int> sum = a => a.Sum();

      Func<int[], int> squareOfSum = square.After(sum);

      Assert.That(squareOfSum(new[] { 1, 2, 3, 4 }), Is.EqualTo(100));
    }

    [Test]
    public void BeforeIsAliasForComposeReverse()
    {
      Func<int, int> square = x => x * x;
      Func<int[], int> sum = a => a.Sum();

      Func<int[], int> squareOfSum = sum.Before(square);

      Assert.That(squareOfSum(new[] { 1, 2, 3, 4 }), Is.EqualTo(100));
    }

    [Test]
    public void ComposeMultipleFunctions()
    {
      Func<int, int> add1 = x => x + 1;
      Func<int, int> times2 = x => x*2;
      Func<int, int> square = x => x*x;
      Func<int, int> subtract10 = x => x - 10;

      Func<int, int> expectedComposed = x => (((x - 10)*(x - 10))*2) + 1;

      Func<int, int> composed = Fn.Compose(add1, times2, square, subtract10);

      Assert.That(composed(12), Is.EqualTo(expectedComposed(12)));
      Assert.That(composed(12), Is.EqualTo(9));
    }

    [Test]
    public void ComposeMultipleFunctionsRightAfterLeft()
    {
      Func<int, int> add1 = x => x + 1;
      Func<int, int> times2 = x => x * 2;
      Func<int, int> square = x => x * x;
      Func<int, int> subtract10 = x => x - 10;

      Func<int, int> expectedComposed = x => (((x + 1)*2)*((x + 1)*2)) - 10;

      Func<int, int> composed =
        Fn.ComposeReverse(add1, times2, square, subtract10);

      Assert.That(composed(12), Is.EqualTo(expectedComposed(12)));
      Assert.That(composed(12), Is.EqualTo(666));
    }

    [Test]
    public void ComposeUnaryFunctionWithBinaryFunction()
    {
      Func<int, int, int> sumTwoValues = (x, y) => x + y;
      Func<int, int> square = x => x*x;

      Func<int, int, int> squareOfSum = square.Compose(sumTwoValues);

      Assert.That(squareOfSum(2, 3), Is.EqualTo(25));
    }

    [Test]
    public void ComposeUnaryFunctionWithTernaryFunction()
    {
      Func<int, int, int, int> sumThreeValues = (x, y, z) => x + y + z;
      Func<int, int> square = x => x * x;

      Func<int, int, int, int> squareOfSum = square.Compose(sumThreeValues);

      Assert.That(squareOfSum(2, 3, 4), Is.EqualTo(81));
    }

    [Test]
    public void ComposeActionWithUnaryFunction()
    {
      double result = 0;
      Func<int, double> squareRoot = x => Math.Sqrt(x);
      Action<double> setResult = x => result = x;

      Action<int> setResultToSquare = setResult.Compose(squareRoot);

      setResultToSquare(12);
      Assert.That(result, Is.EqualTo(3.464D).Within(0.0005D));
    }

    [Test]
    public void ResultIsComposeAsCurriedFunction()
    {
      Func<int, double> square = x => x * x;
      Func<int[], int> sum = a => a.Sum();

      Func<Func<int[], int>, Func<int[], double>>
        resultOfSquare = Fn.Result<int, double, int[]>(square);

      var squareOfSum = resultOfSquare(sum);
      Assert.That(squareOfSum(new[] { 1, 2, 3, 4 }), Is.EqualTo(100));
    }
  }
}
