//-----------------------------------------------------------------------
// <copyright file="ITokenizer.cs" company="Alex Lyman">
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

namespace NParse
{
    /// <summary>
    /// A tokenizer.
    /// </summary>
    public interface ITokenizer
    {
        /// <summary>
        /// Gets or sets the position in the input.
        /// </summary>
        /// <value>The position.</value>
        int Position { get; set; }
        /// <summary>
        /// Looks ahead in the input for the specified possibilities, and returns all of them that occur at the current position.
        /// </summary>
        /// <param name="possibilities">The possibilities.</param>
        /// <returns></returns>
        IEnumerable<Token> LookAhead(params Regex[] possibilities);
        /// <summary>
        /// Reads the next token from the input at the current position, returns the first match, and moves the input position to be immediatly after the returned token.
        /// </summary>
        /// <param name="possibilities">The possibilities.</param>
        /// <returns>The next token from the input.</returns>
        Token ReadNext(params Regex[] possibilities);
    }
}