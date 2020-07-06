using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Libardo
	{
		[Fact]
		public void Can_mix_assert_was_call_with_verify_all()
		{
			MockRepository mocks = new MockRepository();
			var errorHandler = mocks.DynamicMock<IErrorHandler>();
			mocks.ReplayAll();

			var ex = new Exception("Take this");
			errorHandler.HandleError(ex);

			errorHandler.AssertWasCalled(eh => eh.HandleError(ex));

			mocks.ReplayAll();
			mocks.VerifyAll(); // Can I still keep this somehow?
		}
	}

	public interface IErrorHandler
	{
		void HandleError(Exception e);
	}
}
