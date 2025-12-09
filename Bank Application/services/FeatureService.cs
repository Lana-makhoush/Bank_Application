using Bank_Application.Models;
using Bank_Application.Repositories;

namespace Bank_Application.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository _featureRepository;

        public FeatureService(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        public Task<List<Feature>> GetAllFeatures() => _featureRepository.GetAllFeatures();

        public Task<Feature> AddFeature(Feature feature) => _featureRepository.AddFeature(feature);

        public Task<bool> DeleteFeature(int featureId) => _featureRepository.DeleteFeature(featureId);

        public Task<bool> AssignFeatureToAccountType(int featureId, int accountTypeId) =>
            _featureRepository.AssignFeatureToAccountType(featureId, accountTypeId);

        public Task<List<Feature>> GetFeaturesByAccountType(int accountTypeId) =>
            _featureRepository.GetFeaturesByAccountType(accountTypeId);
    }
}
