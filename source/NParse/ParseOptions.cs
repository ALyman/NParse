//-----------------------------------------------------------------------
// <copyright file="ParseOptions.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>28/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NParse
{
    /// <summary>
    /// Options for parsing.
    /// </summary>
    public sealed class ParseOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseOptions"/> class.
        /// </summary>
        public ParseOptions()
        {
            this.Memoize = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ParseContext"/> is trace.
        /// </summary>
        /// <value><c>true</c> if trace; otherwise, <c>false</c>.</value>
        public bool Trace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ParseOptions"/> should memoize results.
        /// </summary>
        /// <value><c>true</c> if memoize; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool Memoize { get; set; }
    }
}