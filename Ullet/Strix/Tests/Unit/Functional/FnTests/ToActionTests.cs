namespace Ullet.Strix.Functional.Tests.Unit.FnTests
{
  using System;
  using NUnit.Framework;

  [TestFixture]
  public class ToActionTests
  {
    [Test]
    public void CanConvertNullaryFuncToAction()
    {
      bool called = false;
      Func<int> func = () =>
      {
        called = true;
        return 123;
      };

      Action action = func.ToAction();

      action();
      Assert.That(called, Is.True);
    }

    [Test]
    public void CanConvertUnaryFuncToAction()
    {
      int parameter = 0;
      Func<int, bool> func = x =>
      {
        parameter = x;
        return true;
      };

      Action<int> action = func.ToAction();

      action(123);
      Assert.That(parameter, Is.EqualTo(123));
    }

    [Test]
    public void CanConvertBinaryFuncToAction()
    {
      int[] parameters = null;
      Func<int, int, bool> func =
        (a, b) =>
        {
          parameters = new[] {a, b};
          return true;
        };

      Action<int, int> action = func.ToAction();

      action(1, 2);
      Assert.That(parameters, Is.EqualTo(new[] {1, 2}));
    }

    [Test]
    public void CanConvertTernaryFuncToAction()
    {
      int[] parameters = null;
      Func<int, int, int, bool> func =
        (a, b, c) =>
        {
          parameters = new[] {a, b, c};
          return true;
        };

      Action<int, int, int> action = func.ToAction();

      action(1, 2, 3);
      Assert.That(parameters, Is.EqualTo(new[] {1, 2, 3}));
    }

    [Test]
    public void CanConvertQuaternaryFuncToAction()
    {
      int[] parameters = null;
      Func<int, int, int, int, bool> func =
        (a, b, c, d) =>
        {
          parameters = new[] {a, b, c, d};
          return true;
        };

      Action<int, int, int, int> action = func.ToAction();

      action(1, 2, 3, 4);
      Assert.That(parameters, Is.EqualTo(new[] {1, 2, 3, 4}));
    }

    [Test]
    public void CanConvertQuinaryFuncToAction()
    {
      int[] parameters = null;
      Func<int, int, int, int, int, bool> func =
        (a, b, c, d, e) =>
        {
          parameters = new[] {a, b, c, d, e};
          return true;
        };

      Action<int, int, int, int, int> action = func.ToAction();

      action(1, 2, 3, 4, 5);
      Assert.That(parameters, Is.EqualTo(new[] {1, 2, 3, 4, 5}));
    }
  }
}
