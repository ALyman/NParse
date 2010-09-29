//-----------------------------------------------------------------------
// <copyright file="ScopeParseExpression.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>28/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NParse.Ast;
using System.Text.RegularExpressions;

namespace NParse.Expressions
{
    /// <summary>
    /// An expression that reduces the contained expression to provide a value.
    /// </summary>
    public sealed class ScopeParseExpression : ParseExpression
    {
        private ParseExpression expression;
        bool isChild;
        private Action<ParseScope> initializer;
        private Action<ParseScope> finalizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeParseExpression"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="isChild">if set to <c>true</c> the scope is a child scope..</param>
        /// <param name="initializer">The initializer.</param>
        /// <param name="finalizer">The finalizer.</param>
        public ScopeParseExpression(ParseExpression expression, bool isChild, Action<ParseScope> initializer, Action<ParseScope> finalizer)
        {
            this.expression = expression;
            this.isChild = isChild;
            this.initializer = initializer;
            this.finalizer = finalizer;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is memoizable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is memoizable; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsMemoizable { get { return false; } }

        /// <summary>
        /// Executes parsing in the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The resulting parse node.</returns>
        protected override ParseNode ExecuteCore(ParseContext context)
        {
            ParseScope scope = context.BeginScope(isChild);
            if (initializer != null)
                initializer(scope);
            var node = expression.Execute(context);
            if (finalizer != null)
                finalizer(scope);
            context.EndScope(scope);
            return node;

        }

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
    }

    /// <summary>
    /// An expression that reduces the contained expression to provide a value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public sealed class ScopeParseExpression<TResult> : ParseExpression<TResult>
    {
        private ParseExpression<TResult> expression;
        private Action<ParseScope> initializer;
        private Action<ParseScope> finalizer;
        private bool isChild;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeParseExpression"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="isChild">if set to <c>true</c> the scope is a child scope..</param>
        /// <param name="initializer">The initializer.</param>
        /// <param name="finalizer">The finalizer.</param>
        public ScopeParseExpression(ParseExpression<TResult> expression, bool isChild, Action<ParseScope> initializer, Action<ParseScope> finalizer)
        {
            this.expression = expression;
            this.isChild = isChild;
            this.initializer = initializer;
            this.finalizer = finalizer;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is memoizable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is memoizable; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsMemoizable { get { return false; } }

        /// <summary>
        /// Executes parsing in the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The resulting parse node.</returns>
        protected override ParseNode ExecuteCore(ParseContext context)
        {
            ParseScope scope = context.BeginScope(isChild);
            if (initializer != null)
                initializer(scope);
            var node = expression.Execute(context);
            if (finalizer != null)
                finalizer(scope);
            context.EndScope(scope);
            return node;
        }

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
    }
}