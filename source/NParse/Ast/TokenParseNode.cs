//-----------------------------------------------------------------------
// <copyright file="TokenParseNode.cs" company="Alex Lyman">
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
using NParse.Expressions;

namespace NParse.Ast
{
    /// <summary>
    /// A parse tree node that represents a token.
    /// </summary>
    /// <typeparam name="T">The type of the value for the parse tree node.</typeparam>
    public class TokenParseNode<T> : ParseNode<T>
    {
        private Token match;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenParseNode&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="match">The match.</param>
        /// <param name="value">The value.</param>
        public TokenParseNode(ParseExpression expression, Token match, T value)
            : base(expression, value)
        {
            this.match = match;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ParseNode"/> was successfully parsed.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public override bool Success
        {
            get { return match.Success; }
        }
    }
}