//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>15/03/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
#define INTERACTIVE

using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using NParse;

namespace SimpleParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GrammarBase<Expression> grammar = new SimpleGrammar();
#if INTERACTIVE
            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                var input = Console.ReadLine();
                if (input == "exit") { break; }

                var parser = grammar.CreateParser();
                ////parser.Options.Trace = true;
                var result = parser.Parse(new StringReader(input));

                ExecuteExpression(result.Value);
            } while (true);
#else
            var result = grammar
                .CreateParser()
                .Parse(new StringReader("1/x"));
            ExecuteExpression(result.Value);
#endif
        }

        private static void ExecuteExpression(Expression expression)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(expression);

            var parameterDiscovery = new ParameterDiscoveryVisitor();
            parameterDiscovery.Visit(expression);

            var lambda = Expression.Lambda(expression, parameterDiscovery.Parameters);
            var compiledLambda = lambda.Compile();
            var values = (from parameter in parameterDiscovery.Parameters
                          select ReadVariable(parameter))
                          .Cast<object>()
                          .ToArray();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(compiledLambda.DynamicInvoke(values));
            Console.WriteLine();
        }

        private static double ReadVariable(ParameterExpression parameter)
        {
            string input;
            double result;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("{0} = ", parameter.Name);
                Console.ForegroundColor = ConsoleColor.White;
                input = Console.ReadLine();
            } while (!double.TryParse(input, out result));
            return result;
        }
    }
}
