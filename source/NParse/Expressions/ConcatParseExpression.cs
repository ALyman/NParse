using System.Collections;
//-----------------------------------------------------------------------
// <copyright file="ConcatParseExpression.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>21/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NParse.Ast;

namespace NParse.Expressions
{
    /// <summary>
    /// A parse expression that consists of a list of sub-expressions concatenated together.
    /// </summary>
    public class ConcatParseExpression : ParseExpression, IEnumerable<ParseExpression>
    {
        private ParseExpression[] expressions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcatParseExpression"/> class.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        public ConcatParseExpression(params ParseExpression[] expressions)
            : this(expressions.AsEnumerable())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcatParseExpression"/> class.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        public ConcatParseExpression(IEnumerable<ParseExpression> expressions)
        {
            this.expressions = expressions.ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is memoizable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is memoizable; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsMemoizable { get { return true; } }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            foreach (var item in expressions)
            {
                sb.Append(item);
                sb.Append(" + ");
            }
            sb.Length -= 3;
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Gets the potential first token regular expressions for this expression.
        /// </summary>
        /// <returns>
        /// The regular expressions that represent the first tokens that match this expression.
        /// </returns>
        public override IEnumerable<Regex> GetFirst()
        {
            return expressions.First().GetFirst();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the concatenated expressions.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator&lt;ParseExpression&lt;T&gt;&gt;"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ParseExpression> GetEnumerator()
        {
            return ((IEnumerable<ParseExpression>)expressions).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the concatenated expressions.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Executes parsing in the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The resulting parse node.</returns>
        protected override ParseNode ExecuteCore(ParseContext context)
        {
            using (var composite = context.BeginComposite())
            {
                List<ParseNode> nodes = new List<ParseNode>();
                foreach (var expr in expressions)
                {
                    var node = expr.Execute(context);
                    nodes.Add(node);
                    if (!node.Success)
                    {
                        composite.Failed();
                        break;
                    }
                }
                return new ConcatParseNode(this, nodes.ToArray());
            }
        }
    }
}