using System;
using System.Collections.Generic;
using Core.Asset;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Enemies.View
{
    public sealed class EnemiesViewCreator : IInitializable
    {
        private readonly IEnemiesService m_Service;
        private readonly IAssetProvider m_AssetProvider;
        private readonly CompositeDisposable m_Disposable = new(); //never Dispose because exemplar never destroyed.

        private readonly Subject<EnemyView> m_OnEnemyAssetLoaded = new();
        private readonly Subject<List<EnemyModel>> m_OnEnemiesCreateRequest = new();

        public EnemiesViewCreator(IEnemiesService service, IAssetProvider assetProvider)
        {
            m_Service = service;
            m_AssetProvider = assetProvider;
        }

        public IObservable<EnemyView> OnEnemyAssetLoaded => m_OnEnemyAssetLoaded;
        public IObservable<List<EnemyModel>> OnEnemiesCreateRequest => m_OnEnemiesCreateRequest;

        public void Initialize()
        {
            m_Service.OnEnemiesInitiateRequest.Subscribe(data => CreateEnemies(data).Forget()).AddTo(m_Disposable);
        }

        private async UniTask CreateEnemies(List<EnemyModel> enemies)
        {
            try
            {
                GameObject go = await m_AssetProvider.GetAssetAsync<GameObject>(EnemiesViewConstants.AssetName);
                if (go.TryGetComponent(out EnemyView enemyView))
                {
                    m_OnEnemyAssetLoaded.OnNext(enemyView);
                    m_OnEnemiesCreateRequest.OnNext(enemies);
                }
                else
                {
                    Debug.LogError("[EnemiesViewCreator]: cannot find EnemyView component!!!");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}