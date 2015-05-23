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
  public class ComposeTests
  {
    [Test]
    public void ComposeTwoFunctions()
    {
      Func<int, int> square = x => x*x;
      Func<int[], int> sum = a => a.Sum();

      var squareOfSum = Fn.Compose(square, sum);

      Assert.That(squareOfSum(new []{1,2,3,4}), Is.EqualTo(100));
    }

    [Test]
    public void ComposeTwoFunctionsRightAfterLeft()
    {
      Func<int, int> square = x => x * x;
      Func<int[], int> sum = a => a.Sum();

      var squareOfSum = Fn.ComposeReverse(sum, square);

      Assert.That(squareOfSum(new[] { 1, 2, 3, 4 }), Is.EqualTo(100));
    }

    [Test]
    public void ComposeCanBeCalledAsAnExtensionMethod()
    {
      Func<int, int> square = x => x * x;
      Func<int[], int> sum = a => a.Sum();

      var squareOfSum = square.Compose(sum);

      Assert.That(squareOfSum(new[] { 1, 2, 3, 4 }), Is.EqualTo(100));
    }

    [Test]
    public void ComposeReverseCanBeCalledAsAnExtensionMethod()
    {
      Func<int, int> square = x => x * x;
      Func<int[], int> sum = a => a.Sum();

      var squareOfSum = sum.ComposeReverse(square);

      Assert.That(squareOfSum(new[] { 1, 2, 3, 4 }), Is.EqualTo(100));
    }

    [Test]
    public void AfterIsAliasForCompose()
    {
      Func<int, int> square = x => x * x;
      Func<int[], int> sum = a => a.Sum();

      var squareOfSum = square.After(sum);

      Assert.That(squareOfSum(new[] { 1, 2, 3, 4 }), Is.EqualTo(100));
    }

    [Test]
    public void BeforeIsAliasForComposeReverse()
    {
      Func<int, int> square = x => x * x;
      Func<int[], int> sum = a => a.Sum();

      var squareOfSum = sum.Before(square);

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

      var composed = Fn.Compose(add1, times2, square, subtract10);

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

      var composed = Fn.ComposeReverse(add1, times2, square, subtract10);

      Assert.That(composed(12), Is.EqualTo(expectedComposed(12)));
      Assert.That(composed(12), Is.EqualTo(666));
    }

    [Test]
    public void CanComposeMultipleFunctionsAsExtensionMethod()
    {
      Func<int, int> add1 = x => x + 1;
      Func<int, int> times2 = x => x * 2;
      Func<int, int> square = x => x * x;
      Func<int, int> subtract10 = x => x - 10;

      Func<int, int> expectedComposed = x => (((x - 10)*(x - 10))*2) + 1;

      var composed = new[] {add1, times2, square, subtract10}.Compose();

      Assert.That(composed(12), Is.EqualTo(expectedComposed(12)));
      Assert.That(composed(12), Is.EqualTo(9));
    }

    [Test]
    public void CanComposeMultipleFunctionsRightAfterLeftAsExtensionMethod()
    {
      Func<int, int> add1 = x => x + 1;
      Func<int, int> times2 = x => x * 2;
      Func<int, int> square = x => x * x;
      Func<int, int> subtract10 = x => x - 10;

      Func<int, int> expectedComposed = x => (((x + 1)*2)*((x + 1)*2)) - 10;

      var composed =
        new[] {add1, times2, square, subtract10}.ComposeReverse();

      Assert.That(composed(12), Is.EqualTo(expectedComposed(12)));
      Assert.That(composed(12), Is.EqualTo(666));
    }
  }
}
