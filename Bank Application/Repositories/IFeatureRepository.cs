using Bank_Application.Models;

namespace Bank_Application.Repositories
{
    public interface IFeatureRepository
    {
        Task<List<Feature>> GetAllFeatures();
        Task<Feature> AddFeature(Feature feature);
        Task<bool> DeleteFeature(int featureId);

        Task<bool> AssignFeatureToAccountType(int featureId, int accountTypeId);
        Task<List<Feature>> GetFeaturesByAccountType(int accountTypeId);
    }
}
