using Cysharp.Threading.Tasks;

namespace Core.Asset
{
    public interface IAssetPreloader
    {
        UniTask PreDownloadNecessaryAssets();
    }
}