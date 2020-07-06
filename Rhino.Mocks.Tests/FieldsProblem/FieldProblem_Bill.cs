﻿using Microsoft.Practices.Unity;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{

    public class FieldProblem_Bill
    {
        /// <summary>
        /// From thread:
        /// http://groups.google.com/group/rhinomocks/browse_thread/thread/a22b18618be887ff?hl=en
        /// </summary>
        [Fact]
        public void Should_be_able_to_proxy_IUnityContainer()
        {
            var unity = MockRepository.GenerateMock<IUnityContainer>();
        }
    }
}
