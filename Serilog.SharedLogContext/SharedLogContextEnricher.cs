using System.Linq;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.SharedLogContext
{
    internal class SharedLogContextEnricher : ILogEventEnricher
    {
        private readonly ISharedLogContext _sharedLogContext;

        public SharedLogContextEnricher(ISharedLogContext sharedLogContext)
        {
            _sharedLogContext = sharedLogContext;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            _sharedLogContext.GetProps().ToList().ForEach(pair =>
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(pair.Key, pair.Value));
            });
        }
    }
}