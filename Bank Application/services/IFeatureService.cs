using Bank_Application.Models;

namespace Bank_Application.Services
{
    public interface IFeatureService
    {
        Task<List<Feature>> GetAllFeatures();
        Task<Feature> AddFeature(Feature feature);
        Task<bool> DeleteFeature(int featureId);

        Task<bool> AssignFeatureToAccountType(int featureId, int accountTypeId);
        Task<List<Feature>> GetFeaturesByAccountType(int accountTypeId);
    }
}
