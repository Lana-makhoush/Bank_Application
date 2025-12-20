using Bank_Application.Models;

namespace Bank_Application.DesignPatterns.Decorator
{
    public abstract class FeatureDecorator : IFeatureDecorator
    {
        protected readonly IFeatureDecorator _inner;

        protected FeatureDecorator(IFeatureDecorator inner)
        {
            _inner = inner;
        }

        public virtual Task<Feature> CreateFeature(string name, string? description = null, decimal cost = 0)
        {
            return _inner.CreateFeature(name, description, cost);
        }

        public virtual Task<List<Feature>> ListAllFeatures()
            => _inner.ListAllFeatures();

        public virtual Task<bool> RemoveFeature(int featureId)
            => _inner.RemoveFeature(featureId);

        public virtual Task<bool> LinkFeatureToAccountType(int featureId, int accountTypeId)
            => _inner.LinkFeatureToAccountType(featureId, accountTypeId);

        public virtual Task<List<Feature>> ListFeaturesOfAccountType(int accountTypeId)
            => _inner.ListFeaturesOfAccountType(accountTypeId);
    }
}
