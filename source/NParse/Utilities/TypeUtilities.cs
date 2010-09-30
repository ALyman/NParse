//-----------------------------------------------------------------------
// <copyright file="TypeUtilities.cs" company="Alex Lyman">
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

namespace NParse.Utilities
{
    internal static class TypeUtilities
    {
        public static IEnumerable<Type> EnumerateBaseTypes(this Type type)
        {
            for (; type != null; type = type.BaseType)
            {
                yield return type;
            }
        }
    }
}