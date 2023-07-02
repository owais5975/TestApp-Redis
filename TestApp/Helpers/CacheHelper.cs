using Microsoft.Extensions.Caching.Distributed;

namespace TestApp.Helpers
{
    public class CacheHelper
    {
        public static DistributedCacheEntryOptions CacheOptions()
        {
            var distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5*60);
            distributedCacheEntryOptions.SlidingExpiration = null;

            return distributedCacheEntryOptions;
        }
    }
}
