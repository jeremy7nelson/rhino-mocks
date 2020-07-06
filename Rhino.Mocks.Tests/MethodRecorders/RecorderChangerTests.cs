﻿#region license
// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion


using Rhino.Mocks.Expectations;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks.MethodRecorders;
using Rhino.Mocks.Tests.Expectations;
using Rhino.Mocks.Tests.Impl;
using System.Reflection;
using Xunit;

namespace Rhino.Mocks.Tests.MethodRecorders
{
    using Generated;


    public class RecorderChangerTests
    {
        private object proxy;
        private MethodInfo method;
        private IExpectation expectation;
        private object[] args;
        private MockRepository mocks;

        public RecorderChangerTests()
        {
            proxy = new object();
            method = typeof(object).GetMethod("ToString");
            expectation = new AnyArgsExpectation(new FakeInvocation(method), new Range(1, 1));
            args = new object[0];
            mocks = new MockRepository();
        }

        [Fact]
        public void ChangeRecorderOnCtor()
        {
            IMethodRecorder recorder = new UnorderedMethodRecorder(new ProxyMethodExpectationsDictionary());
            MethodRecorderBaseTests.TestMethodRecorder testRecorder = new MethodRecorderBaseTests.TestMethodRecorder();
            new RecorderChanger(mocks, recorder, testRecorder);
            recorder.GetAllExpectationsForProxy(new object());
            Assert.True(testRecorder.DoGetAllExpectationsForProxyCalled);
            Assert.Same(testRecorder, Get.Recorder(mocks));
        }

        [Fact]
        public void ChangeBackOnDispose()
        {
            IMethodRecorder recorder = new UnorderedMethodRecorder(new ProxyMethodExpectationsDictionary());
            MethodRecorderBaseTests.TestMethodRecorder testRecorder = new MethodRecorderBaseTests.TestMethodRecorder();
            using (new RecorderChanger(mocks, recorder, testRecorder))
            {
                Assert.Same(testRecorder, Get.Recorder(mocks));
            }
            Assert.NotSame(testRecorder, Get.Recorder(mocks));
            testRecorder.DoRecordCalled = false;
            recorder.Record(proxy, method, expectation);
            Assert.False(testRecorder.DoRecordCalled);

        }

        [Fact]
        public void ChangeRecorderTwice()
        {
            IMethodRecorder recorder = new UnorderedMethodRecorder(new ProxyMethodExpectationsDictionary());
            MethodRecorderBaseTests.TestMethodRecorder testRecorder = new MethodRecorderBaseTests.TestMethodRecorder();
            using (new RecorderChanger(mocks, recorder, testRecorder))
            {
                Assert.Same(testRecorder, Get.Recorder(mocks));
                MethodRecorderBaseTests.TestMethodRecorder testRecorder2 = new MethodRecorderBaseTests.TestMethodRecorder();
                using (new RecorderChanger(mocks, recorder, testRecorder2))
                {
                    Assert.Same(testRecorder2, Get.Recorder(mocks));
                    testRecorder2.DoRecordCalled = false;
                    recorder.Record(proxy, method, expectation);
                    Assert.True(testRecorder2.DoRecordCalled);
                }
                Assert.Same(testRecorder, Get.Recorder(mocks));
                testRecorder.DoRecordCalled = false;
                recorder.Record(proxy, method, expectation);
                Assert.True(testRecorder.DoRecordCalled);
            }
            Assert.NotSame(testRecorder, Get.Recorder(mocks));
            testRecorder.DoRecordCalled = false;
            recorder.Record(proxy, method, expectation);
            Assert.False(testRecorder.DoRecordCalled);
        }
    }
}