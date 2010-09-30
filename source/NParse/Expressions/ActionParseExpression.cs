//-----------------------------------------------------------------------
// <copyright file="ActionParseExpression.cs" company="Alex Lyman">
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

namespace NParse.Expressions
{
    /// <summary>
    /// An expression that executes an action when successfully parsed.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class ActionParseExpression<TValue> : ParseExpression<TValue>
    {
        private ParseExpression<TValue> expression;
        private Action<ParseNode<TValue>> action;
        private bool memoize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionParseExpression&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="action">The action.</param>
        /// <param name="memoize">if set to <c>true</c> [memoize].</param>
        public ActionParseExpression(ParseExpression<TValue> expression, Action<ParseNode<TValue>> action, bool memoize)
        {
            this.expression = expression;
            this.action = action;
            this.memoize = memoize;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is memoizable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is memoizable; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsMemoizable { get { return memoize; } }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "%" + expression.ToString();
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
            var node = (ParseNode<TValue>)expression.Execute(context);
            action(node);
            return node;
            throw new NotImplementedException();
        }
    }
}