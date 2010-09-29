//-----------------------------------------------------------------------
// <copyright file="Future.cs" company="Alex Lyman">
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
    /// Represents a value that will be known in the future.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class Future<T>
    {
        T value;
        bool isSet = false;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get
            {
                if (!isSet)
                    throw new NotSupportedException("Can not predict the Future (Value was not set)");
                return value;
            }
            set
            {
                this.value = value;
                this.isSet = true;
            }
        }

        /// <summary>
        /// Performs an implicit conversion from T to <see cref="NParse.Future&lt;T&gt;"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Future<T>(T value)
        {
            return new Future<T> { Value = value };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NParse.Future&lt;T&gt;"/> to T.
        /// </summary>
        /// <param name="futureValue">The future value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator T(Future<T> futureValue)
        {
            return futureValue.Value;
        }
    }
}