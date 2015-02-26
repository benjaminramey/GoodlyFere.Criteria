#region License

// ------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExpressionExtensions.cs">
// GoodlyFere.Criteria
//  
//  Copyright (C) 2014 Ben Ramey
//  
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//  Lesser General Public License for more details.
//  
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//  
//  http://www.gnu.org/licenses/lgpl-2.1-standalone.html
//  
//  You can contact me at ben.ramey@gmail.com.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace GoodlyFere.Criteria
{
    public static class ExpressionExtensions
    {
        #region Public Methods

        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return Merge(first, second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return Merge(first, second, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> Merge<T>(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right,
            Func<Expression, Expression, Expression> mergeMethod)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(T), "arg");

            var leftReplacer = new ReplaceExpressionVisitor(left.Parameters[0], paramExpression);
            var newLeft = leftReplacer.Visit(left.Body);

            var rightReplacer = new ReplaceExpressionVisitor(right.Parameters[0], paramExpression);
            var newRight = rightReplacer.Visit(right.Body);
            
            // apply composition of lambda expression bodies to parameters from the first expression
            return Expression.Lambda<Func<T, bool>>(mergeMethod(newLeft, newRight), paramExpression);
        }

        #endregion

#if NET35
        public class ReplaceExpressionVisitor : Visitors.ExpressionVisitor
#endif
#if NET45
        public class ReplaceExpressionVisitor : ExpressionVisitor
#endif
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _newValue = newValue;
                _oldValue = oldValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
}