using System;
using FluentAssertions;

namespace CrudR.Core.Tests.Helpers
{
    public class ExceptionNotExpected : IExceptionAssertion
    {
        public void Assert(Action assertionAction)
        {
            assertionAction.Should().NotThrow();
        }
    }

    public class ExceptionNotExpected<TReturn> : IExceptionAssertion<TReturn>
    {
        public void Assert(Func<TReturn> assertionAction)
        {
            assertionAction.Should().NotThrow();
        }
    }
}
