/*
 * Written by Trevor Barnett, <mr.ullet@gmail.com>, 2015
 * Released to the Public Domain.  See http://unlicense.org/ or the
 * UNLICENSE file accompanying this source code.
 */

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
    /// var logSqlExceptionHandler = Fn.ActionHandler<SqlException>(ex =>
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
    ///   Fn.ActionHandler<InvalidOperationException>(ex =>
    ///   {
    ///     // exception handling
    ///   };
    /// var httpHandler = Fn.ActionHandler<HttpException>(
    ///   invalidOperationHandler,
    ///   ex =>
    ///   {
    ///     // exception handling
    ///   };
    /// 
    /// // ActionHandler to catch InvalidOperationException then HttpException
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
    public static Action<Action> ActionHandler<TEx>(Action<TEx> handleException)
      where TEx : Exception
    {
      return ActionHandler<TEx>(action =>
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
    /// var notFoundWebExceptionHandler = Fn.ActionHandler<WebException>(ex =>
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
    public static Action<Action> ActionHandler<TEx>(
      Func<TEx, bool> handleException)
      where TEx : Exception
    {
      /* 
       * Easier and clearer to define this "long hand" rather than trying to
       * delegate to FuncHandler.
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
    /// <returns>
    /// Func delegate that catches and handles exceptions of the configured
    /// type when called with an action to execute.
    /// </returns>
    /// <remarks>
    /// Return value may be null for reference and Nullable types.
    /// </remarks>
    public static Func<Func<TReturn>, TReturn> FuncHandler<TEx, TReturn>(
      Func<TEx, TReturn> handleException)
      where TEx : Exception
    {
      return FuncHandler<TEx, TReturn>(ex => Just(handleException(ex)));
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
    /// <returns>
    /// Func delegate that catches and handles exceptions of the configured
    /// type when called with an action to execute.
    /// </returns>
    /// <remarks>
    /// Return value may be null for reference and Nullable types.
    /// </remarks>
    public static Func<Func<TReturn>, TReturn> FuncHandler<TEx, TReturn>(
      Func<TEx, Maybe<TReturn>> handleException)
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
          Maybe<TReturn> returned = Nothing<TReturn>();
          if (tex != null)
            returned = handleException(tex);
          if (returned.HasValue)
            return returned.Value;
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
    /// Particularly useful for nesting exception handler delegates.
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

    /// <summary>
    /// Nest one Func within another.
    /// </summary>
    /// <param name="outerFunc">
    /// The outer <![CDATA[Func<Func<T>, T>]]> to wrap
    /// <paramref name="innerFunc"/>.
    /// </param>
    /// <param name="innerFunc">
    /// The inner <![CDATA[Func<Func<T>, T>]]> to nest inside
    /// <paramref name="outerFunc"/>.
    /// </param>
    /// <returns>An <![CDATA[Func<Func<T>, T>]]> delegate.</returns>
    /// <remarks>
    /// Particularly useful for nesting exception handler delegates.
    /// </remarks>
    public static Func<Func<T>, T> Nest<T>(
      Func<Func<T>, T> outerFunc, Func<Func<T>, T> innerFunc)
    {
      return fn => outerFunc(() => innerFunc(fn));
    }

    /// <summary>
    /// Nest one Func within another.
    /// </summary>
    /// <param name="outerFunc">
    /// The outer <![CDATA[Func<Func<T>, T>]]> to wrap
    /// <paramref name="innerFunc"/>.
    /// </param>
    /// <param name="innerFunc">
    /// The inner <see cref="Func{T}"/> to nest inside
    /// <paramref name="outerFunc"/>.
    /// </param>
    /// <returns>An <see cref="Func{T}"/> delegate.</returns>
    public static Func<T> Nest<T>(Func<Func<T>, T> outerFunc, Func<T> innerFunc)
    {
      return () => outerFunc(innerFunc);
    }

    /// <summary>
    /// Create a <see cref="Maybe{T}"/> instance without a value.
    /// </summary>
    public static Maybe<T> Nothing<T>()
    {
      return new Nothing<T>();
    }

    /// <summary>
    /// Create a <see cref="Maybe{T}"/> instance with a specific value.
    /// </summary>
    public static Maybe<T> Just<T>(T value)
    {
      return new Just<T>(value);
    }
  }
}
