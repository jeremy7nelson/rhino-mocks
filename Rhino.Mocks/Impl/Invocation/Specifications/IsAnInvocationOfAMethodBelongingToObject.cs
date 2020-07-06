using Castle.Core.Interceptor;
using Rhino.Mocks.Impl.InvocationSpecifications;
using System;
using System.Reflection;

namespace Rhino.Mocks.Impl.Invocation.Specifications
{
    ///<summary>
    ///</summary>
    public class IsAnInvocationOfAMethodBelongingToObject : ISpecification<IInvocation>
    {
        private static MethodInfo[] objectMethods =
            new MethodInfo[]
            {
                typeof (object).GetMethod("ToString"), typeof (object).GetMethod("Equals", new Type[] {typeof (object)}),
                typeof (object).GetMethod("GetHashCode"), typeof (object).GetMethod("GetType")
            };

        ///<summary>
        ///</summary>
        public bool IsSatisfiedBy(IInvocation item)
        {
            return Array.IndexOf(objectMethods, item.Method) != -1;
        }
    }
}