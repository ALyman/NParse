//-----------------------------------------------------------------------
// <copyright file="Token.cs" company="Alex Lyman">
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
    /// A token
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="Token"/> was successfully parsed.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; private set; }
        /// <summary>
        /// Gets or sets the source that this token was parsed for.
        /// </summary>
        /// <value>The source.</value>
        public Regex Source { get; private set; }
        /// <summary>
        /// Gets or sets the text of the token.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }
        /// <summary>
        /// Gets or sets the length of the token.
        /// </summary>
        /// <value>The length.</value>
        public int Length { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="text">The text.</param>
        /// <param name="length">The length.</param>
        public Token(Regex source, string text, int length)
        {
            this.Success = true;
            this.Source = source;
            this.Text = text;
            this.Length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        private Token() { }

        /// <summary>
        /// A failure token.
        /// </summary>
        public static readonly Token Failed = new Token { Success = false };
    }
}