using System;
using System.Threading;

namespace Serilog.SharedLogContext
{
    internal class SharedLogPropsHolder : IDisposable
    {
        public static SharedLogContextProps Current => _currentSharedLogPropsHolder.Value?._sharedLogProps;

        private static readonly AsyncLocal<SharedLogPropsHolder> _currentSharedLogPropsHolder = new AsyncLocal<SharedLogPropsHolder>();

        private SharedLogContextProps _sharedLogProps;

        public static IDisposable Begin()
        {
            if (_currentSharedLogPropsHolder.Value != null)
                return _currentSharedLogPropsHolder.Value;

            var sharedLogPropsHolder = new SharedLogPropsHolder();
            sharedLogPropsHolder._sharedLogProps = new SharedLogContextProps();
            _currentSharedLogPropsHolder.Value = sharedLogPropsHolder;

            return sharedLogPropsHolder;
        }

        public void Dispose()
        {
            // check first if the call came from the same control flow (ambient context), then release the holder
            if (_currentSharedLogPropsHolder.Value == this)
            {
                _sharedLogProps = null;
                _currentSharedLogPropsHolder.Value = null;
            }
        }
    }
}