namespace Ullet.Strix.Functional.Tests.Unit.FnTests
{
  using System;
  using NUnit.Framework;
  using Unit = Functional.Unit;

  [TestFixture]
  public class ToFuncTests
  {
    [Test]
    public void CanConvertNullaryActionToFunc()
    {
      bool called = false;
      Action action = () =>
      {
        called = true;
      };

      Func<Unit> func = action.ToFunc();

      var returned = func();
      Assert.That(called, Is.True);
      Assert.That(returned, Is.EqualTo(Fn.Unit));
    }

    [Test]
    public void CanConvertUnaryActionToFunc()
    {
      int parameter = 0;
      Action<int> action = x =>
      {
        parameter = x;
      };

      Func<int, Unit> func = action.ToFunc();

      var returned = func(123);
      Assert.That(parameter, Is.EqualTo(123));
      Assert.That(returned, Is.EqualTo(Fn.Unit));
    }

    [Test]
    public void CanConvertBinaryActionToFunc()
    {
      int[] parameters = null;
      Action<int, int> action =
        (a, b) =>
        {
          parameters = new[] { a, b };
        };

      Func<int, int, Unit> func = action.ToFunc();

      var returned = func(1, 2);
      Assert.That(parameters, Is.EqualTo(new[] { 1, 2 }));
      Assert.That(returned, Is.EqualTo(Fn.Unit));
    }

    [Test]
    public void CanConvertTernaryActionToFunc()
    {
      int[] parameters = null;
      Action<int, int, int> action =
        (a, b, c) =>
        {
          parameters = new[] { a, b, c };
        };

      Func<int, int, int, Unit> func = action.ToFunc();

      var returned = func(1, 2, 3);
      Assert.That(parameters, Is.EqualTo(new[] { 1, 2, 3 }));
      Assert.That(returned, Is.EqualTo(Fn.Unit));
    }

    [Test]
    public void CanConvertQuaternaryActionToFunc()
    {
      int[] parameters = null;
      Action<int, int, int, int> action =
        (a, b, c, d) =>
        {
          parameters = new[] { a, b, c, d };
        };

      Func<int, int, int, int, Unit> func = action.ToFunc();

      var returned = func(1, 2, 3, 4);
      Assert.That(parameters, Is.EqualTo(new[] { 1, 2, 3, 4 }));
      Assert.That(returned, Is.EqualTo(Fn.Unit));
    }

    [Test]
    public void CanConvertQuinaryActionToFunc()
    {
      int[] parameters = null;
      Action<int, int, int, int, int> action =
        (a, b, c, d, e) =>
        {
          parameters = new[] { a, b, c, d, e };
        };

      Func<int, int, int, int, int, Unit> func = action.ToFunc();

      var returned = func(1, 2, 3, 4, 5);
      Assert.That(parameters, Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));
      Assert.That(returned, Is.EqualTo(Fn.Unit));
    }
  }
}
