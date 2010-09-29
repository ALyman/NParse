//-----------------------------------------------------------------------
// <copyright file="NotMemoizableAttribute.cs" company="Alex Lyman">
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

namespace NParse
{
    /// <summary>
    /// Marks a rule as not memoizable (it will be run in every context it is found in).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class NotMemoizableAttribute : Attribute
    {
        /// <summary>
        /// Marks a rule as not memoizable (it will be run in every context it is found in).
        /// </summary>
        public NotMemoizableAttribute() { }
    }
}