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
  public class CurryTests
  {
    [Test]
    public void CurriedFormOfUnaryFunctionIsItself()
    {
      Func<int, int> uncurried = a => a + 1;

      // ReSharper disable once InvokeAsExtensionMethod
      Func<int, int> curried = Fn.Curry(uncurried);

      var inputs = RandomValues(100);
      var resultsViaUncurried = inputs.Select(uncurried).ToArray();
      var resultsViaCurried = inputs.Select(curried).ToArray();
      Assert.That(resultsViaCurried, Is.EqualTo(resultsViaUncurried));
    }

    [Test]
    public void CanCurryTwoParameterFunction()
    {
      Func<char, int, string> uncurried = (c, count) => new string(c, count);

      // ReSharper disable once InvokeAsExtensionMethod
      Func<char, Func<int, string>> curried = Fn.Curry(uncurried);

      Assert.That(curried('X')(4), Is.EqualTo(uncurried('X', 4)));
    }

    [Test]
    public void CanCurryThreeParameterFunction()
    {
      Func<int, long, float, double> uncurried =
        (i, l, f) => ((double) (i + l))/f;

      // ReSharper disable once InvokeAsExtensionMethod
      Func<int, Func<long, Func<float, double>>> curried = Fn.Curry(uncurried);

      Assert.That(curried(10)(4L)(2.0f), Is.EqualTo(uncurried(10, 4L, 2.0f)));
    }

    [Test]
    public void CanCurryFourParameterFunction()
    {
      Func<string, string[], int, int, string> uncurried = string.Join;

      // ReSharper disable once InvokeAsExtensionMethod
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

      // ReSharper disable once InvokeAsExtensionMethod
      Func<string, Func<int, Func<string, Func<int, Func<int, int>>>>>
        curried = Fn.Curry(uncurried);

      Assert.That(
        curried("pontificate")(0)("cattle")(0)(3),
        Is.EqualTo(uncurried("pontificate", 0, "cattle", 0, 3)));
    }

    [Test]
    public void CurriedFormOfUnaryActionIsItself()
    {
      int result = 0;
      Action<int> uncurried = a => result = a + 1;

      // ReSharper disable once InvokeAsExtensionMethod
      Action<int> curried = Fn.Curry(uncurried);

      var input = new Random().Next();
      uncurried(input);
      var resultViaUncurried = result;
      curried(input);
      var resultViaCurried = result;
      Assert.That(resultViaCurried, Is.EqualTo(resultViaUncurried));
    }

    [Test]
    public void CanCurryTwoParameterAction()
    {
      string result = null;
      Action<char, int> uncurried = (c, count) => result =new string(c, count);

      // ReSharper disable once InvokeAsExtensionMethod
      Func<char, Action<int>> curried = Fn.Curry(uncurried);

      curried('X')(4);
      Assert.That(result, Is.EqualTo("XXXX"));
    }

    [Test]
    public void CanCurryThreeParameterAction()
    {
      double result = 0;
      Action<int, long, float> uncurried =
        (i, l, f) => result = ((double)(i + l)) / f;

      // ReSharper disable once InvokeAsExtensionMethod
      Func<int, Func<long, Action<float>>> curried = Fn.Curry(uncurried);

      curried(10)(4L)(2.0f);
      Assert.That(result, Is.EqualTo(7.0D));
    }

    [Test]
    public void CanCurryFourParameterAction()
    {
      string result = null;
      Action<string, string[], int, int> uncurried =
        (separator, values, startIndex, count) =>
          result = string.Join(separator, values, startIndex, count);

      // ReSharper disable once InvokeAsExtensionMethod
      Func<string, Func<string[], Func<int, Action<int>>>> curried =
        Fn.Curry(uncurried);

      curried("-")(new[] {"a", "b", "c", "d"})(1)(2);
      Assert.That(result, Is.EqualTo("b-c"));
    }

    [Test]
    public void CanCurryFiveParameterAction()
    {
      int result = 0;
      Action<string, int, string, int, int> uncurried =
        (strA, indexA, strB, indexB, length) =>
          result = string.Compare(strA, indexA, strB, indexB, length);

      // ReSharper disable once InvokeAsExtensionMethod
      Func<string, Func<int, Func<string, Func<int, Action<int>>>>>
        curried = Fn.Curry(uncurried);

      curried("pontificate")(0)("cattle")(0)(3);
      Assert.That(result, Is.EqualTo(1));
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

    [Test]
    public void CanCurryTwoParameterActionAsExtensionMethod()
    {
      string result = null;
      Action<char, int> uncurried = (c, count) => result = new string(c, count);

      Func<char, Action<int>> curried = uncurried.Curry();

      curried('X')(4);
      Assert.That(result, Is.EqualTo("XXXX"));
    }

    [Test]
    public void CanCurryThreeParameterActionAsExtensionMethod()
    {
      double result = 0;
      Action<int, long, float> uncurried =
        (i, l, f) => result = ((double)(i + l)) / f;

      Func<int, Func<long, Action<float>>> curried = uncurried.Curry();

      curried(10)(4L)(2.0f);
      Assert.That(result, Is.EqualTo(7.0D));
    }

    [Test]
    public void CanCurryFourParameterActionAsExtensionMethod()
    {
      string result = null;
      Action<string, string[], int, int> uncurried =
        (separator, values, startIndex, count) =>
          result = string.Join(separator, values, startIndex, count);

      Func<string, Func<string[], Func<int, Action<int>>>> curried =
        uncurried.Curry();

      curried("-")(new[] { "a", "b", "c", "d" })(1)(2);
      Assert.That(result, Is.EqualTo("b-c"));
    }

    [Test]
    public void CanCurryFiveParameterActionAsExtensionMethod()
    {
      int result = 0;
      Action<string, int, string, int, int> uncurried =
        (strA, indexA, strB, indexB, length) =>
          result = string.Compare(strA, indexA, strB, indexB, length);

      Func<string, Func<int, Func<string, Func<int, Action<int>>>>>
        curried = uncurried.Curry();

      curried("pontificate")(0)("cattle")(0)(3);
      Assert.That(result, Is.EqualTo(1));
    }

    private static int[] RandomValues(int count)
    {
      var random = new Random();
      return Enumerable.Range(1, count).Select(x => random.Next()).ToArray();
    }
  }
}
