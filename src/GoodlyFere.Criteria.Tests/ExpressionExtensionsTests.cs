#region Usings

using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Xunit;

#endregion

namespace GoodlyFere.Criteria.Tests
{
    public class ExpressionExtensionsTests
    {
        [Fact]
        public void And_CombinesWithAndAlso()
        {
            Expression<Func<string, bool>> expr1 = arg => arg == "bob";
            Expression<Func<string, bool>> expr2 = arg => arg == "sally";
            string expected = string.Format("arg => ({0} AndAlso {1})", expr1.Body, expr2.Body);

            string actual = ExpressionExtensions.And(expr1, expr2).ToString();

            actual.Should().Be(expected);
        }

        [Fact]
        public void Or_CombinesWithOrElse()
        {
            Expression<Func<string, bool>> expr1 = arg => arg == "bob";
            Expression<Func<string, bool>> expr2 = arg => arg == "sally";
            string expected = string.Format("arg => ({0} OrElse {1})", expr1.Body, expr2.Body);

            string actual = ExpressionExtensions.Or(expr1, expr2).ToString();

            actual.Should().Be(expected);
        }
    }
}