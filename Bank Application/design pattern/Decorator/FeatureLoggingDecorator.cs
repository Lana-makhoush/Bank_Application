using Bank_Application.Models;
using Microsoft.Extensions.Logging;

namespace Bank_Application.DesignPatterns.Decorator
{
    public class FeatureLoggingDecorator : FeatureDecorator
    {
        private readonly ILogger<FeatureLoggingDecorator> _logger;

        public FeatureLoggingDecorator(
            IFeatureDecorator inner,
            ILogger<FeatureLoggingDecorator> logger
        ) : base(inner)
        {
            _logger = logger;
        }

        public override async Task<Feature> CreateFeature(string name, string? description = null, decimal cost = 0)
        {
            _logger.LogInformation($"Creating feature: {name} with cost: {cost}");
            var result = await base.CreateFeature(name, description, cost);
            _logger.LogInformation($"Created feature ID = {result.FeatureId}");
            return result;
        }

    }
}
