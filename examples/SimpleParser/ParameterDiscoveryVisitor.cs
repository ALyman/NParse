//-----------------------------------------------------------------------
// <copyright file="ParameterDiscoveryVisitor.cs" company="Alex Lyman">
//     Copyright (c) Alex Lyman. All rights reserved.
// </copyright>
// <link rel="website" href="http://github.com/ALyman/NParse" />
// <link rel="license" href="http://creativecommons.org/licenses/BSD/" />
// <author>Alex Lyman</author>
// <created>28/08/2010</created>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimpleParser
{
    internal class ParameterDiscoveryVisitor : ExpressionVisitor
    {
        public ParameterDiscoveryVisitor()
        {
            this.Parameters = new HashSet<ParameterExpression>();
        }

        public HashSet<ParameterExpression> Parameters { get; private set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            this.Parameters.Add(node);
            return base.VisitParameter(node);
        }
    }
}