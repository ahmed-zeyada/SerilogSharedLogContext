using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Serilog.SharedLogContext
{
    public interface ISharedLogContext
    {
        ReadOnlyDictionary<string, object> GetProps();
        void Set(string propertyName, object value);
        void Clear();
    }

    internal interface ISharedLogContextInitializer
    {
        IDisposable BeginContext();
    }

    internal class SharedLogContext : ISharedLogContext, ISharedLogContextInitializer
    {
        public void Set(string propertyName, object value)
        {
            SharedLogPropsHolder.Current?.AddOrUpdate(propertyName, value, (_, __) => value);
        }

        public void Clear()
        {
            SharedLogPropsHolder.Current?.Clear();
        }

        IDisposable ISharedLogContextInitializer.BeginContext()
        {
            return SharedLogPropsHolder.Begin();
        }

        ReadOnlyDictionary<string, object> ISharedLogContext.GetProps()
        {
            return new ReadOnlyDictionary<string, object>(SharedLogPropsHolder.Current?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<string, object>());
        }
    }
}