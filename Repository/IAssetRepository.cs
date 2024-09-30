using AMSWebApi.Models;

namespace AMSWebApi.Repository
{
    public interface IAssetRepository
    {
        // Create a new asset record
        Task<AssetMaster> CreateAssetAsync(AssetMaster asset);

        // Read (Get) an asset by its ID
        Task<AssetMaster> GetAssetByIdAsync(int assetId);

        // Read (Get) all assets
        Task<IEnumerable<AssetMaster>> GetAllAssetsAsync();

        // Update an existing asset
        Task<bool> UpdateAssetAsync(AssetMaster asset);

        // Search for assets based on specific criteria (e.g., model name)
        Task<IEnumerable<AssetMaster>> SearchAssetsAsync(string searchTerm);

    }
}

