using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_Application.Repositories
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly AppDbContext _context;

        public FeatureRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Feature>> GetAllFeatures()
        {
            return await _context.Features.Include(f => f.AccountTypeFeatures).ToListAsync();
        }

        public async Task<Feature> AddFeature(Feature feature)
        {
            _context.Features.Add(feature);
            await _context.SaveChangesAsync();
            return feature;
        }

        public async Task<bool> DeleteFeature(int featureId)
        {
            var feature = await _context.Features
                .Include(f => f.AccountTypeFeatures)
                .FirstOrDefaultAsync(f => f.FeatureId == featureId);

            if (feature == null)
                return false;

            if (feature.AccountTypeFeatures != null && feature.AccountTypeFeatures.Any())
                return false;

            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AssignFeatureToAccountType(int featureId, int accountTypeId)
        {
            var exists = await _context.AccountTypeFeatures
                .AnyAsync(atf => atf.AccountTypeId == accountTypeId && atf.FeatureId == featureId);

            if (exists) return false;

            _context.AccountTypeFeatures.Add(new AccountTypeFeature
            {
                AccountTypeId = accountTypeId,
                FeatureId = featureId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Feature>> GetFeaturesByAccountType(int accountTypeId)
        {
            return await _context.AccountTypeFeatures
                .Where(atf => atf.AccountTypeId == accountTypeId)
                .Select(atf => atf.Feature!)
                .ToListAsync();
        }
    }
}
