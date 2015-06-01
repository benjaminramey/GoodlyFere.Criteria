using System;
using System.Linq;
using System.Linq.Expressions;

namespace GoodlyFere.Criteria.Tests
{
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