using System;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace GoodlyFere.Criteria.Tests
{
    public class NotCriteriaTests
    {
        [Fact]
        public void Satisfier_IsNegativeOfOriginalCriteria()
        {
            // arrange
            var positiveCrit = new FakeCriteria2("sally");

            // act
            var negativeCrit = positiveCrit.Not();

            // assert
            positiveCrit.IsSatisfiedBy("sally").Should().BeTrue();
            negativeCrit.IsSatisfiedBy("sally").Should().BeFalse();
        }
    }
}
