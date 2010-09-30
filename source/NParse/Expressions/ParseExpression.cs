//-----------------------------------------------------------------------
// <copyright file="ParseExpression.cs" company="Alex Lyman">
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
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using NParse.Ast;

namespace NParse.Expressions
{
    /// <summary>
    /// An expression that represents the structure of a parsing rule.
    /// </summary>
    public abstract class ParseExpression
    {
        private static Dictionary<string, WeakReference> literalCache = new Dictionary<string, WeakReference>();

        private Dictionary<int, Tuple<ParseNode, int>> memoized = new Dictionary<int, Tuple<ParseNode, int>>();

        /// <summary>
        /// Gets a value indicating whether this instance is memoizable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is memoizable; otherwise, <c>false</c>.
        /// </value>
        protected abstract bool IsMemoizable { get; }

        /// <summary>
        /// Implements the concatenation operator, +.
        /// </summary>
        /// <param name="left">The left-side of the concatenation.</param>
        /// <param name="right">The right-side of the concatenation.</param>
        /// <returns>The result of the operator.</returns>
        public static ConcatParseExpression operator +(ParseExpression left, ParseExpression right)
        {
            return new ConcatParseExpression(
                left.GetConcatenatedExpressions()
                    .Concat(right.GetConcatenatedExpressions()));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="NParse.Expressions.ParseExpression"/>.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ParseExpression(string token)
        {
            WeakReference weakRef;

            if (literalCache.TryGetValue(token, out weakRef))
            {
                var value = (TokenParseExpression<string>)weakRef.Target;
                if (weakRef.IsAlive) { return value; }
            }

            var result = new TokenParseExpression<string>(
                new Regex(Regex.Escape(token)),
                (match) => match.Text);
            literalCache.Add(token, new WeakReference(result));
            return result;
        }

        /// <summary>
        /// Executes parsing in the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The resulting parse node.</returns>
        public virtual ParseNode Execute(ParseContext context)
        {
            if (!IsMemoizable)
            {
                return ExecuteCore(context);
            }
            var state = Tuple.Create(context.Position, this);
            var tuple = context.Memoizer.Memoize(
                state,
                () =>
                {
                    Trace.WriteLineIf(context.Options.Trace, state, "Begin");
                    Trace.Indent();
                    var result = ExecuteCore(context);
                    Trace.Unindent();
                    Trace.WriteLineIf(context.Options.Trace, Tuple.Create(state, result), "End");
                    return Tuple.Create(context.Position, result);
                },
                (node) =>
                {
                    Trace.WriteLineIf(context.Options.Trace, Tuple.Create(state, node), "Memoized");
                },
                () =>
                {
                    return Tuple.Create(context.Position, ParseNode.Failed(this));
                });

            context.Position = tuple.Item1;
            return tuple.Item2;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public abstract override string ToString();

        /// <summary>
        /// Gets the potential first token regular expressions for this expression.
        /// </summary>
        /// <returns>The regular expressions that represent the first tokens that match this expression.</returns>
        public abstract IEnumerable<Regex> GetFirst();

        /// <summary>
        /// Executes parsing in the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The resulting parse node.</returns>
        protected abstract ParseNode ExecuteCore(ParseContext context);
    }
}