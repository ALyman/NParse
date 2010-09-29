//-----------------------------------------------------------------------
// <copyright file="ParseNode.cs" company="Alex Lyman">
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

namespace NParse.Ast
{
    /// <summary>
    /// A node in the parse tree.
    /// </summary>
    public abstract class ParseNode
    {
        /// <summary>
        /// Gets or sets the expression that this ParseNode represents.
        /// </summary>
        /// <value>The expression.</value>
        protected ParseExpression Expression { get; private set; }

        internal ParseNode(ParseExpression expression)
        {
            this.Expression = expression;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ParseNode"/> was successfully parsed.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public abstract bool Success { get; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (Success)
            {
                return String.Format("SUCCESS[{0}]", Expression);
            }
            else
            {
                return String.Format("FAILED[{0}]", Expression);
            }
        }

        /// <summary>
        /// Provides a Generic ParseNode that specifies a general failure for the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A ParseNode that specifies a general failure for the specified expression.</returns>
        public static ParseNode Failed(ParseExpression expression)
        {
            return new FailedParseNode(expression);
        }

        class FailedParseNode : ParseNode
        {
            public FailedParseNode(ParseExpression expression)
                : base(expression) { }

            public override bool Success { get { return false; } }

            public override string ToString()
            {
                return "<FAILED>";
            }
        }
    }
}