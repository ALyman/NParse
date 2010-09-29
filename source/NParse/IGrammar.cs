//-----------------------------------------------------------------------
// <copyright file="IGrammar.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>23/08/2010</created>
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
    /// An interface for Grammars
    /// </summary>
    public interface IGrammar
    {
        /// <summary>
        /// Gets the whitespace regular expression.  The tokenizer should skip over this whenever it is found.
        /// </summary>
        /// <value>The whitespace regular expression.</value>
        Regex Whitespace { get; }
    }
}