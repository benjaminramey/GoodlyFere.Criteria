using System;
using System.Linq;
using System.Linq.Expressions;

namespace GoodlyFere.Criteria.Tests
{
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
}