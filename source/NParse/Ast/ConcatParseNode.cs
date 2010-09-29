//-----------------------------------------------------------------------
// <copyright file="ConcatParseNode.cs" company="Alex Lyman">
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
    /// A parse node that consists of a list of sub-nodes concatenated together.
    /// </summary>
    public sealed class ConcatParseNode : ParseNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcatParseNode"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="nodes">The nodes.</param>
        internal ConcatParseNode(ParseExpression expression, ParseNode[] nodes)
            : base(expression)
        {
            this.Children = nodes;
        }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public IEnumerable<ParseNode> Children { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ParseNode"/> was successfully parsed.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public override bool Success
        {
            get { return Children.All(n => n.Success); }
        }
    }
}