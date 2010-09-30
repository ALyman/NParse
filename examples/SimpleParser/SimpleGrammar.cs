//-----------------------------------------------------------------------
// <copyright file="SimpleGrammar.cs" company="Alex Lyman">
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
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using NParse;
using NParse.Expressions;

namespace SimpleParser
{
    public class SimpleGrammar : GrammarBase<Expression>
    {
        private TokenParseExpression<double> number = new TokenParseExpression<double>(
                new Regex(@"\d+"),
                (match) => Convert.ToDouble(match.Text));

        private TokenParseExpression<string> identifier = new TokenParseExpression<string>(
                new Regex(@"\w+"),
                (match) => match.Text);

        public SimpleGrammar()
        {
            Whitespace = new Regex(@"[\s\n]+", RegexOptions.Singleline);
        }

        protected override ParseExpression<Expression> RootRule()
        {
            return Rule(Sum).WithScope((ParseScope scope) =>
            {
                scope["variables"] = new Dictionary<string, ParameterExpression>();
            });
        }

        private ParseExpression<Expression> Sum()
        {
            return this.BinaryOperators(
                Rule(Product),
                Rule(Sum),
                new Grammars.OperatorCollection<Expression>
                {
                    { "+", (left, right) => Expression.Add(left, right) },
                    { "-", (left, right) => Expression.Subtract(left, right) },
                });
        }

        private ParseExpression<Expression> Product()
        {
            return this.BinaryOperators(
                Rule(Exponential),
                Rule(Product),
                new Grammars.OperatorCollection<Expression>
                {
                    { "*", (left, right) => Expression.Multiply(left, right) },
                    { "/", (left, right) => Expression.Divide(left, right) },
                });
        }

        private ParseExpression<Expression> Exponential()
        {
            return this.BinaryOperators(
                Rule(Atom),
                Rule(Exponential),
                new Grammars.OperatorCollection<Expression>
                {
                    { "^", (left, right) => Expression.Power(left, right) },
                    { "**", (left, right) => Expression.Power(left, right) },
                });
        }

        private ParseExpression<Expression> Atom()
        {
            return number.Reduce((value) => Expression.Constant(value)).Cast<Expression>()
                | identifier.Reduce((context, value) =>
                    {
                        var variables = context.Scope["variables"] as Dictionary<string, ParameterExpression>;
                        ParameterExpression p;
                        if (!variables.TryGetValue(value, out p))
                            return variables[value] = Expression.Parameter(typeof(double), value);
                        return p;
                    }).Cast<Expression>()
                | ("(" + Rule(Sum) + ")").Single<Expression>();
        }
    }
}