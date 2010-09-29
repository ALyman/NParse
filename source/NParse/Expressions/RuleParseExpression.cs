//-----------------------------------------------------------------------
// <copyright file="RuleParseExpression.cs" company="Alex Lyman">
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
using NParse.Ast;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using NParse.Utilities;

namespace NParse.Expressions
{
    /// <summary>
    /// Parses a rule.
    /// </summary>
    /// <typeparam name="T">The type of the value provided by this expression.</typeparam>
    public class RuleParseExpression<T> : ParseExpression<T>
    {
        /// <summary>
        /// Gets a value indicating whether this instance is memoizable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is memoizable; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsMemoizable
        {
            get
            {
                return !rule.Method.IsDefined<NotMemoizableAttribute>(true);
            }
        }

        private Func<ParseExpression<T>> rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleParseExpression&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public RuleParseExpression(Func<ParseExpression<T>> rule)
        {
            this.rule = rule;
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
                var expression = rule();
                var result = expression.Execute(context);
                if (!result.Success)
                    composite.Failed();
                return result;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return rule.Method.Name.ToLower();
        }

        IEnumerable<Regex> first;

        /// <summary>
        /// Gets the potential first token regular expressions for this expression.
        /// </summary>
        /// <returns>
        /// The regular expressions that represent the first tokens that match this expression.
        /// </returns>
        public override IEnumerable<Regex> GetFirst()
        {
            if (first == null)
            {
                first = Enumerable.Empty<Regex>();
                var expression = rule();
                first = expression.GetFirst().ToArray();
            }

            return first;
        }
    }
}