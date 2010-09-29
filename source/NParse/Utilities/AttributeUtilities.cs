//-----------------------------------------------------------------------
// <copyright file="AttributeUtilities.cs" company="Alex Lyman">
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
using System.Reflection;

namespace NParse.Utilities
{
    static class AttributeUtilities
    {
        public static bool IsDefined<TAttribute>(this ICustomAttributeProvider customAttributeProvider, bool inherit)
            where TAttribute : Attribute
        {
            return customAttributeProvider.IsDefined(typeof(TAttribute), inherit);
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this ICustomAttributeProvider customAttributeProvider, bool inherit)
        {
            return customAttributeProvider.GetCustomAttributes(typeof(TAttribute), inherit) as TAttribute[];
        }
    }
}