using System;
using Core.Asset;
using Core.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.GamePrepare.UI
{
    public sealed class GamePrepareUICreator : IInitializable
    {
        private readonly IGamePrepareService m_GamePrepareService;
        private readonly IAssetProvider m_AssetProvider;
        private readonly IUISystem m_UISystem;

        private string m_CurrentLoaderGuid;
        private readonly CompositeDisposable m_Disposable = new(); //never Dispose because exemplar never destroyed.

        public GamePrepareUICreator(IGamePrepareService gamePrepareService, IAssetProvider assetProvider,
            IUISystem uiSystem)
        {
            m_GamePrepareService = gamePrepareService;
            m_AssetProvider = assetProvider;
            m_UISystem = uiSystem;
        }

        public void Initialize()
        {
            m_GamePrepareService.OnLoadStarted.Subscribe(_ => ProcessLoadStartedAsync().Forget()).AddTo(m_Disposable);
            m_GamePrepareService.OnLoadEnded.Subscribe(_ => ProcessLoadEnded()).AddTo(m_Disposable);
        }

        private async UniTask ProcessLoadStartedAsync()
        {
            try
            {
                GameObject go = await m_AssetProvider.GetAssetAsync<GameObject>(GamePrepareUIConstants.AssetName);
                if (go.TryGetComponent(out GamePrepareLoaderUI loaderView))
                {
                    m_CurrentLoaderGuid = loaderView.ElementGameObject.name;
                    m_UISystem.ShowSimpleUI(UISystemPlaces.FullScreenUnderUI, loaderView);
                }
                else
                {
                    Debug.LogError("[GamePrepareUICreator]: cannot find GamePrepareLoaderView component!!!");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void ProcessLoadEnded()
        {
            m_UISystem.HideSimpleUI(UISystemPlaces.FullScreenUnderUI, m_CurrentLoaderGuid);
            m_CurrentLoaderGuid = string.Empty;
        }
    }
}