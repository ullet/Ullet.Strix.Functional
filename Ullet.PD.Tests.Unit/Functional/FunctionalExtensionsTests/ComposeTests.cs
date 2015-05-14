/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Linq;
using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.FunctionalExtensionsTests
{
  [TestFixture]
  public class ComposeTests
  {
    [Test]
    public void CompositionOfLeftAfterRight()
    {
      Func<int, int> square = x => x*x;
      Func<int[], int> sum = a => a.Sum();

      var squareOfSum = square.After(sum);

      Assert.That(squareOfSum(new []{1,2,3,4}), Is.EqualTo(100));
    }

    [Test]
    public void CompositionOfMultipleLeftAfterRight()
    {
      Func<int, int> add1 = x => x + 1;
      Func<int, int> times2 = x => x*2;
      Func<int, int> square = x => x*x;
      Func<int, int> subtract10 = x => x - 10;

      Func<int, int> expectedComposite = x => (((x - 10) * (x - 10)) * 2) + 1;

      var composite = (new[] {add1, times2, square, subtract10}).Compose();

      Assert.That(composite(12), Is.EqualTo(expectedComposite(12)));
      Assert.That(composite(12), Is.EqualTo(9));
    }

    [Test]
    public void CompositionOfMultipleRightAfterLeft()
    {
      Func<int, int> add1 = x => x + 1;
      Func<int, int> times2 = x => x * 2;
      Func<int, int> square = x => x * x;
      Func<int, int> subtract10 = x => x - 10;

      Func<int, int> expectedComposite = x => (((x + 1)*2)*((x + 1)*2)) - 10;

      var composite = 
        (new[] { add1, times2, square, subtract10 }).ComposeReverse();

      Assert.That(composite(12), Is.EqualTo(expectedComposite(12)));
      Assert.That(composite(12), Is.EqualTo(666));
    }
  }
}
