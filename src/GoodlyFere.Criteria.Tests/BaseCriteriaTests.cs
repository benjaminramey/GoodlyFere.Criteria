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
                    new[] { new FakeCriteria2("sally").And(new FakeCriteria2("mary")) },
                    new[] { new FakeCriteria(s => s.Contains("bob")) },
                    new[] { new FakeCriteria(s => s.StartsWith("bob") && s != null) },
                    new[] { new FakeCriteria(s => s == "sam") },
                    new[] { new FakeCriteria(s => s != "joe" && s.EndsWith("alfred") || s == null) },
                    new[] { new FakeCriteria(s => s != "joe").And(new FakeCriteria(s => s.EndsWith("alfred"))) },
                    new[] { new FakeCriteria(s => s != "joe").And(new FakeCriteria(s => s.EndsWith("alfred"))).Or(new FakeCriteria(s => s == null)) },
                };
            }
        }

        [Theory]
        [PropertyData("Expressions")]
        public void ToString_IsCorrect(ICriteria<string> crit)
        {
            string expected = crit.Satisfier.ToString();

            string str = crit.ToString();

            str.Should().Be(string.Format("String: {0}", expected));
        }
    }

    internal class FakeCriteria2 : BaseCriteria<string>
    {
        private readonly string _compareTo;

        public FakeCriteria2(string compareTo)
        {
            _compareTo = compareTo;
        }

        public override Expression<Func<string, bool>> Satisfier
        {
            get
            {
                return s => s == _compareTo;
            }
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