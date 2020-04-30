using System;

namespace CrudR.Core.Tests.Helpers
{
    public interface IExceptionAssertion
    {
        void Assert(Action assertionAction);
    }

    public interface IExceptionAssertion<TReturn>
    {
        void Assert(Func<TReturn> assertionAction);
    }
}
