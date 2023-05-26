using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Configuration;

namespace Serilog.SharedLogContext
{
    public static class SharedLogContextConfExtensions
    {
        public static LoggerConfiguration FromSharedLogContext(this LoggerEnrichmentConfiguration enrichmentConfiguration, IServiceProvider serviceProvider)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            }

            var sharedLogContext = serviceProvider.GetService<ISharedLogContext>();

            if (sharedLogContext is null)
            {
                throw new Exception($"shared log context services are not registered, use {nameof(AddSharedLogContext)} method to register it");
            }

            return enrichmentConfiguration.With(new SharedLogContextEnricher(sharedLogContext));
        }

        public static IServiceCollection AddSharedLogContext(this IServiceCollection services)
        {
            services.AddSingleton<ISharedLogContext, SharedLogContext>();
            services.AddSingleton(sp => sp.GetService<ISharedLogContext>() as ISharedLogContextInitializer);
            return services;
        }

        public static IApplicationBuilder UseSharedLogContext(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<SharedLogContextMiddleware>();
        }
    }
}