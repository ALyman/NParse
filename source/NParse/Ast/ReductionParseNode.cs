//-----------------------------------------------------------------------
// <copyright file="ReductionParseNode.cs" company="Alex Lyman">
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
    /// A parse node that represents a reduction of the containing node to a value of the specified type.
    /// </summary>
    /// <typeparam name="T">the type of value contained in the parse node.</typeparam>
    public class ReductionParseNode<T> : ParseNode<T>
    {
        private ParseNode source;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReductionParseNode&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        public ReductionParseNode(ParseExpression expression, ParseNode source, T value)
            : base(expression, value)
        {
            this.source = source;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ParseNode"/> was successfully parsed.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public override bool Success
        {
            get { return source.Success; }
        }
    }
}