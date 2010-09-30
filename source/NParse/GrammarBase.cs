//-----------------------------------------------------------------------
// <copyright file="GrammarBase.cs" company="Alex Lyman">
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
using System.Reflection;
using System.Text.RegularExpressions;
using NParse.Expressions;

namespace NParse
{
    /// <summary>
    /// A base class that a grammar must implement.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class GrammarBase<TResult> : IGrammar
    {
        private Dictionary<MethodInfo, ParseExpression> ruleCache = new Dictionary<MethodInfo, ParseExpression>();

        /// <summary>
        /// Gets or sets the whitespace regular expression.  The tokenizer should skip over this whenever it is found.
        /// </summary>
        /// <value>The whitespace regular expression.</value>
        public Regex Whitespace { get; set; }

        /// <summary>
        /// Creates a parser for this grammar.
        /// </summary>
        /// <returns>A parser that will parse the language defined by this grammar.</returns>
        public Parser<TResult> CreateParser()
        {
            return new Parser<TResult>(this, RootRule);
        }

        /// <summary>
        /// The root rule used for parsing.
        /// </summary>
        /// <returns>An expression that describes how the rule is parsed.</returns>
        protected abstract ParseExpression<TResult> RootRule();

        /// <summary>
        /// Parses the specified rule in the current context.
        /// </summary>
        /// <typeparam name="T">The result type of the rule.</typeparam>
        /// <param name="rule">The rule.</param>
        /// <returns>The parse expression that describes the invocation of the rule.</returns>
        protected virtual ParseExpression<T> Rule<T>(Func<ParseExpression<T>> rule)
        {
            ParseExpression result;
            if (!ruleCache.TryGetValue(rule.Method, out result))
            {
                result = new RuleParseExpression<T>(rule);
                ruleCache.Add(rule.Method, result);
            }
            return (ParseExpression<T>)result;
        }
    }
}