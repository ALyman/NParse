//-----------------------------------------------------------------------
// <copyright file="ParseScope.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>28/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System.Collections;

namespace NParse
{
    /// <summary>
    /// A scope in which custom parse-time data may be defined.
    /// </summary>
    public class ParseScope
    {
        private Hashtable data = new Hashtable();

        private ParseScope parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseScope"/> class.
        /// </summary>
        public ParseScope() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseScope"/> class.
        /// </summary>
        /// <param name="parent">The parent scope.</param>
        public ParseScope(ParseScope parent) { this.parent = parent; }

        /// <summary>
        /// Gets or sets the data with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <value>The data.</value>
        public object this[object key]
        {
            get
            {
                if (data.ContainsKey(key))
                    return data[key];
                else if (parent != null)
                    return parent[key];
                else
                    return null;
            }
            set { data[key] = value; }
        }
    }
}