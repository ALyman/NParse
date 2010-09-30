using System.Collections;
//-----------------------------------------------------------------------
// <copyright file="ChoiceParseExpression.cs" company="Alex Lyman">
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
    /// A expression that represents a choice between potential expressions.
    /// </summary>
    /// <typeparam name="T">The type of value that the expression represents..</typeparam>
    public class ChoiceParseExpression<T> : ParseExpression<T>, IEnumerable<ParseExpression<T>>
    {
        private ParseExpression<T>[] expressions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceParseExpression&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="expressions">The potential expressions.</param>
        public ChoiceParseExpression(params ParseExpression<T>[] expressions)
            : this(expressions.AsEnumerable())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceParseExpression&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="expressions">The potential expressions.</param>
        public ChoiceParseExpression(IEnumerable<ParseExpression<T>> expressions)
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
                sb.Append(" | ");
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
            return expressions.SelectMany(e => e.GetFirst()).Distinct();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the potential expressions.
        /// </summary>
        /// An <see cref="T:System.Collections.IEnumerator&lt;ParseExpression&lt;T&gt;&gt;"/> object that can be used to iterate through the collection.
        /// <returns>The enumerations of the potential expressions.</returns>
        public IEnumerator<ParseExpression<T>> GetEnumerator()
        {
            return ((IEnumerable<ParseExpression<T>>)expressions).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the potential expressions.
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
            var canidates = (from e in expressions
                             from f in e.GetFirst()
                             select new { e, f })
                            .ToLookup(o => o.f, o => o.e);

            var tokens = context.Tokenizer.LookAhead(canidates.Select(l => l.Key).ToArray());

            var selected = (from token in tokens
                            from c in canidates[token.Source]
                            select c).Distinct().ToArray();

            using (var composite = context.BeginComposite())
            {
                var nodes = selected.Select(e => e.Execute(context)).Cast<ParseNode<T>>();
                foreach (var node in nodes)
                {
                    if (node.Success)
                        return node;
                }

                composite.Failed();
                return new ChoiceParseNode<T>(this, nodes.ToArray());
            }
        }
    }
}