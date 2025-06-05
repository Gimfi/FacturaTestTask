using Cysharp.Threading.Tasks;

namespace Core.Asset
{
    public interface IAssetProvider
    {
        UniTask<T> GetAssetAsync<T>(string assetAddressableName);
    }
}