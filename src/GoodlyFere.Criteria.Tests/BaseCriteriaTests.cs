#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using FluentAssertions;
using Xunit.Extensions;

#endregion

namespace GoodlyFere.Criteria.Tests
{
    public class BaseCriteriaTests
    {
        public static IEnumerable<object[]> Expressions
        {
            get
            {
                return new[]
                {
                    new Expression<Func<string, bool>>[] { s => s.Contains("bob") },
                    new Expression<Func<string, bool>>[] { s => s.StartsWith("bob") && s != null },
                    new Expression<Func<string, bool>>[] { s => s == "sam" },
                    new Expression<Func<string, bool>>[] { s => s != "joe" && s.EndsWith("alfred") || s == null },
                };
            }
        }

        [Theory]
        [PropertyData("Expressions")]
        public void ToString_IsCorrect(Expression<Func<string, bool>> expr)
        {
            var crit = new FakeCriteria(expr);
            string expected = expr.ToString();

            string str = crit.ToString();

            str.Should().Be(string.Format("String: {0}", expected));
        }
    }

    internal class FakeCriteria : BaseCriteria<string>
    {
        private readonly Expression<Func<string, bool>> _expr;

        public FakeCriteria(Expression<Func<string, bool>> expr)
        {
            _expr = expr;
        }

        public override Expression<Func<string, bool>> Satisfier
        {
            get
            {
                return _expr;
            }
        }
    }
}