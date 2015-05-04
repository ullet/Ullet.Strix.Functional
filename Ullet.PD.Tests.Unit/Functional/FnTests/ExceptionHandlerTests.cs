using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ullet.PD.Functional;

namespace Ullet.PD.Tests.Unit.Functional.FnTests
{
  [TestFixture]
  public class ExceptionHandlerTests
  {
    [Test]
    public void CanConstructHandler()
    {
      var handledIt = false;

      Action<Action> handler
        = Fn.Handler<ArgumentException>(ex => handledIt = true);
      handler(() => { throw new ArgumentException(); });

      Assert.That(handledIt, Is.True);
    }

    [Test]
    public void CanConstructNestedHandler()
    {
      string handledBy = null;

      Action<Action> innerHandler
        = Fn.Handler<ArgumentException>(ex => handledBy = "ArgumentException");
      Action<Action> outerHandler = Fn.Handler<InvalidOperationException>(
        ex => handledBy = "InvalidOperationException");
      Action<Action> nestedHandler = Fn.Nest(innerHandler, outerHandler);
      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(handledBy, Is.EqualTo("InvalidOperationException"));
    }

    [Test]
    public void OuterHandlerNotCalledIfAlreadyHandled()
    {
      var orderCalled = new List<string>();
      Action<Action> innerHandler
        = Fn.Handler<Exception>(ex => orderCalled.Add("Inner"));
      Action<Action> outerHandler 
        = Fn.Handler<Exception>(ex => orderCalled.Add("Outer"));
      Action<Action> nestedHandler = Fn.Nest(outerHandler, innerHandler);

      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(orderCalled, Is.EqualTo(new List<string> {"Inner"}));
    }

    [Test]
    public void CanConstructHandlerFromFunction()
    {
      Action<Action> handler = Fn.Handler<ArgumentException>(ex => false);
      Assert.Throws<ArgumentException>(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void ThrowExceptionIfHandlerFunctionReturnsFalse()
    {
      Action<Action> handler = Fn.Handler<ArgumentException>(ex => false);
      Assert.Throws<ArgumentException>(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void DoesNotThrowExceptionIfHandlerFunctionReturnsTrue()
    {
      Action<Action> handler = Fn.Handler<ArgumentException>(ex => true);
      Assert.DoesNotThrow(
        () => handler(() => { throw new ArgumentException(); }));
    }

    [Test]
    public void ExceptionThrownIfNotHandled()
    {
      var handled = false;
      Action<Action> handler
        = Fn.Handler<ArgumentException>(ex => handled = true);
      Assert.Throws<InvalidOperationException>(
        () => handler(() => { throw new InvalidOperationException(); }));
      Assert.That(handled, Is.False);
    }

    [Test]
    public void CanConstructNestedHandlerFromFuncHandlers()
    {
      var handled = false;

      Action<Action> innerHandler
        = Fn.Handler<ArgumentException>(ex => false);
      Action<Action> outerHandler = Fn.Handler<InvalidOperationException>(
        ex =>
        {
          handled = true;
          return true;
        });
      Action<Action> nestedHandler = Fn.Nest(innerHandler, outerHandler);
      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(handled, Is.True);
    }

    [Test]
    public void CanConstructNestedHandlerFromFuncAndNonActionHandlers()
    {
      var handled = false;

      Action<Action> innerHandler
        = Fn.Handler<ArgumentNullException>(ex => false);
      Action<Action> middleHandler = Fn.Handler<ArgumentException>(ex => { });
      Action<Action> outerHandler = Fn.Handler<InvalidOperationException>(
        ex =>
        {
          handled = true;
          return true;
        });
      Action<Action> nestedHandler = 
        Fn.Nest(outerHandler, Fn.Nest(middleHandler, innerHandler));
      nestedHandler(() => { throw new InvalidOperationException(); });

      Assert.That(handled, Is.True);
    }

    [Test]
    public void AllMatchingHandlersCalledUntilHandled()
    {
      /*
       * The behaviour is as if nested try..catch blocks rather than a single
       * try..catch with many catch blocks.
       * i.e. equivalent structure is:
       * try
       * {
       *   try
       *   {
       *     try
       *     {
       *       // action
       *     }
       *     catch (InnerException iex)
       *     {
       *       // try to handle it but if not ...
       *       throw;
       *     }
       *   }
       *   catch (MiddleException mex)
       *   {
       *     // try to handle it but if not ...
       *     throw;
       *   }
       * }
       * catch (OuterException oex)
       * {
       *   // try to handle it but if not ...
       *   throw;
       * }
       */

      var callCount = 0;
      Action<Action> innerHandler = Fn.Handler<Exception>(
        ex =>
        {
          callCount++;
          return false;
        });
      Action<Action> middleHandler = Fn.Handler<Exception>(
        ex =>
        {
          callCount++;
          return false;
        });
      Action<Action> outerHandler = Fn.Handler<Exception>(
        ex =>
        {
          callCount++;
          return true;
        });
      var nestedHandler =
        Fn.Nest(outerHandler, Fn.Nest(middleHandler, innerHandler));

      nestedHandler(() => { throw new Exception(); });

      Assert.That(callCount, Is.EqualTo(3));
    }

    [Test]
    public void HandlersCalledInOrderFromInnerToOuter()
    {
      var orderCalled = new List<string>();
      Action<Action> innerHandler = Fn.Handler<Exception>(
        ex =>
        {
          orderCalled.Add("inner");
          return false;
        });
      Action<Action> middleHandler = Fn.Handler<Exception>(
          ex =>
          {
            orderCalled.Add("middle");
            return false;
          });
      Action<Action> outerHandler = Fn.Handler<Exception>(
        ex =>
        {
          orderCalled.Add("outer");
          return true;
        });
      var nestedHandler =
        Fn.Nest(outerHandler, Fn.Nest(middleHandler, innerHandler));
      
      nestedHandler(() => { throw new Exception(); });

      Assert.That(
        orderCalled, Is.EqualTo(new List<string> {"inner", "middle", "outer"}));
    }

    [Test]
    public void UnhandledExceptionRetainsStackTrace()
    {
      var handler = Fn.Handler<InvalidOperationException>(ex => { });
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
    public void ReThrownExceptionRetainsStackTrace()
    {
      var handler = Fn.Handler<ArgumentException>(ex => false);
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
    public void UnhandledExceptionByNestedHandlerRetainsStackTrace()
    {
      var innerHandler = Fn.Handler<InvalidOperationException>(ex => { });
      var outerHandler = Fn.Handler<MissingMethodException>(ex => { });
      var handler = Fn.Nest(innerHandler, outerHandler);
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
    public void ReThrownExceptionByNestedHandlerRetainsStackTrace()
    {
      var innerHandler = Fn.Handler<ArgumentException>(ex => false);
      var outerHandler = Fn.Handler<ArgumentException>(ex => false);
      var handler = Fn.Nest(innerHandler, outerHandler);
      var argEx = Assert.Throws<ArgumentException>(() =>
        handler(() =>
        {
          // Call method so that get an easily matchable name in stack trace
          ThrowArgumentException("Error", new InvalidOperationException());
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
