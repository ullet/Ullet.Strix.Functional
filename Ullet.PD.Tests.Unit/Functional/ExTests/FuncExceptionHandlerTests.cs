/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.ExTests
{
  [TestFixture]
  public class FuncExceptionHandlerTests
  {
    [Test]
    public void CanConstructFuncHandler()
    {
      var handledIt = false;

      Func<Func<object>, object> handler
        = Ex.Handler<ArgumentException, object>(ex =>
        {
          handledIt = true;
          return new object();
        });
      handler(() => { throw new ArgumentException(); });

      Assert.That(handledIt, Is.True);
    }

    [Test]
    public void CanConstructNestedFuncHandler()
    {
      string handledBy = null;

      Func<Func<object>, object> innerHandler =
        Ex.Handler<ArgumentException, object>(
          ex =>
          {
            handledBy = "ArgumentException";
            return new object();
          });
      Func<Func<object>, object> outerHandler = Ex.Handler<InvalidOperationException, object>(
        ex =>
        {
          handledBy = "InvalidOperationException";
          return new object();
        });
      Func<Func<object>, object> nestedHandler =
        Fn.Nest(innerHandler, outerHandler);
      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(handledBy, Is.EqualTo("InvalidOperationException"));
    }

    [Test]
    public void OuterFuncHandlerNotCalledIfAlreadyHandled()
    {
      var orderCalled = new List<string>();
      Func<Func<object>, object> innerHandler =
        Ex.Handler<Exception, object>(ex =>
        {
          orderCalled.Add("Inner");
          return new object();
        });
      Func<Func<object>, object> outerHandler =
        Ex.Handler<Exception, object>(ex =>
        {
          orderCalled.Add("Outer");
          return new object();
        });
      Func<Func<object>, object> nestedHandler =
        Fn.Nest(outerHandler, innerHandler);

      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(orderCalled, Is.EqualTo(new List<string> {"Inner"}));
    }

    [Test]
    public void CanConstructFuncHandlerFromConditionalHandlingDelegate()
    {
      Func<Func<object>, object> handler =
        Ex.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      Assert.Throws<ArgumentException>(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void FuncHandlerThrowsExceptionIfDelegateFunctionReturnsNothing()
    {
      Func<Func<object>, object> handler =
        Ex.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      Assert.Throws<ArgumentException>(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void FuncHandlerNotThrowExceptionIfDelegateFuncReturnsSomething()
    {
      Func<Func<object>, object> handler =
        Ex.Handler<ArgumentException, object>(ex => Fn.Just(new object()));
      Assert.DoesNotThrow(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void ExceptionThrownIfNotHandledByAnyFuncHandler()
    {
      var handled = false;
      Func<Func<object>, object> handler
        = Ex.Handler<ArgumentException, object>(ex =>
        {
          handled = true;
          return new object();
        });
      Assert.Throws<InvalidOperationException>(
        () => handler(() => { throw new InvalidOperationException(); }));
      Assert.That(handled, Is.False);
    }

    [Test]
    public void CanConstructNestedFuncHandlerFromConditionalHandlingDelegates()
    {
      var handled = false;

      Func<Func<object>, object> innerHandler =
        Ex.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      Func<Func<object>, object> outerHandler = 
        Ex.Handler<InvalidOperationException, object>(
        ex =>
        {
          handled = true;
          return Fn.Just(new object());
        });
      Func<Func<object>, object> nestedHandler =
        Fn.Nest(innerHandler, outerHandler);
      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(handled, Is.True);
    }

    [Test]
    public void CanConstructNestedActionHandlerFromMixedDelegates()
    {
      var handled = false;

      Func<Func<object>, object> innerHandler =
        Ex.Handler<ArgumentNullException, object>(
          ex => Fn.Nothing<object>());
      Func<Func<object>, object> middleHandler
        = Ex.Handler<ArgumentException, object>(ex => new object());
      Func<Func<object>, object> outerHandler =
        Ex.Handler<InvalidOperationException, object>(
          ex =>
          {
            handled = true;
            return Fn.Just(new object());
          });
      Func<Func<object>, object> nestedHandler =
        Fn.Nest(outerHandler, Fn.Nest(middleHandler, innerHandler));
      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(handled, Is.True);
    }

    [Test]
    public void AllMatchingFuncHandlersCalledUntilHandled()
    {
      var callCount = 0;
      Func<Func<object>, object> innerHandler =
        Ex.Handler<Exception, object>(
        ex =>
        {
          callCount++;
          return Fn.Nothing<object>();
        });
      Func<Func<object>, object> middleHandler =
        Ex.Handler<Exception, object>(
        ex =>
        {
          callCount++;
          return Fn.Nothing<object>();
        });
      Func<Func<object>, object> outerHandler =
        Ex.Handler<Exception, object>(
        ex =>
        {
          callCount++;
          return Fn.Just(new object());
        });
      var nestedHandler =
        Fn.Nest(outerHandler, Fn.Nest(middleHandler, innerHandler));

      nestedHandler(() => { throw new Exception(); });

      Assert.That(callCount, Is.EqualTo(3));
    }

    [Test]
    public void FuncHandlersCalledInOrderFromInnerToOuter()
    {
      var orderCalled = new List<string>();
      Func<Func<object>, object> innerHandler =
        Ex.Handler<Exception, object>(
        ex =>
        {
          orderCalled.Add("inner");
          return Fn.Nothing<object>();
        });
      Func<Func<object>, object> middleHandler =
        Ex.Handler<Exception, object>(
          ex =>
          {
            orderCalled.Add("middle");
            return Fn.Nothing<object>();
          });
      Func<Func<object>, object> outerHandler =
        Ex.Handler<Exception, object>(
        ex =>
        {
          orderCalled.Add("outer");
          return Fn.Just(new object());
        });
      var nestedHandler =
        Fn.Nest(outerHandler, Fn.Nest(middleHandler, innerHandler));

      nestedHandler(() => { throw new Exception(); });

      Assert.That(
        orderCalled, Is.EqualTo(new List<string> {"inner", "middle", "outer"}));
    }

    [Test]
    public void ExceptionUnhandledByFuncHandlerRetainsStackTrace()
    {
      var handler =
        Ex.Handler<InvalidOperationException, object>(ex => new object());
      var argEx = Assert.Throws<ArgumentException>(() =>
        handler(() =>
          {
            // Call method so that get an easily matchable name in stack trace
            ThrowArgumentException("Error", new InvalidOperationException());
            return null;
          }));

      Assert.That(
        argEx.StackTrace, Is.StringContaining("ThrowArgumentException"));
    }

    [Test]
    public void ExceptionReThrownByFuncHandlerRetainsStackTrace()
    {
      var handler =
        Ex.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      var argEx = Assert.Throws<ArgumentException>(() =>
        handler(() =>
        {
          // Call method so that get an easily matchable name in stack trace
          ThrowArgumentException("Error", new InvalidOperationException());
          return null;
        }));

      Assert.That(
        argEx.StackTrace, Is.StringContaining("ThrowArgumentException"));
    }

    [Test]
    public void ExceptionUnhandledByNestedActionHandlerRetainsStackTrace()
    {
      var innerHandler =
        Ex.Handler<InvalidOperationException, object>(ex => new object());
      var outerHandler =
        Ex.Handler<MissingMethodException, object>(ex => new object());
      var handler = Fn.Nest(innerHandler, outerHandler);
      var argEx = Assert.Throws<ArgumentException>(() =>
        handler(() =>
        {
          // Call method so that get an easily matchable name in stack trace
          ThrowArgumentException("Error", new InvalidOperationException());
          return null;
        }));

      Assert.That(
        argEx.StackTrace, Is.StringContaining("ThrowArgumentException"));
    }

    [Test]
    public void ExceptionReThrownByNestedActionHandlerRetainsStackTrace()
    {
      var innerHandler =
        Ex.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      var outerHandler =
        Ex.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      var handler = Fn.Nest(innerHandler, outerHandler);
      var argEx = Assert.Throws<ArgumentException>(() =>
        handler(() =>
        {
          // Call method so that get an easily matchable name in stack trace
          ThrowArgumentException("Error", new InvalidOperationException());
          return null;
        }));

      Assert.That(
        argEx.StackTrace, Is.StringContaining("ThrowArgumentException"));
    }

    [Test]
    public void ReturnValuePassedThroughWhenNoExceptionForCapturingDelegate()
    {
      var handler = Ex.Handler<Exception, int>(ex => -1);

      var returned = handler(() => 42);

      Assert.That(returned, Is.EqualTo(42));
    }

    [Test]
    public void ReturnValuePassedThroughWhenNoExceptionForConditionalDelegate()
    {
      var handler = Ex.Handler<Exception, int>(ex => Fn.Nothing<int>());
      
      var returned = handler(() => 42);

      Assert.That(returned, Is.EqualTo(42));
    }

    [Test]
    public void ValueReturnedFromHandlerWhenHandledForCapturingDelegate()
    {
      var handler = Ex.Handler<Exception, int>(ex => 42);

      var returned = handler(() => { throw new Exception(); });

      Assert.That(returned, Is.EqualTo(42));
    }

    [Test]
    public void ValueReturnedFromHandlerWhenHandledForConditionalDelegate()
    {
      var handler = Ex.Handler<Exception, int>(ex => Fn.Just(42));

      var returned = handler(() => {throw new Exception();});

      Assert.That(returned, Is.EqualTo(42));
    }

    [Test]
    public void ValueReturnedFromFirstHandlerThatCanHandleExceptionCapturing()
    {
      var handler = Fn.Nest(
        Ex.Handler<Exception, int>(ex => 4),
        Fn.Nest(
          Ex.Handler<ArgumentException, int>(ex => 3),
          Fn.Nest(
            Ex.Handler<ArgumentException, int>(ex => 2),
            Ex.Handler<InvalidOperationException, int>(ex => 1))));

      var returned = handler(() => { throw new ArgumentException(); });

      Assert.That(returned, Is.EqualTo(2));
    }

    [Test]
    public void ValueReturnedFromFirstHandlerThatCanHandleExceptionConditional()
    {
      var handler = Fn.Nest(
        Ex.Handler<Exception, int>(ex => Fn.Just(4)),
        Fn.Nest(
          Ex.Handler<ArgumentException, int>(ex => Fn.Just(3)),
          Fn.Nest(
            Ex.Handler<ArgumentException, int>(ex => Fn.Just(2)),
            Fn.Nest(
              Ex.Handler<ArgumentException, int>(ex => Fn.Nothing<int>()),
              Ex.Handler<InvalidOperationException, int>(ex => Fn.Just(1))))));

      var returned = handler(() => { throw new ArgumentException(); });

      Assert.That(returned, Is.EqualTo(2));
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfNoExceptionForCapturingDelegate()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception, int>(
        ex => -1, () => finallyWasCalled = true);

      handler(() => 42);

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void ReturnValuePassedThroughWhenNoExceptionAndFinallyForCapturing()
    {
      var handler = Ex.Handler<Exception, int>(ex => -1, () => { });

      var returned = handler(() => 42);

      Assert.That(returned, Is.EqualTo(42));
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfNoExceptionForConditionalDelegate()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception, int>(
        ex => Fn.Nothing<int>(), () => finallyWasCalled = true);

      handler(() => 42);

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void ReturnValuePassedThruWhenNoExceptionAndFinallyForConditional()
    {
      var handler = Ex.Handler<Exception, int>(
        ex => Fn.Nothing<int>(), () => { });

      var returned = handler(() => 42);

      Assert.That(returned, Is.EqualTo(42));
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfHandledForCapturingDelegate()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception, int>(
        ex => -1, () => finallyWasCalled = true);

      handler(() => { throw new Exception(); });

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void ReturnValueFromHandlerWithFinallyBlockForCapturingDelegate()
    {
      var handler = Ex.Handler<Exception, int>(ex => -1, () => { });

      var returned = handler(() => { throw new Exception(); });

      Assert.That(returned, Is.EqualTo(-1));
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfHandledForConditionalDelegate()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception, int>(
        ex => Fn.Just(-1), () => finallyWasCalled = true);

      handler(() => { throw new Exception(); });

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void ReturnValueFromHandlerWithFinallyBlockForConditionalDelegate()
    {
      var handler = Ex.Handler<Exception, int>(ex => Fn.Just(-1), () => { });

      var returned = handler(() => { throw new Exception(); });

      Assert.That(returned, Is.EqualTo(-1));
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfUnhandledForCapturingDelegate()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<ArgumentException, int>(
        ex => -1, () => finallyWasCalled = true);

      Assert.Throws<Exception>(() => handler(() => { throw new Exception(); }));

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfUnhandledForConditionalDelegate()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<ArgumentException, int>(
        ex => Fn.Just(-1), () => finallyWasCalled = true);

      Assert.Throws<Exception>(() => handler(() => { throw new Exception(); }));

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfExceptionBubbledUp()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception, int>(
        ex => Fn.Nothing<int>(), () => finallyWasCalled = true);

      Assert.Throws<Exception>(() => handler(() => { throw new Exception(); }));

      Assert.That(finallyWasCalled, Is.True);
    }

    private static void ThrowArgumentException(
      string message, Exception innerException)
    {
      throw new ArgumentException(message, innerException);
    }
  }
}
