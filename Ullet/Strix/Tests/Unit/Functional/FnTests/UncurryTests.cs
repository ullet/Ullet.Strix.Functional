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
  public class UncurryTests
  {
    [Test]
    public void UncurriedFormOfUnaryFunctionIsItself()
    {
      Func<int, int> curried = a => a + 1;

      Func<int, int> uncurried = curried.Uncurry();

      var inputs = RandomValues(100);
      var resultsViaCurried = inputs.Select(curried).ToArray();
      var resultsViaUncurried = inputs.Select(uncurried).ToArray();
      Assert.That(resultsViaUncurried, Is.EqualTo(resultsViaCurried));
    }

    [Test]
    public void CanUncurryToTwoParameterFunction()
    {
      Func<char, Func<int, string>> curried =
        c => count => new string(c, count);

      Func<char, int, string> uncurried = curried.Uncurry();

      Assert.That(uncurried('X', 4), Is.EqualTo(curried('X')(4)));
    }

    [Test]
    public void CanUncurryToThreeParameterFunction()
    {
      Func<int, Func<long, Func<float, double>>> curried =
        i => l => f => ((double) (i + l))/f;

      Func<int, long, float, double> uncurried = curried.Uncurry();

      Assert.That(uncurried(10, 4L, 2.0f), Is.EqualTo(curried(10)(4L)(2.0f)));
    }

    [Test]
    public void CanUncurryToFourParameterFunction()
    {
      Func<string, Func<string[], Func<int, Func<int, string>>>> curried =
        s => v => i => c => string.Join(s, v, i, c);

      Func<string, string[], int, int, string> uncurried = curried.Uncurry();

      Assert.That(uncurried("-", new[] { "a", "b", "c", "d" }, 1, 2),
        Is.EqualTo(curried("-")(new[] { "a", "b", "c", "d" })(1)(2)));
    }

    [Test]
    public void CanUncurryToFiveParameterFunction()
    {
      Func<string, Func<int, Func<string, Func<int, Func<int, int>>>>> curried =
        s1 => i1 => s2 => i2 => l => string.Compare(s1, i1, s2, i2, l);

      Func<string, int, string, int, int, int> uncurried = curried.Uncurry();

      Assert.That(uncurried("pontificate", 0, "cattle", 0, 3),
        Is.EqualTo(curried("pontificate")(0)("cattle")(0)(3)));
    }

    [Test]
    public void UncurriedFormOfUnaryActionIsItself()
    {
      int result = 0;
      Action<int> curried = a => result = a + 1;

      Action<int> uncurried = curried.Uncurry();

      var input = new Random().Next();
      curried(input);
      var resultViaCurried = result;
      uncurried(input);
      var resultViaUncurried = result;
      Assert.That(resultViaUncurried, Is.EqualTo(resultViaCurried));
    }

    [Test]
    public void CanUncurryToTwoParameterAction()
    {
      string result = null;
      Func<char, Action<int>> curried =
        c => count => result = new string(c, count);

      Action<char, int> uncurried = curried.Uncurry();

      uncurried('X', 4);
      Assert.That(result, Is.EqualTo("XXXX"));
    }

    [Test]
    public void CanUncurryToThreeParameterAction()
    {
      double result = 0;
      Func<int, Func<long, Action<float>>> curried =
        i => l => f => result = ((double)(i + l)) / f;

      Action<int, long, float> uncurried = curried.Uncurry();

      uncurried(10, 4L, 2.0f);
      Assert.That(result, Is.EqualTo(7.0D));
    }

    [Test]
    public void CanUncurryToFourParameterAction()
    {
      string result = null;
      Func<string, Func<string[], Func<int, Action<int>>>> curried =
        s => v => i => c => result = string.Join(s, v, i, c);

      Action<string, string[], int, int> uncurried = curried.Uncurry();

      uncurried("-", new[] {"a", "b", "c", "d"}, 1, 2);
      Assert.That(result, Is.EqualTo("b-c"));
    }

    [Test]
    public void CanUncurryToFiveParameterAction()
    {
      int result = 0;
      Func<string, Func<int, Func<string, Func<int, Action<int>>>>> curried =
        s1 => i1 => s2 => i2 => l => result = string.Compare(s1, i1, s2, i2, l);

      Action<string, int, string, int, int> uncurried = curried.Uncurry();

      uncurried("pontificate", 0, "cattle", 0, 3);
      Assert.That(result, Is.EqualTo(1));
    }

    private static int[] RandomValues(int count)
    {
      var random = new Random();
      return Enumerable.Range(1, count).Select(x => random.Next()).ToArray();
    }
  }
}
