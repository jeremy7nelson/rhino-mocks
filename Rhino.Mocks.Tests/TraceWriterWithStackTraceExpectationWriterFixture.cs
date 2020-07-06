namespace Rhino.Mocks.Tests
{
    using Rhino.Mocks.Impl;
    using System.IO;
    using Xunit;


    public class TraceWriterWithStackTraceExpectationWriterFixture
    {
        [Fact]
        public void WillPrintLogInfoWithStackTrace()
        {
            TraceWriterWithStackTraceExpectationWriter expectationWriter = new TraceWriterWithStackTraceExpectationWriter();
            StringWriter writer = new StringWriter();
            expectationWriter.AlternativeWriter = writer;

            RhinoMocks.Logger = expectationWriter;

            MockRepository mocks = new MockRepository();
            IDemo mock = mocks.StrictMock<IDemo>();
            mock.VoidNoArgs();
            mocks.ReplayAll();
            mock.VoidNoArgs();
            mocks.VerifyAll();

            Assert.Contains("WillPrintLogInfoWithStackTrace",
                writer.GetStringBuilder().ToString());
        }
    }
}