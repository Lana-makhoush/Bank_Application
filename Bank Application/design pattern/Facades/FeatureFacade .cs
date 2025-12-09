using Bank_Application.Models;
using Bank_Application.Services;

namespace Bank_Application.DesignPatterns
{
    public class FeatureFacade : IFeatureFacade
    {
        private readonly IFeatureService _featureService;

        public FeatureFacade(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        public async Task<Feature> CreateFeature(string name, string? description = null)
        {
            var feature = new Feature
            {
                FeatureName = name,
                Description = description
            };
            return await _featureService.AddFeature(feature);
        }

        public async Task<List<Feature>> ListAllFeatures()
        {
            return await _featureService.GetAllFeatures();
        }

        public async Task<bool> RemoveFeature(int featureId)
        {
            return await _featureService.DeleteFeature(featureId);
        }

        public async Task<bool> LinkFeatureToAccountType(int featureId, int accountTypeId)
        {
            return await _featureService.AssignFeatureToAccountType(featureId, accountTypeId);
        }

        public async Task<List<Feature>> ListFeaturesOfAccountType(int accountTypeId)
        {
            return await _featureService.GetFeaturesByAccountType(accountTypeId);
        }
    }
}
