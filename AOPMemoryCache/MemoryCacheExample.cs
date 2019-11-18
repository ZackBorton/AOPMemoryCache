using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace AOPMemoryCache
{
    /// <summary>
    ///     Intercepts
    ///     AOP allows you to interject code into the standard operation flow of your code
    ///     without the need to become dependent on the code itself.
    /// </summary>
    public class MemoryCacheExample : IInterceptor
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
​
        /// <summary>
        ///      Intercept a method or constructor instantiation and cache a result
        /// </summary>
        /// <Remark>
        ///     An invocation is a joint point and can be intercepted by an interceptor
        ///     This allows you to intercept something such as a method call and perform
        ///     two control flow paths such as logging or in this case caching
        /// </Remark>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            var name = $"{invocation.Method.DeclaringType}_{invocation.Method.Name}";
            var args = string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()));
            var cacheKey = $"{name}|{args}";
​
            _cache.TryGetValue(cacheKey, out object returnValue);
            if (returnValue == null)
            {
                invocation.Proceed();
                returnValue = invocation.ReturnValue;
                _cache.Add(cacheKey, returnValue);
            }
            else
            {
                invocation.ReturnValue = returnValue;
            }
        }
    }
}