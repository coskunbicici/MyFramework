using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            // IMemoryCache karşılığı (Core.CCC.Caching.Microsoft)
            // Redis'e çevirirsek buna gerek kalmaz, silmemiz gerekir
            serviceCollection.AddMemoryCache();
            // Hangi CacheManager ile çalışacağımızı belirliyoruz
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
            
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            serviceCollection.AddSingleton<Stopwatch>();
        }
    }
}
