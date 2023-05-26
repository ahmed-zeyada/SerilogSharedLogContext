namespace SerilogExtensionDemo.Infrastructure
{
    public interface IRepository
    {
        Task<List<string>> GetProducts(int catId);
    }

    internal class Repository : IRepository
    {
        private readonly ILogger _logger;
        private readonly ISharedLogContextAdapter _sharedLogContext;

        private List<string> _cachedProduct = new List<string>() { "prod1", "prod2" };

        public Repository(ILogger<Repository> logger, ISharedLogContextAdapter sharedLogContext)
        {
            _logger = logger;
            _sharedLogContext = sharedLogContext;
        }

        public async Task<List<string>> GetProducts(int catId)
        {
            _logger.LogInformation("calling database");

            await Task.Delay(1000);

            // database is down, response is read from cache, for a diagnostic reason current system needs to add a property to all incoming logs within the request with a specific id.

            _sharedLogContext.Add("CachedDataCollerationId", Guid.NewGuid().ToString());
            return _cachedProduct;
        }
    }
}