/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

using System;

namespace Ullet.PD.Functional
{
  /// <summary>
  /// Functional exception handling.
  /// </summary>
  public static class Ex
  {
    /// <summary>
    /// Construct an exception handler delegate handling exceptions of type
    /// <typeparamref name="TEx"/>.
    /// </summary>
    /// <typeparam name="TEx">
    /// Type of <see cref="Exception" /> handled by the handler.
    /// </typeparam>
    /// <param name="handleException">
    /// Action delegate to handle exceptions of type <typeparamref name="TEx"/>
    /// </param>
    /// <param name="finallyBlock">
    /// Optional Action delegate that is always called before returning from the
    /// try-catch block.
    /// </param>
    /// <returns>
    /// Action delegate that catches and handles exceptions of the configured
    /// type when called with an action to execute.
    /// </returns>
    /// <example>
    /// <![CDATA[
    /// var logSqlExceptionHandler = Ex.Handler<SqlException>(ex =>
    ///   {
    ///     SomeLogger.Log(ex);
    ///   });
    /// var sqlTask = () => {
    ///   // execute a query or something
    /// };
    /// logSqlExceptionHandler(sqlTask);
    /// ]]>
    /// </example>
    /// <example>
    /// <![CDATA[
    /// /* Nested handler */
    /// 
    /// var invalidOperationHandler =
    ///   Ex.Handler<InvalidOperationException>(ex =>
    ///   {
    ///     // exception handling
    ///   };
    /// var httpHandler = Ex.Handler<HttpException>(
    ///   invalidOperationHandler,
    ///   ex =>
    ///   {
    ///     // exception handling
    ///   };
    /// 
    /// // Handler to catch InvalidOperationException then HttpException
    /// // As with try..catch block, order is important if catching derived
    /// // Exception types, e.g. ArgumentNullException and ArgumentException.
    /// var invalidOperationAndHttpExceptionHandler =
    ///   Ex.Nest(httpHandler, invalidOperationHandler)
    /// 
    /// var someTask = () => {
    ///   // do some work
    /// };
    /// 
    /// // Do task with exception handling
    /// invalidOperationAndHttpExceptionHandler(someTask);
    /// 
    /// /* This example is equivalent to nested try..catch blocks:
    ///  *
    ///  * try
    ///  * {
    ///  *   try
    ///  *   {
    ///  *     // do some work
    ///  *   }
    ///  *   catch (InvalidOperationException iex)
    ///  *   {
    ///  *     // exception handling
    ///  *   }
    ///  * }
    ///  * catch (HttpException hex)
    ///  * {
    ///  *   // exception handling
    ///  * }
    ///  *
    ///  */
    /// ]]>
    /// </example>
    public static Action<Action> Handler<TEx>(
      Action<TEx> handleException, Action finallyBlock = null)
      where TEx : Exception
    {
      return Handler<TEx>(
        action =>
        {
          handleException(action);
          return true;
        },
        finallyBlock);
    }

    /// <summary>
    /// Construct an exception handler delegate handling exceptions of type
    /// <typeparamref name="TEx"/>.
    /// </summary>
    /// <typeparam name="TEx">
    /// Type of <see cref="Exception" /> handled by the handler.
    /// </typeparam>
    /// <param name="handleException">
    /// Function delegate to handle exceptions of type 
    /// <typeparamref name="TEx"/>.  Return true if exception is handled and
    /// should not propogate, or false to re-throw exception.
    /// </param>
    /// <param name="finallyBlock">
    /// Optional Action delegate that is always called before returning from the
    /// try-catch block.
    /// </param>
    /// <returns>
    /// Action delegate that catches and handles exceptions of the configured
    /// type when called with an action to execute.
    /// </returns>
    /// <remarks>
    /// Can be used to deconstruct exception handling into small single purpose
    /// functions, especially if need to catch multiple types of exceptions or
    /// handle exception differently depending on a property, such as HTTP
    /// status code.
    /// </remarks>
    /// <example>
    /// <![CDATA[
    /// /* Conditionally handle exception */
    /// 
    /// var notFoundWebExceptionHandler = Ex.Handler<WebException>(ex =>
    ///   {
    ///     if (((HttpWebResponse)ex.Response).StatusCode
    ///         == HttpStatusCode.NotFound)
    ///     {
    ///       // handle exception
    ///       return true;
    ///     }
    ///     else
    ///     {
    ///       // re-throw exception for another handler to deal with it
    ///       return false;
    ///     }
    ///   });
    /// 
    /// /* When called with an action is equivalent to:
    ///  *
    ///  * try
    ///  * {
    ///  *   // some action
    ///  * }
    ///  * catch (WebException ex)
    ///  * {
    ///  *   if (((HttpWebResponse)ex.Response).StatusCode
    ///  *       == HttpStatusCode.NotFound)
    ///  *   {
    ///  *     // handle exception
    ///  *   }
    ///  *   else
    ///  *   {
    ///  *     throw;
    ///  *   }
    ///  * }
    ///  *
    ///  */
    /// ]]>
    /// </example>
    public static Action<Action> Handler<TEx>(
      Func<TEx, bool> handleException, Action finallyBlock = null)
      where TEx : Exception
    {
      /* 
       * Easier and clearer to define this "long hand" rather than trying to
       * delegate to Handler<TEx, TReturn>.
       */

      return action =>
      {
        try
        {
          action();
        }
        catch (Exception ex)
        {
          var tex = ex as TEx;
          if (tex == null || !handleException(tex))
            throw;
        }
        finally
        {
          if (finallyBlock != null)
            finallyBlock();
        }
      };
    }

    /// <summary>
    /// Construct an exception handler delegate handling exceptions of type
    /// <typeparamref name="TEx"/> always returning a value of type
    /// <typeparamref name="TReturn"/>.
    /// </summary>
    /// <typeparam name="TEx">
    /// Type of <see cref="Exception" /> handled by the handler.
    /// </typeparam>
    /// <typeparam name="TReturn">
    /// Type returned by function delegates that can be passed to the
    /// exception handler.
    /// </typeparam>
    /// <param name="handleException">
    /// Func delegate to handle exceptions of type <typeparamref name="TEx"/>.
    /// Always returns a, possibly null, value of type
    /// <typeparamref name="TReturn"/>.
    /// </param>
    /// <param name="finallyBlock">
    /// Optional Action delegate that is always called before returning from the
    /// try-catch block.
    /// </param>
    /// <returns>
    /// Func delegate that catches and handles exceptions of the configured
    /// type when called with an action to execute.
    /// </returns>
    /// <remarks>
    /// Return value may be null for reference and Nullable types.
    /// </remarks>
    public static Func<Func<TReturn>, TReturn> Handler<TEx, TReturn>(
      Func<TEx, TReturn> handleException, Action finallyBlock = null)
      where TEx : Exception
    {
      return Handler<TEx, TReturn>(
        ex => Fn.Just(handleException(ex)), finallyBlock);
    }

    /// <summary>
    /// Construct an exception handler delegate handling exceptions of type
    /// <typeparamref name="TEx"/> always returning a value of type
    /// <typeparamref name="TReturn"/>.
    /// </summary>
    /// <typeparam name="TEx">
    /// Type of <see cref="Exception" /> handled by the handler.
    /// </typeparam>
    /// <typeparam name="TReturn">
    /// Type returned by function delegates that can be passed to the
    /// exception handler.
    /// </typeparam>
    /// <param name="handleException">
    /// Function delegate to handle exceptions of type
    /// <typeparamref name="TEx"/>.  Optionally return a value, possibly null,
    /// of type <typeparamref name="TReturn"/>.  Delegate must return an
    /// instance of <see cref="Maybe{TReturn}"/> to indicate if a value has
    /// been returned.  If HasValue property of returned instance is false then
    /// the exception will be re-thrown to be caught by another handler or to
    /// bubble up unhandled.
    /// </param>
    /// <param name="finallyBlock">
    /// Optional Action delegate that is always called before returning from the
    /// try-catch block.
    /// </param>
    /// <returns>
    /// Func delegate that catches and handles exceptions of the configured
    /// type when called with an action to execute.
    /// </returns>
    /// <remarks>
    /// Return value may be null for reference and Nullable types.
    /// </remarks>
    public static Func<Func<TReturn>, TReturn> Handler<TEx, TReturn>(
      Func<TEx, Maybe<TReturn>> handleException, Action finallyBlock = null)
      where TEx : Exception
    {
      return fn =>
      {
        try
        {
          return fn();
        }
        catch (Exception ex)
        {
          var tex = ex as TEx;
          Maybe<TReturn> returned = Fn.Nothing<TReturn>();
          if (tex != null)
            returned = handleException(tex);
          if (returned.HasValue)
            return returned.Value;
          throw;
        }
        finally
        {
          if (finallyBlock != null)
            finallyBlock();
        }
      };
    }
  }
}
