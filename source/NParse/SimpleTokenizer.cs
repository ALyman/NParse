//-----------------------------------------------------------------------
// <copyright file="SimpleTokenizer.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>22/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace NParse
{
    /// <summary>
    /// A simple tokenizer.
    /// </summary>
    public sealed class SimpleTokenizer : ITokenizer
    {
        IGrammar grammar;
        string buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTokenizer"/> class.
        /// </summary>
        /// <param name="grammar">The grammar.</param>
        /// <param name="reader">The reader.</param>
        public SimpleTokenizer(IGrammar grammar, TextReader reader)
        {
            this.grammar = grammar;
            this.buffer = reader.ReadToEnd();
        }

        /// <summary>
        /// Gets or sets the position in the input.
        /// </summary>
        /// <value>The position.</value>
        public int Position { get; set; }

        /// <summary>
        /// Looks ahead in the input for the specified possibilities, and returns all of them that occur at the current position.
        /// </summary>
        /// <param name="possibilities">The possibilities.</param>
        /// <returns></returns>
        public IEnumerable<Token> LookAhead(params Regex[] possibilities)
        {
            if (grammar.Whitespace != null)
            {
                var wsMatch = grammar.Whitespace.Match(buffer, Position);
                if (wsMatch.Index == Position)
                {
                    Position += wsMatch.Length;
                }
            }

            var all = from regex in possibilities
                      let m = regex.Match(buffer, Position)
                      where m.Success == true && m.Index == Position
                      select new Token(source: regex, text: m.Value, length: m.Length);

            return all;
        }

        /// <summary>
        /// Reads the next token from the input at the current position, returns the first match, and moves the input position to be immediatly after the returned token.
        /// </summary>
        /// <param name="possibilities">The possibilities.</param>
        /// <returns>The next token from the input.</returns>
        public Token ReadNext(params Regex[] possibilities)
        {
            var token = LookAhead(possibilities).FirstOrDefault();
            if (token == null)
                return Token.Failed;
            Position += token.Length;
            return token;
        }
    }
}