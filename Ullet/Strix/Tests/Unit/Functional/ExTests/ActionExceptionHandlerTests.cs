﻿/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Ullet.Strix.Functional.Tests.Unit.ExTests
{
  [TestFixture]
  public class ActionExceptionHandlerTests
  {
    [Test]
    public void CanConstructActionHandler()
    {
      var handledIt = false;

      Action<Action> handler
        = Ex.Handler<ArgumentException>(ex => handledIt = true);
      handler(() => { throw new ArgumentException(); });

      Assert.That(handledIt, Is.True);
    }

    [Test]
    public void CanConstructNestedActionHandler()
    {
      string handledBy = null;

      Action<Action> innerHandler = Ex.Handler<ArgumentException>(
        ex => handledBy = "ArgumentException");
      Action<Action> outerHandler = Ex.Handler<InvalidOperationException>(
        ex => handledBy = "InvalidOperationException");
      Action<Action> nestedHandler = innerHandler.Nest(outerHandler);
      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(handledBy, Is.EqualTo("InvalidOperationException"));
    }

    [Test]
    public void OuterActionHandlerNotCalledIfAlreadyHandled()
    {
      var orderCalled = new List<string>();
      Action<Action> innerHandler
        = Ex.Handler<Exception>(ex => orderCalled.Add("Inner"));
      Action<Action> outerHandler
        = Ex.Handler<Exception>(ex => orderCalled.Add("Outer"));
      Action<Action> nestedHandler = outerHandler.Nest(innerHandler);

      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(orderCalled, Is.EqualTo(new List<string> {"Inner"}));
    }

    [Test]
    public void CanConstructActionHandlerFromDelegateFunction()
    {
      Action<Action> handler = Ex.Handler<ArgumentException>(ex => false);
      Assert.Throws<ArgumentException>(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void ActionHandlerThrowsExceptionIfDelegateFunctionReturnsFalse()
    {
      Action<Action> handler = Ex.Handler<ArgumentException>(ex => false);
      Assert.Throws<ArgumentException>(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void ActionHandlerNotThrowExceptionIfDelegateFunctionReturnsTrue()
    {
      Action<Action> handler = Ex.Handler<ArgumentException>(ex => true);
      Assert.DoesNotThrow(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void ExceptionThrownIfNotHandledByAnyActionHandler()
    {
      var handled = false;
      Action<Action> handler
        = Ex.Handler<ArgumentException>(ex => handled = true);
      Assert.Throws<InvalidOperationException>(
        () => handler(() => { throw new InvalidOperationException(); }));
      Assert.That(handled, Is.False);
    }

    [Test]
    public void CanConstructNestedActionHandlerFromFunctionDelegates()
    {
      var handled = false;

      Action<Action> innerHandler
        = Ex.Handler<ArgumentException>(ex => false);
      Action<Action> outerHandler = Ex.Handler<InvalidOperationException>(
        ex =>
        {
          handled = true;
          return true;
        });
      Action<Action> nestedHandler = innerHandler.Nest(outerHandler);
      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(handled, Is.True);
    }

    [Test]
    public void CanConstructNestedActionHandlerFromFunctionAndActionDelegates()
    {
      var handled = false;

      Action<Action> innerHandler
        = Ex.Handler<ArgumentNullException>(ex => false);
      Action<Action> middleHandler
        = Ex.Handler<ArgumentException>(ex => { });
      Action<Action> outerHandler = Ex.Handler<InvalidOperationException>(
        ex =>
        {
          handled = true;
          return true;
        });
      Action<Action> nestedHandler =
        outerHandler.Nest(middleHandler.Nest(innerHandler));
      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(handled, Is.True);
    }

    [Test]
    public void AllMatchingActionHandlersCalledUntilHandled()
    {
      var callCount = 0;
      Action<Action> innerHandler = Ex.Handler<Exception>(
        ex =>
        {
          callCount++;
          return false;
        });
      Action<Action> middleHandler = Ex.Handler<Exception>(
        ex =>
        {
          callCount++;
          return false;
        });
      Action<Action> outerHandler = Ex.Handler<Exception>(
        ex =>
        {
          callCount++;
          return true;
        });
      var nestedHandler = outerHandler.Nest(middleHandler.Nest(innerHandler));

      nestedHandler(() => { throw new Exception(); });

      Assert.That(callCount, Is.EqualTo(3));
    }

    [Test]
    public void ActionHandlersCalledInOrderFromInnerToOuter()
    {
      var orderCalled = new List<string>();
      Action<Action> innerHandler = Ex.Handler<Exception>(
        ex =>
        {
          orderCalled.Add("inner");
          return false;
        });
      Action<Action> middleHandler = Ex.Handler<Exception>(
          ex =>
          {
            orderCalled.Add("middle");
            return false;
          });
      Action<Action> outerHandler = Ex.Handler<Exception>(
        ex =>
        {
          orderCalled.Add("outer");
          return true;
        });
      var nestedHandler = outerHandler.Nest(middleHandler.Nest(innerHandler));

      nestedHandler(() => { throw new Exception(); });

      Assert.That(
        orderCalled, Is.EqualTo(new List<string> {"inner", "middle", "outer"}));
    }

    [Test]
    public void ExceptionUnhandledByActionHandlerRetainsStackTrace()
    {
      var handler = Ex.Handler<InvalidOperationException>(ex => { });
      var argEx = Assert.Throws<ArgumentException>(() =>
        handler(() =>
          {
            // Call method so that get an easily matchable name in stack trace
            ThrowArgumentException("Error", new InvalidOperationException());
          }));

      Assert.That(
        argEx.StackTrace, Is.StringContaining("ThrowArgumentException"));
    }

    [Test]
    public void ExceptionReThrownByActionHandlerRetainsStackTrace()
    {
      var handler = Ex.Handler<ArgumentException>(ex => false);
      var argEx = Assert.Throws<ArgumentException>(() =>
        handler(() =>
        {
          // Call method so that get an easily matchable name in stack trace
          ThrowArgumentException("Error", new InvalidOperationException());
        }));

      Assert.That(
        argEx.StackTrace, Is.StringContaining("ThrowArgumentException"));
    }

    [Test]
    public void ExceptionUnhandledByNestedActionHandlerRetainsStackTrace()
    {
      var innerHandler = Ex.Handler<InvalidOperationException>(ex => { });
      var outerHandler = Ex.Handler<MissingMethodException>(ex => { });
      var handler = innerHandler.Nest(outerHandler);
      var argEx = Assert.Throws<ArgumentException>(() =>
        handler(() =>
        {
          // Call method so that get an easily matchable name in stack trace
          ThrowArgumentException("Error", new InvalidOperationException());
        }));

      Assert.That(
        argEx.StackTrace, Is.StringContaining("ThrowArgumentException"));
    }

    [Test]
    public void ExceptionReThrownByNestedActionHandlerRetainsStackTrace()
    {
      var innerHandler = Ex.Handler<ArgumentException>(ex => false);
      var outerHandler = Ex.Handler<ArgumentException>(ex => false);
      var handler = innerHandler.Nest(outerHandler);
      var argEx = Assert.Throws<ArgumentException>(() =>
        handler(() =>
        {
          // Call method so that get an easily matchable name in stack trace
          ThrowArgumentException("Error", new InvalidOperationException());
        }));

      Assert.That(
        argEx.StackTrace, Is.StringContaining("ThrowArgumentException"));
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfNoExceptionForActionDelegateHandler()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception>(
        ex => { }, () => finallyWasCalled = true);

      handler(() => { });

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfNoExceptionForFuncDelegateHandler()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception>(
        ex => false, () => finallyWasCalled = true);

      handler(() => { });

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfHandledForActionDelegateHandler()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception>(
        ex => { }, () => finallyWasCalled = true);

      handler(() => { throw new Exception(); });

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfHandledForFuncDelegateHandler()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception>(
        ex => true, () => finallyWasCalled = true);

      handler(() => { throw new Exception(); });

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfExceptionUnhandled()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<ArgumentException>(
        ex => true, () => finallyWasCalled = true);

      Assert.Throws<Exception>(() => handler(() => { throw new Exception(); }));

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void FinallyBlockAlwaysCalledIfExceptionBubbledUp()
    {
      var finallyWasCalled = false;
      var handler = Ex.Handler<Exception>(
        ex => false, () => finallyWasCalled = true);

      Assert.Throws<Exception>(() => handler(() => { throw new Exception(); }));

      Assert.That(finallyWasCalled, Is.True);
    }

    [Test]
    public void CanConstructActionWithBuiltInExceptionHandling()
    {
      var handledIt = false;
      var actionWithExceptionHandling =
        Ex.Handler<ArgumentException>(ex => handledIt = true).Partial(() =>
        {
          throw new ArgumentException();
        });

      actionWithExceptionHandling();

      Assert.That(handledIt, Is.True);
    }

    private static void ThrowArgumentException(
      string message, Exception innerException)
    {
      throw new ArgumentException(message, innerException);
    }
  }
}