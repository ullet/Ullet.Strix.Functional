using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Ullet.Strix.Functional.Tests.Unit.FnTests
{
  [TestFixture]
  public class NestTests
  {
    [Test]
    public void NestActionOfActionInsideActionOfAction()
    {
      var trace = new List<string>();
      Action<Action> innerAction = action =>
      {
        trace.Add("start-inner");
        action();
        trace.Add("end-inner");
      };
      Action<Action> outerAction = action =>
      {
        trace.Add("start-outer");
        action();
        trace.Add("end-outer");
      };

      Action<Action> nested = outerAction.Nest(innerAction);

      nested(() => trace.Add("action"));
      Assert.That(
        trace,
        Is.EqualTo(new List<string>
        {
          "start-outer",
          "start-inner",
          "action",
          "end-inner",
          "end-outer"
        }));
    }

    [Test]
    public void NestActionInsideActionOfAction()
    {
      var trace = new List<string>();
      Action innerAction = () =>
      {
        trace.Add("inner");
      };
      Action<Action> outerAction = action =>
      {
        trace.Add("start-outer");
        action();
        trace.Add("end-outer");
      };

      Action nested = outerAction.Nest(innerAction);

      nested();
      Assert.That(
        trace,
        Is.EqualTo(new List<string>
        {
          "start-outer",
          "inner",
          "end-outer"
        }));
    }

    [Test]
    public void NestFuncOfFuncInsideFuncOfFunc()
    {
      var trace = new List<string>();
      Func<Func<int>, double> innerFunc = func =>
      {
        trace.Add("start-inner");
        var value = func();
        trace.Add("end-inner");
        return value;
      };
      Func<Func<double>, long> outerFunc = func =>
      {
        trace.Add("start-outer");
        var value = func();
        trace.Add("end-outer");
        return (long)Math.Floor(value);
      };

      Func<Func<int>, long> nested = outerFunc.Nest(innerFunc);

      var returnValue = nested(() =>
      {
        trace.Add("func");
        return 42;
      });
      Assert.That(
        trace,
        Is.EqualTo(new List<string>
        {
          "start-outer",
          "start-inner",
          "func",
          "end-inner",
          "end-outer"
        }));
      Assert.That(returnValue, Is.EqualTo(42L));
    }

    [Test]
    public void NestFuncInsideFuncOfFunc()
    {
      var trace = new List<string>();
      Func<int> innerFunc = () =>
      {
        trace.Add("inner");
        return 42;
      };
      Func<Func<int>, double> outerFunc = func =>
      {
        trace.Add("start-outer");
        var value = func();
        trace.Add("end-outer");
        return value;
      };

      Func<double> nested = outerFunc.Nest(innerFunc);

      var returnValue = nested();
      Assert.That(
        trace,
        Is.EqualTo(new List<string>
        {
          "start-outer",
          "inner",
          "end-outer"
        }));
      Assert.That(returnValue, Is.EqualTo(42.0));
    }
  }
}
