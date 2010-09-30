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
using System.Text.RegularExpressions;

namespace NParse
{
    /// <summary>
    /// A token
    /// </summary>
    public class Token
    {
        /// <summary>
        /// A failure token.
        /// </summary>
        public static readonly Token Failed = new Token { Success = false };

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
        /// Prevents a default instance of the <see cref="Token"/> class from being created.
        /// </summary>
        private Token() { }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Token"/> was successfully parsed.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; private set; }
        /// <summary>
        /// Gets the source that this token was parsed for.
        /// </summary>
        /// <value>The source.</value>
        public Regex Source { get; private set; }
        /// <summary>
        /// Gets the text of the token.
        /// </summary>
        /// <value>The matched text of the token.</value>
        public string Text { get; private set; }
        /// <summary>
        /// Gets the length of the token.
        /// </summary>
        /// <value>The length of the text matched.</value>
        public int Length { get; private set; }
    }
}