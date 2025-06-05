using Cysharp.Threading.Tasks;

namespace Core.Asset
{
    public sealed class AssetPreloader : IAssetPreloader
    {
        public async UniTask PreDownloadNecessaryAssets()
        {
            await UniTask.Delay(2000); //Pre-download the necessary assets here! (Delay just an example)
        }
    }
}