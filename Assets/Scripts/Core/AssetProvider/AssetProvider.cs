using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Asset
{
    public sealed class AssetProvider : IAssetProvider
    {
        public async UniTask<T> GetAssetAsync<T>(string assetAddressableName)
        {
            T result = default;
            AsyncOperationHandle<T> handle = default;

            try
            {
                handle = Addressables.LoadAssetAsync<T>(assetAddressableName);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    result = handle.Result;
                }
                else
                {
                    Debug.LogException(handle.OperationException);
                    Addressables.Release(handle);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                Addressables.Release(handle);
            }

            return result;
        }
    }
}