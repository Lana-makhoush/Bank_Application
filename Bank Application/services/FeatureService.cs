using Bank_Application.Models;
using Bank_Application.Repositories;

namespace Bank_Application.DesignPatterns.Decorator
{
    public class FeatureService : IFeatureDecorator
    {
        private readonly IFeatureRepository _repo;

        public FeatureService(IFeatureRepository repo)
        {
            _repo = repo;
        }

        public Task<Feature> CreateFeature(string name, string? description = null, decimal cost = 0)
        {
            var feature = new Feature
            {
                FeatureName = name,
                Description = description,
                Cost = cost 
            };

            return _repo.AddFeature(feature);
        }


        public Task<List<Feature>> ListAllFeatures()
            => _repo.GetAllFeatures();

        public Task<bool> RemoveFeature(int featureId)
            => _repo.DeleteFeature(featureId);

        public Task<bool> LinkFeatureToAccountType(int featureId, int accountTypeId)
            => _repo.AssignFeatureToAccountType(featureId, accountTypeId);

        public Task<List<Feature>> ListFeaturesOfAccountType(int accountTypeId)
            => _repo.GetFeaturesByAccountType(accountTypeId);
    }
}
