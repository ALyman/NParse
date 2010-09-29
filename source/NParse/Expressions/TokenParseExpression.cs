//-----------------------------------------------------------------------
// <copyright file="TokenParseExpression.cs" company="Alex Lyman">
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
using System.Text.RegularExpressions;
using NParse.Ast;

namespace NParse.Expressions
{
    /// <summary>
    /// An expression that parses a particular token from the input.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public sealed class TokenParseExpression<TValue> : ParseExpression<TValue>
    {
        /// <summary>
        /// Gets a value indicating whether this instance is memoizable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is memoizable; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsMemoizable { get { return true; } }

        private Regex regex;
        private Func<Token, TValue> reduce;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenParseExpression&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="reduce">The reduce.</param>
        public TokenParseExpression(Regex regex, Func<Token, TValue> reduce)
        {
            // TODO: Complete member initialization
            this.regex = regex;
            this.reduce = reduce;
        }

        /// <summary>
        /// Executes parsing in the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The resulting parse node.</returns>
        protected override ParseNode ExecuteCore(ParseContext context)
        {
            var tokenMatch = context.Tokenizer.ReadNext(regex);
            return new TokenParseNode<TValue>(this, tokenMatch, tokenMatch.Success ? reduce(tokenMatch) : default(TValue));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("/{0}/", regex);
        }

        /// <summary>
        /// Gets the potential first token regular expressions for this expression.
        /// </summary>
        /// <returns>
        /// The regular expressions that represent the first tokens that match this expression.
        /// </returns>
        public override IEnumerable<Regex> GetFirst()
        {
            yield return regex;
        }
    }
}