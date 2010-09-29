//-----------------------------------------------------------------------
// <copyright file="ActionParseExpression.cs" company="Alex Lyman">
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
using NParse.Ast;
using System.Text.RegularExpressions;

namespace NParse.Expressions
{
    class ActionParseExpression<TValue> : ParseExpression<TValue>
    {
        private ParseExpression<TValue> expression;
        private Action<ParseNode<TValue>> action;
        private bool memoize;

        protected override bool IsMemoizable { get { return memoize; } }

        public ActionParseExpression(ParseExpression<TValue> expression, Action<ParseNode<TValue>> action, bool memoize)
        {
            this.expression = expression;
            this.action = action;
            this.memoize = memoize;
        }
        protected override ParseNode ExecuteCore(ParseContext context)
        {
            var node = (ParseNode<TValue>)expression.Execute(context);
            action(node);
            return node;
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "%" + expression.ToString();
        }

        public override IEnumerable<Regex> GetFirst()
        {
            return expression.GetFirst();
        }
    }
}