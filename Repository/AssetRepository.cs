using AMSWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMSWebApi.Repository
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AssetMsDbContext _context;

        // Constructor to inject the database context
        public AssetRepository(AssetMsDbContext context)
        {
            _context = context;
        }

        // Create a new asset record
        public async Task<AssetMaster> CreateAssetAsync(AssetMaster asset)
        {
            if (_context != null)
            {
                await _context.AssetMasters.AddAsync(asset);
                await _context.SaveChangesAsync();
                return asset;
            }
            return null;
        }

        // Get an asset by its ID
        public async Task<AssetMaster> GetAssetByIdAsync(int assetId)
        {
            if (_context != null)
            {
                return await _context.AssetMasters
                    .Include(a => a.AmAtype)
                    .Include(a => a.AmMake)
                    .Include(a => a.AmAd)
                    .FirstOrDefaultAsync(a => a.AmId == assetId);
            }
            return null;
        }

        // Get all assets
        public async Task<IEnumerable<AssetMaster>> GetAllAssetsAsync()
        {
            if (_context != null)
            {
                return await _context.AssetMasters
                    .Include(a => a.AmAtype)
                    .Include(a => a.AmMake)
                    .Include(a => a.AmAd)
                    .ToListAsync();
            }
            return null;
        }

        // Update an existing asset
        public async Task<bool> UpdateAssetAsync(AssetMaster asset)
        {
            if (_context != null)
            {
                var existingAsset = await _context.AssetMasters.FirstOrDefaultAsync(a => a.AmId == asset.AmId);
                if (existingAsset != null)
                {
                    existingAsset.AmModel = asset.AmModel;
                    existingAsset.AmSnnumber = asset.AmSnnumber;
                    existingAsset.AmMyyear = asset.AmMyyear;
                    existingAsset.AmPdate = asset.AmPdate;
                    existingAsset.AmWarranty = asset.AmWarranty;
                    existingAsset.AmFromDate = asset.AmFromDate;
                    existingAsset.AmToDate = asset.AmToDate;
                    existingAsset.AmAtypeid = asset.AmAtypeid;
                    existingAsset.AmMakeid = asset.AmMakeid;
                    existingAsset.AmAdId = asset.AmAdId;

                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        // Search for assets based on specific criteria (model name)
        public async Task<IEnumerable<AssetMaster>> SearchAssetsAsync(string searchTerm)
        {
            if (_context != null)
            {
                return await _context.AssetMasters
                    .Where(a => a.AmModel.Contains(searchTerm) || a.AmSnnumber.Contains(searchTerm))
                    .ToListAsync();
            }
            return null;
        }
    }
}
