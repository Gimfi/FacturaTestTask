using System;
using Core.Asset;
using Core.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.GameSession.UI
{
    public sealed class GameSessionUICreator : IInitializable
    {
        private readonly IGameSessionService m_GameSessionService;
        private readonly IAssetProvider m_AssetProvider;
        private readonly IUISystem m_UISystem;
        private string m_CurrentResultUIGuid;

        public GameSessionUICreator(IGameSessionService gameSessionService, IAssetProvider assetProvider,
            IUISystem UISystem)
        {
            m_GameSessionService = gameSessionService;
            m_AssetProvider = assetProvider;
            m_UISystem = UISystem;
        }

        public void Initialize()
        {
            m_GameSessionService.OnGameEnded.Subscribe(_ => GameEnded());
            m_GameSessionService.OnGameReadyToStart.Subscribe(_ => HideResultPopup());
        }

        private void GameEnded()
        {
            ShowResultPopup().Forget();
        }

        private async UniTask ShowResultPopup()
        {
            try
            {
                GameObject go = await m_AssetProvider.GetAssetAsync<GameObject>(GameSessionUIConstants.AssetName);
                if (go.TryGetComponent(out GameSessionResultPopup loaderView))
                {
                    m_CurrentResultUIGuid = loaderView.ElementGameObject.name;
                    m_UISystem.ShowPopup(loaderView);
                }
                else
                {
                    Debug.LogError("[GameSessionUICreator]: cannot find GameSessionResultPopup component!!!");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void HideResultPopup()
        {
            if (!string.IsNullOrEmpty(m_CurrentResultUIGuid))
                m_UISystem.HidePopup(m_CurrentResultUIGuid);
        }
    }
}