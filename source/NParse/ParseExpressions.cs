//-----------------------------------------------------------------------
// <copyright file="ParseExpressions.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>21/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NParse.Expressions;
using NParse.Utilities;
using System.Linq.Expressions;
using NParse.Ast;

namespace NParse
{
    /// <summary>
    /// A class containing extensions for ParseExpressions
    /// </summary>
    public static class ParseExpressions
    {
        /// <summary>
        /// Stores the value of the expression into the future variable specified.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="futureValue">The future value.</param>
        /// <returns>A parse expression that stores the value when executed.</returns>
        public static ParseExpression<TValue> Into<TValue>(this ParseExpression<TValue> expression, Future<TValue> futureValue)
        {
            return new ActionParseExpression<TValue>(
                expression,
                (node) => { futureValue.Value = node.Value; },
                true);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reduces the specified expression to provide a new value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="reduction">The reduction.</param>
        /// <returns>A parse expression that does the reduction when executed.</returns>
        public static ParseExpression<TResult> Reduce<TResult>(this ParseExpression expression, Func<TResult> reduction)
        {
            return new ReduceParseExpression<TResult>(expression, (context, src) => reduction());
        }

        /// <summary>
        /// Reduces the specified expression to provide a new value.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="reduction">The reduction.</param>
        /// <returns>
        /// A parse expression that does the reduction when executed.
        /// </returns>
        public static ParseExpression<TResult> Reduce<TSource, TResult>(this ParseExpression<TSource> expression, Func<TSource, TResult> reduction)
        {
            return new ReduceParseExpression<TSource, TResult>(expression, (context, src) => reduction(src.Value));
        }

        /// <summary>
        /// Reduces the specified expression to provide a new value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="reduction">The reduction.</param>
        /// <returns>A parse expression that does the reduction when executed.</returns>
        public static ParseExpression<TResult> Reduce<TResult>(this ParseExpression expression, Func<ParseContext, TResult> reduction)
        {
            return new ReduceParseExpression<TResult>(expression, (context, src) => reduction(context));
        }

        /// <summary>
        /// Reduces the specified expression to provide a new value.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="reduction">The reduction.</param>
        /// <returns>
        /// A parse expression that does the reduction when executed.
        /// </returns>
        public static ParseExpression<TResult> Reduce<TSource, TResult>(this ParseExpression<TSource> expression, Func<ParseContext, TSource, TResult> reduction)
        {
            return new ReduceParseExpression<TSource, TResult>(expression, (context, src) => reduction(context, src.Value));
        }

        /// <summary>
        /// Gives the expression a scope in which custom parse-time data can be defined.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="initializer">The scope initializer.</param>
        /// <param name="finalizer">The scope finalizer.</param>
        /// <returns>The scoped expression.</returns>
        public static ParseExpression WithScope(this ParseExpression expression, Action<ParseScope> initializer, Action<ParseScope> finalizer = null)
        {
            return new ScopeParseExpression(expression, true, initializer, finalizer);
        }

        /// <summary>
        /// Gives the expression a scope in which custom parse-time data can be defined.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="initializer">The scope initializer.</param>
        /// <param name="finalizer">The scope finalizer.</param>
        /// <returns>The scoped expression.</returns>
        public static ParseExpression<TResult> WithScope<TResult>(this ParseExpression<TResult> expression, Action<ParseScope> initializer, Action<ParseScope> finalizer = null)
        {
            return new ScopeParseExpression<TResult>(expression, true, initializer, finalizer);
        }

        /// <summary>
        /// Provides the value of the specified type from the concatenated expressions.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// A parse expression that gets the value of the specfied type.
        /// </returns>
        public static ParseExpression<TResult> Single<TResult>(this ConcatParseExpression expression)
        {
            return new ReduceParseExpression<TResult>(expression, (context, sourceNode) =>
            {
                var concatSourceNode = sourceNode as ConcatParseNode;

                return concatSourceNode
                    .Children
                    .OfType<ParseNode<TResult>>()
                    .Single().Value;
            });
        }

        /// <summary>
        /// Casts the value of the specified expression to the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// A parse expression that casts the value to the specfied type.
        /// </returns>
        public static ParseExpression<TResult> Cast<TResult>(this ParseExpression expression)
        {
            var alreadyTyped = expression as ParseExpression<TResult>;
            if (alreadyTyped != null)
                return alreadyTyped;

            var currentType = expression.GetValueType();

            if (typeof(TResult).IsAssignableFrom(currentType))
            {
                var reduceType = typeof(ReduceParseExpression<,>).MakeGenericType(currentType, typeof(TResult));
                var inputContext = Expression.Parameter(typeof(ParseContext), "context");
                var inputValue = Expression.Parameter(typeof(ParseNode<>).MakeGenericType(currentType), "value");
                var reduction = Expression.Lambda(
                    Expression.Convert(
                        Expression.Property(inputValue, "Value"),
                        typeof(TResult)),
                    inputContext,
                    inputValue);
                var compiledReduction = reduction.Compile();
                var result = Activator.CreateInstance(reduceType, expression, compiledReduction);
                return (ParseExpression<TResult>)result;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the type of the value for the provided expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The type of the value.</returns>
        public static Type GetValueType(this ParseExpression expression)
        {
            var query = from type in expression.GetType().EnumerateBaseTypes()
                        where type.IsGenericType
                        let generic = type.GetGenericTypeDefinition()
                        where generic == typeof(ParseExpression<>)
                        let arguments = type.GetGenericArguments()
                        select arguments[0];

            return query.SingleOrDefault();
        }

        /// <summary>
        /// Gets an enumeration of the concatenated expressions for the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The enumeration.</returns>
        internal static IEnumerable<ParseExpression> GetConcatenatedExpressions(this ParseExpression expression)
        {
            if (expression is ConcatParseExpression)
            {
                return expression as ConcatParseExpression;
            }
            else
            {
                return Enumerable.Repeat(expression, 1);
            }
        }

        /// <summary>
        /// Gets an enumeration of the possible expressions for the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The enumeration.</returns>
        public static IEnumerable<ParseExpression<T>> GetChoiceExpressions<T>(this ParseExpression<T> expression)
        {
            if (expression is ChoiceParseExpression<T>)
            {
                return expression as ChoiceParseExpression<T>;
            }
            else
            {
                return Enumerable.Repeat(expression, 1);
            }
        }
    }
}