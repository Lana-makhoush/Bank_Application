using Bank_Application.Models;

namespace Bank_Application.DesignPatterns.Decorator
{
    public interface IFeatureDecorator
    {
        Task<Feature> CreateFeature(string name, string? description = null);
        Task<List<Feature>> ListAllFeatures();
        Task<bool> RemoveFeature(int featureId);
        Task<bool> LinkFeatureToAccountType(int featureId, int accountTypeId);
        Task<List<Feature>> ListFeaturesOfAccountType(int accountTypeId);
    }
}
