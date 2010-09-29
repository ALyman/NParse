//-----------------------------------------------------------------------
// <copyright file="Grammars.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>22/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NParse.Expressions;
using System.Collections.ObjectModel;

namespace NParse
{
    /// <summary>
    /// A class containing extensions for the GrammarBase class
    /// </summary>
    public static class Grammars
    {
        /// <summary>
        /// Generates a ParseExpression for a set of binary operators.
        /// </summary>
        /// <typeparam name="TGrammar">The type of the grammar.</typeparam>
        /// <typeparam name="T">The type of the result of the binary operators.</typeparam>
        /// <param name="grammar">The grammar.</param>
        /// <param name="left">The left-side expression.</param>
        /// <param name="right">The right-side expression.</param>
        /// <param name="operators">The set of operators.</param>
        /// <returns>A ParseExpression that parses the specified binary operators.</returns>
        public static ParseExpression<T> BinaryOperators<TGrammar, T>(
            this GrammarBase<TGrammar> grammar,
            ParseExpression<T> left,
            ParseExpression<T> right,
            OperatorCollection<T> operators)
        {
            ParseExpression<T> result = left;
            foreach (var pair in operators)
            {
                var leftValue = new Future<T>();
                var rightValue = new Future<T>();
                var @operator = pair.Item1;
                var reduction = pair.Item2;

                var current = left.Into(leftValue) + @operator + right.Into(rightValue);
                var reduce = current.Reduce(() => reduction(leftValue.Value, rightValue.Value));

                result = reduce | result;
            }

            return result;
        }

        /// <summary>
        /// A collection that describes a set of operators to be generated.
        /// </summary>
        /// <typeparam name="T">The left-hand, right-hand and result type of the operators.</typeparam>
        public class OperatorCollection<T> : Collection<Tuple<ParseExpression, Func<T, T, T>>>
        {
            /// <summary>
            /// Adds the specified operator and reduction to the collection.
            /// </summary>
            /// <param name="operator">The operator.</param>
            /// <param name="reduction">The reduction.</param>
            public void Add(ParseExpression @operator, Func<T, T, T> reduction)
            {
                this.Add(Tuple.Create(@operator, reduction));
            }
        }
    }
}