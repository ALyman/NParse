//-----------------------------------------------------------------------
// <copyright file="ChoiceParseNode.cs" company="Alex Lyman">
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
    /// A parse node that represents a choice between potential expressions.
    /// </summary>
    /// <typeparam name="T">The type of value contained by this node.</typeparam>
    public class ChoiceParseNode<T> : ParseNode<T>
    {
        private ParseNode<T>[] nodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceParseNode&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="nodes">The nodes.</param>
        public ChoiceParseNode(ParseExpression<T> expression, ParseNode<T>[] nodes)
            : base(expression, default(T))
        {
            this.nodes = nodes;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ParseNode"/> was successfully parsed.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public override bool Success
        {
            get { return nodes.Any(n => n.Success); }
        }
    }
}