/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.FnTests
{
  [TestFixture]
  public class CurryTests
  {
    [Test]
    public void CanCurryTwoParameterFunction()
    {
      Func<char, int, string> uncurried = (c, count) => new string(c, count);

      Func<char, Func<int, string>> curried = Fn.Curry(uncurried);

      Assert.That(curried('X')(4), Is.EqualTo(uncurried('X', 4)));
    }

    [Test]
    public void CanCurryThreeParameterFunction()
    {
      Func<int, long, float, double> uncurried =
        (i, l, f) => ((double) (i + l))/f;

      Func<int, Func<long, Func<float, double>>> curried = Fn.Curry(uncurried);

      Assert.That(curried(10)(4L)(2.0f), Is.EqualTo(uncurried(10, 4L, 2.0f)));
    }

    [Test]
    public void CanCurryFourParameterFunction()
    {
      Func<string, string[], int, int, string> uncurried = string.Join;

      Func<string, Func<string[], Func<int, Func<int, string>>>> curried =
        Fn.Curry(uncurried);

      Assert.That(
        curried("-")(new[] {"a", "b", "c", "d"})(1)(2),
        Is.EqualTo(uncurried("-", new[] { "a", "b", "c", "d" }, 1, 2)));
    }

    [Test]
    public void CanCurryFiveParameterFunction()
    {
      Func<string, int, string, int, int, int> uncurried = string.Compare;

      Func<string, Func<int, Func<string, Func<int, Func<int, int>>>>>
        curried = Fn.Curry(uncurried);

      Assert.That(
        curried("pontificate")(0)("cattle")(0)(3),
        Is.EqualTo(uncurried("pontificate", 0, "cattle", 0, 3)));
    }

    [Test]
    public void CanCurryTwoParameterFunctionAsExtensionMethod()
    {
      Func<char, int, string> uncurried = (c, count) => new string(c, count);

      Func<char, Func<int, string>> curried = uncurried.Curry();

      Assert.That(curried('X')(4), Is.EqualTo(uncurried('X', 4)));
    }

    [Test]
    public void CanCurryThreeParameterFunctionAsExtensionMethod()
    {
      Func<int, long, float, double> uncurried =
        (i, l, f) => ((double) (i + l))/f;

      Func<int, Func<long, Func<float, double>>> curried = uncurried.Curry();

      Assert.That(curried(10)(4L)(2.0f), Is.EqualTo(uncurried(10, 4L, 2.0f)));
    }

    [Test]
    public void CanCurryFourParameterFunctionAsExtensionMethod()
    {
      Func<string, string[], int, int, string> uncurried = string.Join;

      Func<string, Func<string[], Func<int, Func<int, string>>>> curried =
        uncurried.Curry();

      Assert.That(
        curried("-")(new[] {"a", "b", "c", "d"})(1)(2),
        Is.EqualTo(uncurried("-", new[] { "a", "b", "c", "d" }, 1, 2)));
    }

    [Test]
    public void CanCurryFiveParameterFunctionAsExtensionMethod()
    {
      Func<string, int, string, int, int, int> uncurried = string.Compare;

      Func<string, Func<int, Func<string, Func<int, Func<int, int>>>>>
        curried = uncurried.Curry();

      Assert.That(
        curried("pontificate")(0)("cattle")(0)(3),
        Is.EqualTo(uncurried("pontificate", 0, "cattle", 0, 3)));
    }
  }
}
