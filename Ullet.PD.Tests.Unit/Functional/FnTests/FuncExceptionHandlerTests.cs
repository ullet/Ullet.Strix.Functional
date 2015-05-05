/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.FnTests
{
  [TestFixture]
  public class FuncExceptionHandlerTests
  {
    [Test]
    public void CanConstructFuncHandler()
    {
      var handledIt = false;

      Func<Func<object>, object> handler
        = Fn.Handler<ArgumentException, object>(ex =>
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
        Fn.Handler<ArgumentException, object>(
          ex =>
          {
            handledBy = "ArgumentException";
            return new object();
          });
      Func<Func<object>, object> outerHandler = Fn.Handler<InvalidOperationException, object>(
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
        Fn.Handler<Exception, object>(ex =>
        {
          orderCalled.Add("Inner");
          return new object();
        });
      Func<Func<object>, object> outerHandler =
        Fn.Handler<Exception, object>(ex =>
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
        Fn.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      Assert.Throws<ArgumentException>(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void FuncHandlerThrowsExceptionIfDelegateFunctionReturnsNothing()
    {
      Func<Func<object>, object> handler =
        Fn.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      Assert.Throws<ArgumentException>(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void FuncHandlerNotThrowExceptionIfDelegateFuncReturnsSomething()
    {
      Func<Func<object>, object> handler =
        Fn.Handler<ArgumentException, object>(ex => Fn.Just(new object()));
      Assert.DoesNotThrow(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void ExceptionThrownIfNotHandledByAnyFuncHandler()
    {
      var handled = false;
      Func<Func<object>, object> handler
        = Fn.Handler<ArgumentException, object>(ex =>
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
        Fn.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      Func<Func<object>, object> outerHandler = 
        Fn.Handler<InvalidOperationException, object>(
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
        Fn.Handler<ArgumentNullException, object>(
          ex => Fn.Nothing<object>());
      Func<Func<object>, object> middleHandler
        = Fn.Handler<ArgumentException, object>(ex => new object());
      Func<Func<object>, object> outerHandler =
        Fn.Handler<InvalidOperationException, object>(
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
        Fn.Handler<Exception, object>(
        ex =>
        {
          callCount++;
          return Fn.Nothing<object>();
        });
      Func<Func<object>, object> middleHandler =
        Fn.Handler<Exception, object>(
        ex =>
        {
          callCount++;
          return Fn.Nothing<object>();
        });
      Func<Func<object>, object> outerHandler =
        Fn.Handler<Exception, object>(
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
        Fn.Handler<Exception, object>(
        ex =>
        {
          orderCalled.Add("inner");
          return Fn.Nothing<object>();
        });
      Func<Func<object>, object> middleHandler =
        Fn.Handler<Exception, object>(
          ex =>
          {
            orderCalled.Add("middle");
            return Fn.Nothing<object>();
          });
      Func<Func<object>, object> outerHandler =
        Fn.Handler<Exception, object>(
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
        Fn.Handler<InvalidOperationException, object>(ex => new object());
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
        Fn.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
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
        Fn.Handler<InvalidOperationException, object>(ex => new object());
      var outerHandler =
        Fn.Handler<MissingMethodException, object>(ex => new object());
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
        Fn.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
      var outerHandler =
        Fn.Handler<ArgumentException, object>(ex => Fn.Nothing<object>());
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

    private static void ThrowArgumentException(
      string message, Exception innerException)
    {
      throw new ArgumentException(message, innerException);
    }
  }
}
