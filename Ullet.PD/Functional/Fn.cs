using System;

namespace Ullet.PD.Functional
{
  /// <summary>
  /// A collection of functional functions.
  /// </summary>
  public static class Fn
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
    /// <returns>
    /// Action delegate that catches and handles exceptions of the configured
    /// type when called with an action to execute.
    /// </returns>
    /// <example>
    /// <![CDATA[
    /// var logSqlExceptionHandler = Fn.Handler<SqlException>(ex =>
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
    ///   Fn.Handler<InvalidOperationException>(ex =>
    ///   {
    ///     // exception handling
    ///   };
    /// var httpHandler = Fn.Handler<HttpException>(
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
    ///   Fn.Nest(httpHandler, invalidOperationHandler)
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
    public static Action<Action> Handler<TEx>(Action<TEx> handleException)
      where TEx : Exception
    {
      return Handler<TEx>(action =>
      {
        handleException(action);
        return true;
      });
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
    /// var notFoundWebExceptionHandler = Fn.Handler<WebException>(ex =>
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
    public static Action<Action> Handler<TEx>(Func<TEx, bool> handleException)
      where TEx : Exception
    {
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
      };
    }

    /// <summary>
    /// Nest one action within another.
    /// </summary>
    /// <param name="outerAction">
    /// The outer <![CDATA[Action<Action>]]> to wrap
    /// <paramref name="innerAction"/>.
    /// </param>
    /// <param name="innerAction">
    /// The inner <![CDATA[Action<Action>]]> to nest inside
    /// <paramref name="outerAction"/>.
    /// </param>
    /// <returns>An <![CDATA[Action<Action>]]> delegate.</returns>
    /// <remarks>
    /// Particularly useful for nesting exception handler delegats.
    /// </remarks>
    public static Action<Action> Nest(
      Action<Action> outerAction, Action<Action> innerAction)
    {
      return action => outerAction(() => innerAction(action));
    }

    /// <summary>
    /// Nest one action within another.
    /// </summary>
    /// <param name="outerAction">
    /// The outer <![CDATA[Action<Action>]]> to wrap
    /// <paramref name="innerAction"/>.
    /// </param>
    /// <param name="innerAction">
    /// The inner <see cref="Action"/> to nest inside
    /// <paramref name="outerAction"/>.
    /// </param>
    /// <returns>An <see cref="Action"/> delegate.</returns>
    public static Action Nest(
      Action<Action> outerAction, Action innerAction)
    {
      return () => outerAction(innerAction);
    }
  }
}
