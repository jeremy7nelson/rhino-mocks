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


using System;
using Xunit;

namespace Rhino.Mocks.Tests
{

    public class DotNet2Tests : IDisposable
    {
        MockRepository mocks;
        IDotNet2Features demo;
        public DotNet2Tests()
        {
            mocks = new MockRepository();
            demo = mocks.DynamicMock<IDotNet2Features>();
        }

        public void Dispose()
        {
            mocks.VerifyAll();
        }

        [Fact]
        public void DefaultValueOfNullableIsNull()
        {
            mocks.ReplayAll();
            Assert.Null(demo.NullableInt(3));
        }

        [Fact]
        public void CanUseNullAsReturnValueForNullables()
        {
            Expect.Call(demo.NullableInt(5)).Return(null);
            mocks.ReplayAll();
            Assert.Null(demo.NullableInt(5));
        }

        [Fact]
        public void CanPassNonNullableValues()
        {
            Expect.Call(demo.NullableInt(53)).Return(5);
            mocks.ReplayAll();
            Assert.Equal(5, demo.NullableInt(53));
        }

        [Fact]
        public void CanStrictMockOnClassWithInternalMethod()
        {
            WithInternalMethod withInternalMethod = mocks.StrictMock<WithInternalMethod>();
            withInternalMethod.Foo();
            LastCall.Throw(new Exception("foo"));
            mocks.ReplayAll();
            try
            {
                withInternalMethod.Foo();
                Assert.False(true, "Should have thrown");
            }
            catch (Exception e)
            {
                Assert.Equal("foo", e.Message);
            }
        }

        internal interface IDotNet2Features
        {
            int? NullableInt(int i);
        }

        public class WithInternalMethod
        {
            internal virtual void Foo()
            {
            }
        }
    }
}
