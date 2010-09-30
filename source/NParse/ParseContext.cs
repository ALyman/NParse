//-----------------------------------------------------------------------
// <copyright file="ParseContext.cs" company="Alex Lyman">
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

namespace NParse
{
    /// <summary>
    /// The context for a parsing operation
    /// </summary>
    public class ParseContext
    {
        private Stack<ParseScope> scopeStack = new Stack<ParseScope>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseContext"/> class.
        /// </summary>
        /// <param name="tokenizer">The tokenizer.</param>
        /// <param name="options">The options.</param>
        public ParseContext(ITokenizer tokenizer, ParseOptions options)
        {
            this.Tokenizer = tokenizer;
            this.Options = options;
            if (options.Memoize)
            {
                this.Memoizer = Memoizer.Create();
            }
            else
            {
                this.Memoizer = Memoizer.Null;
            }

            BeginScope(false);
        }

        /// <summary>
        /// Gets or sets the tokenizer.
        /// </summary>
        /// <value>The tokenizer.</value>
        public ITokenizer Tokenizer { get; private set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public int Position
        {
            get { return this.Tokenizer.Position; }
            set { this.Tokenizer.Position = value; }
        }

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>The options.</value>
        public ParseOptions Options { get; private set; }

        /// <summary>
        /// Gets the memoizer.
        /// </summary>
        /// <value>The memoizer.</value>
        public Memoizer Memoizer { get; private set; }

        /// <summary>
        /// Gets the current scope.
        /// </summary>
        /// <value>The scope.</value>
        public ParseScope Scope
        {
            get
            {
                return scopeStack.Peek();
            }
        }

        /// <summary>
        /// Begins a new composite region.
        /// </summary>
        /// <returns>The composite description.</returns>
        public IComposite BeginComposite()
        {
            return new Composite(this);
        }

        /// <summary>
        /// Begins a new scope.
        /// </summary>
        /// <param name="isChild">if set to <c>true</c> the scope is a child.</param>
        /// <returns>A new scope.</returns>
        public ParseScope BeginScope(bool isChild)
        {
            ParseScope result;
            if (isChild)
            {
                result = new ParseScope(this.Scope);
            }
            else
            {
                result = new ParseScope();
            }
            scopeStack.Push(result);
            return result;
        }

        /// <summary>
        /// Ends the scope.
        /// </summary>
        /// <param name="scope">The scope.</param>
        public void EndScope(ParseScope scope)
        {
            if (scope != this.Scope)
            {
                throw new NotSupportedException();
            }
            scopeStack.Pop();
        }

        /// <summary>
        /// A composite region.
        /// </summary>
        private class Composite : IComposite
        {
            private ParseContext parseContext;
            private int position;
            private bool success = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="Composite"/> class.
            /// </summary>
            /// <param name="parseContext">The parse context.</param>
            public Composite(ParseContext parseContext)
            {
                this.parseContext = parseContext;
                this.position = parseContext.Position;
            }

            /// <summary>
            /// Marks this composite as having failed.
            /// </summary>
            public void Failed()
            {
                success = false;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (!success)
                {
                    this.parseContext.Position = this.position;
                    System.Diagnostics.Trace.WriteLineIf(parseContext.Options.Trace, "@" + position, "Backtrack");
                }
            }
        }
    }
}