using System.Collections.Concurrent;

namespace Serilog.SharedLogContext
{
    internal class SharedLogContextProps : ConcurrentDictionary<string, object>
    {

    }
}