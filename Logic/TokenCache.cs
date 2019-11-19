using System;
using System.Linq;
using System.Security.Claims;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;

namespace Logic
{
    /// <summary>
    ///     Intercepts
    ///     AOP allows you to interject code into the standard operation flow of your code
    ///     without the need to become dependent on the code itself.
    /// </summary>
    public class TokenCache : IInterceptor
    {
        private readonly IMemoryCache _memoryCache;
        private ClaimsPrincipal _claimsPrincipal;
        
        public TokenCache(IMemoryCache memoryCache, ClaimsPrincipal claimsPrincipal)
        {
            _memoryCache = memoryCache;
            _claimsPrincipal = claimsPrincipal;
        }

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
            var principalHash = _claimsPrincipal.Claims.Select(x => x.Type == ClaimTypes.Hash);

            // Look for cache key.
            if (!_memoryCache.TryGetValue(principalHash, out _claimsPrincipal))
            {
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                // Save data in cache
                _memoryCache.Set(principalHash, _claimsPrincipal, cacheEntryOptions);
            }
        }
    }
}