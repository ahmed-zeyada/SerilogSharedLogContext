using System.Collections.ObjectModel;
using Serilog.SharedLogContext;

namespace SerilogExtensionDemo.Infrastructure
{
    /// <summary>
    /// adapter to hide the dependency on serilog for the business code
    /// </summary>
    public interface ISharedLogContextAdapter
    {
        ReadOnlyDictionary<string, object> ReadAll();
        void Add(string propertyName, object value);
        void Clear();
    }

    internal class SharedLogContextAdapter : ISharedLogContextAdapter
    {
        private readonly ISharedLogContext _sharedLogContext;

        public SharedLogContextAdapter(ISharedLogContext  sharedLogContext)
        {
            _sharedLogContext = sharedLogContext;
        }

        public void Add(string propertyName, object value)
        {
            _sharedLogContext.Set(propertyName, value);
        }

        public void Clear()
        {
            _sharedLogContext.Clear();
        }

        public ReadOnlyDictionary<string, object> ReadAll() => _sharedLogContext.GetProps();
    }
}