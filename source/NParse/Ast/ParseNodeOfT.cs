//-----------------------------------------------------------------------
// <copyright file="ParseNodeOfT.cs" company="Alex Lyman">
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
    /// A typed node in the parse tree.
    /// </summary>
    /// <typeparam name="T">the type of the value contained in this parse node</typeparam>
    public abstract class ParseNode<T> : ParseNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseNode&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value.</param>
        public ParseNode(ParseExpression expression, T value)
            : base(expression)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; private set; }

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
                return String.Format("SUCCESS[{0} -> {1}]", Expression, Value);
            }
            else
            {
                return String.Format("FAILED[{0}]", Expression, Value);
            }
        }
    }
}