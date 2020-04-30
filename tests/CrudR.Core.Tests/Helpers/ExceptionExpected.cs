using System;
using FluentAssertions;
using FluentAssertions.Specialized;

namespace CrudR.Core.Tests.Helpers
{
    public class ExceptionExpected<TException> : IExceptionAssertion where TException : Exception
    {
        private readonly Action<ExceptionAssertions<TException>> _assertionExpression;

        public ExceptionExpected(Action<ExceptionAssertions<TException>> assertionExpression)
        {
            _assertionExpression = assertionExpression;
        }

        public void Assert(Action assertionAction)
        {
            var asserter = assertionAction.Should().Throw<TException>();
            _assertionExpression.Invoke(asserter);
        }
    }

    public class ExceptionExpected<TException, TReturn> : IExceptionAssertion<TReturn> where TException : Exception
    {
        private readonly Action<ExceptionAssertions<TException>> _assertionExpression;

        public ExceptionExpected(Action<ExceptionAssertions<TException>> assertionExpression)
        {
            _assertionExpression = assertionExpression;
        }

        public void Assert(Func<TReturn> assertionAction)
        {
            var asserter = assertionAction.Should().Throw<TException>();
            _assertionExpression.Invoke(asserter);
        }
    }
}
