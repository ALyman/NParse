//-----------------------------------------------------------------------
// <copyright file="Parser.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>22/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.IO;
using NParse.Ast;
using NParse.Expressions;

namespace NParse
{
    /// <summary>
    /// A parser for a specified grammar.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public sealed class Parser<TResult>
    {
        private GrammarBase<TResult> grammar;
        private Func<ParseExpression<TResult>> rootRule;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser&lt;TResult&gt;"/> class.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        /// <param name="rootRule">The root rule.</param>
        internal Parser(GrammarBase<TResult> grammar, Func<Expressions.ParseExpression<TResult>> rootRule)
        {
            this.grammar = grammar;
            this.rootRule = rootRule;
            this.Options = new ParseOptions();
        }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options.</value>
        public ParseOptions Options { get; set; }

        /// <summary>
        /// Parses the contents of the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>A parse tree node that represents the result of the parse.</returns>
        public ParseNode<TResult> Parse(TextReader reader)
        {
            var tokenizer = new SimpleTokenizer(grammar, reader);
            return Parse(tokenizer);
        }

        /// <summary>
        /// Parses the contents of the specified tokenizer.
        /// </summary>
        /// <param name="tokenizer">The tokenizer.</param>
        /// <returns>A parse tree node that represents the result of the parse.</returns>
        public ParseNode<TResult> Parse(ITokenizer tokenizer)
        {
            var expression = rootRule();
            var context = new ParseContext(tokenizer, this.Options);
            return (ParseNode<TResult>)expression.Execute(context);
        }
    }
}