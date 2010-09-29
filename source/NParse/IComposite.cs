//-----------------------------------------------------------------------
// <copyright file="IComposite.cs" company="Alex Lyman">
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

namespace NParse
{
    /// <summary>
    /// Used by ParseExpressions when they are executing, to tell the context that a composite expression is active, and notify whether or not it failed.
    /// </summary>
    public interface IComposite : IDisposable
    {
        /// <summary>
        /// Marks this composite as having failed.
        /// </summary>
        void Failed();
    }
}