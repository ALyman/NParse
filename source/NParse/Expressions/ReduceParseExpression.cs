//-----------------------------------------------------------------------
// <copyright file="ReduceParseExpression.cs" company="Alex Lyman">
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
using System.Text.RegularExpressions;
using NParse.Ast;
using NParse.Expressions;

namespace NParse
{
    /// <summary>
    /// An expression that reduces the contained expression to provide a value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public sealed class ReduceParseExpression<TResult> : ParseExpression<TResult>
    {
        private ParseExpression expression;
        private Func<ParseContext, ParseNode, TResult> reduction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReduceParseExpression&lt;TResult&gt;"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="reduction">The reduction.</param>
        public ReduceParseExpression(ParseExpression expression, Func<ParseContext, ParseNode, TResult> reduction)
        {
            this.expression = expression;
            this.reduction = reduction;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is memoizable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is memoizable; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsMemoizable { get { return false; } }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return expression.ToString();
        }

        /// <summary>
        /// Gets the potential first token regular expressions for this expression.
        /// </summary>
        /// <returns>
        /// The regular expressions that represent the first tokens that match this expression.
        /// </returns>
        public override IEnumerable<Regex> GetFirst()
        {
            return expression.GetFirst();
        }

        /// <summary>
        /// Executes parsing in the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The resulting parse node.</returns>
        protected override ParseNode ExecuteCore(ParseContext context)
        {
            var sourceNode = expression.Execute(context);
            return new ReductionParseNode<TResult>(
                this,
                sourceNode,
                sourceNode.Success ? reduction(context, sourceNode) : default(TResult));
        }
    }
}