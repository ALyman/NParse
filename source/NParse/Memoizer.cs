//-----------------------------------------------------------------------
// <copyright file="Memoizer.cs" company="Alex Lyman">
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

namespace NParse
{
    /// <summary>
    /// Memoizes results to function calls.
    /// </summary>
    public class Memoizer
    {
        /// <summary>
        /// A Null Memoizer that does not actually memoize results.
        /// </summary>
        public static readonly Memoizer Null = new NullMemoizer();

        /// <summary>
        /// A mapping of state-type/result-type tuples to the corresponding memoized state-to-result mapping dictionaries.
        /// </summary>
        private Dictionary<Tuple<Type, Type>, object> stateMap = new Dictionary<Tuple<Type, Type>, object>();

        /// <summary>
        /// Prevents a default instance of the <see cref="Memoizer"/> class from being created.
        /// </summary>
        private Memoizer()
        {
        }

        /// <summary>
        /// Creates a new Memoizer.
        /// </summary>
        /// <returns>A newly created Memoizer</returns>
        public static Memoizer Create()
        {
            return new Memoizer();
        }

        /// <summary>
        /// Memoizes the specified state, returning either the already-generated and memoized value, or a newly generated value if it was not found.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="generate">A function that generates the value requested.</param>
        /// <param name="callback">The callback that is called if the state was already found.</param>
        /// <returns>A memoized value, if one exists, otherwise the generated value.</returns>
        public TResult Memoize<TState, TResult>(TState state, Func<TResult> generate, Action callback = null)
        {
            if (callback == null)
            {
                return Memoize(state, s => generate());
            }
            else
            {
                return Memoize(state, s => generate(), (s, r) => callback());
            }
        }

        /// <summary>
        /// Memoizes the specified state, returning either the already-generated and memoized value, or a newly generated value if it was not found.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="generate">A function that generates the value requested.</param>
        /// <param name="callback">The callback that is called if the state was already found.</param>
        /// <returns>A memoized value, if one exists, otherwise the generated value.</returns>
        public TResult Memoize<TState, TResult>(TState state, Func<TState, TResult> generate, Action callback)
        {
            if (callback == null)
            {
                return Memoize(state, s => generate(s));
            }
            else
            {
                return Memoize(state, s => generate(s), (s, r) => callback());
            }
        }

        /// <summary>
        /// Memoizes the specified state, returning either the already-generated and memoized value, or a newly generated value if it was not found.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="generate">A function that generates the value requested.</param>
        /// <param name="callback">The callback that is called if the state was already found.</param>
        /// <returns>A memoized value, if one exists, otherwise the generated value.</returns>
        public TResult Memoize<TState, TResult>(TState state, Func<TResult> generate, Action<TState, TResult> callback)
        {
            return Memoize(state, s => generate(), callback);
        }

        /// <summary>
        /// Memoizes the specified state, returning either the already-generated and memoized value, or a newly generated value if it was not found.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="generate">A function that generates the value requested.</param>
        /// <param name="callback">The callback that is called if the state was already found.</param>
        /// <param name="recursiveResult">The recursive result.</param>
        /// <returns>
        /// A memoized value, if one exists, otherwise the generated value.
        /// </returns>
        public TResult Memoize<TState, TResult>(TState state, Func<TResult> generate, Action<TResult> callback, Func<TResult> recursiveResult = null)
        {
            if (callback == null)
            {
                return Memoize(state, s => generate(), recursiveResult: recursiveResult);
            }
            else
            {
                return Memoize(state, s => generate(), (s, r) => callback(r), recursiveResult: recursiveResult);
            }
        }

        /// <summary>
        /// Memoizes the specified state, returning either the already-generated and memoized value, or a newly generated value if it was not found.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="generate">A function that generates the value requested.</param>
        /// <param name="callback">The callback that is called if the state was already found.</param>
        /// <param name="recursiveResult">The recursive result.</param>
        /// <returns>
        /// A memoized value, if one exists, otherwise the generated value.
        /// </returns>
        public TResult Memoize<TState, TResult>(TState state, Func<TState, TResult> generate, Action<TResult> callback, Func<TResult> recursiveResult = null)
        {
            if (callback == null)
            {
                return Memoize(state, s => generate(s), recursiveResult: recursiveResult);
            }
            else
            {
                return Memoize(state, s => generate(s), (s, r) => callback(r), recursiveResult: recursiveResult);
            }
        }

        /// <summary>
        /// Memoizes the specified state, returning either the already-generated and memoized value, or a newly generated value if it was not found.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="generate">A function that generates the value requested.</param>
        /// <param name="callback">The callback that is called if the state was already found.</param>
        /// <param name="recursiveResult">The recursive result.</param>
        /// <returns>
        /// A memoized value, if one exists, otherwise the generated value.
        /// </returns>
        public virtual TResult Memoize<TState, TResult>(TState state, Func<TState, TResult> generate, Action<TState, TResult> callback = null, Func<TResult> recursiveResult = null)
        {
            var mapping = GetStateMapping<TState, TResult>();

            TResult result;
            if (mapping.TryGetValue(state, out result))
            {
                callback(state, result);
            }
            else
            {
                if (recursiveResult != null)
                {
                    mapping[state] = recursiveResult();
                }
                result = generate(state);
                mapping[state] = result;
            }

            return result;
        }

        /// <summary>
        /// Gets the state mapping dictionary for the specified state and result types.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <returns>A dictionary for the specified state and result types.</returns>
        private Dictionary<TState, TResult> GetStateMapping<TState, TResult>()
        {
            var key = Tuple.Create(typeof(TState), typeof(TResult));
            object mapping;
            if (!this.stateMap.TryGetValue(key, out mapping))
            {
                mapping = new Dictionary<TState, TResult>();
                this.stateMap.Add(key, mapping);
            }

            return (Dictionary<TState, TResult>)mapping;
        }

        /// <summary>
        /// A Null (non-memoizing) memoizer.
        /// </summary>
        private class NullMemoizer : Memoizer
        {
            /// <summary>
            /// Memoizes the specified state, returning either the already-generated and memoized value, or a newly generated value if it was not found.
            /// </summary>
            /// <typeparam name="TState">The type of the state.</typeparam>
            /// <typeparam name="TResult">The type of the result.</typeparam>
            /// <param name="state">The state.</param>
            /// <param name="generate">A function that generates the value requested.</param>
            /// <param name="callback">The callback that is called if the state was already found.</param>
            /// <param name="recursiveResult">The recursive result.</param>
            /// <returns>
            /// A memoized value, if one exists, otherwise the generated value.
            /// </returns>
            public override TResult Memoize<TState, TResult>(TState state, Func<TState, TResult> generate, Action<TState, TResult> callback = null, Func<TResult> recursiveResult = null)
            {
                return generate(state);
            }
        }
    }
}