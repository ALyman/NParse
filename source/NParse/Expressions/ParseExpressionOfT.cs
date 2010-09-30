//-----------------------------------------------------------------------
// <copyright file="ParseExpressionOfT.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>21/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System.Linq;

namespace NParse.Expressions
{
    /// <summary>
    /// An expression that represents the structure of a parsing rule, that provides a value of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of value that the expression provides.</typeparam>
    public abstract class ParseExpression<T> : ParseExpression
    {
        /// <summary>
        /// Implements the choice operator, |.
        /// </summary>
        /// <param name="left">The left-side of the alternation.</param>
        /// <param name="right">The right-side of the alternation.</param>
        /// <returns>The result of the operator.</returns>
        public static ParseExpression<T> operator |(ParseExpression<T> left, ParseExpression<T> right)
        {
            return new ChoiceParseExpression<T>(
                left.GetChoiceExpressions<T>()
                    .Concat(right.GetChoiceExpressions<T>()));
        }
    }
}