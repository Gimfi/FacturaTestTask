using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Enemies.View
{
    public sealed class EnemiesViewsPool : MonoBehaviour
    {
        [SerializeField]
        private int m_PoolSize;

        [SerializeField]
        private Transform m_PoolContainer;

        private EnemiesViewCreator m_EnemiesViewCreator;
        private EnemyView m_CurrentEnemyAsset;
        private IFactory<EnemyView, Transform, UniTask<EnemyView>> m_EnemyViewFactory;
        private readonly CompositeDisposable m_Disposable = new();

        private readonly List<EnemyView> m_InUseList = new();
        private readonly List<EnemyView> m_PoolList = new();

        [Inject]
        private void Construct(EnemiesViewCreator enemiesViewCreator,
            IFactory<EnemyView, Transform, UniTask<EnemyView>> enemyViewFactory)
        {
            m_EnemiesViewCreator = enemiesViewCreator;
            m_EnemyViewFactory = enemyViewFactory;
        }

        private void Awake()
        {
            m_EnemiesViewCreator.OnEnemyAssetLoaded.Subscribe(ProcessEnemyViewLoaded).AddTo(m_Disposable);
        }

        private void OnDestroy()
        {
            m_Disposable.Dispose();
        }

        private void ProcessEnemyViewLoaded(EnemyView segmentAsset)
        {
            m_CurrentEnemyAsset = segmentAsset;
            CreateFreeViews().Forget();
        }

        private async UniTask CreateFreeViews()
        {
            UniTask<EnemyView>[] tasks = new UniTask<EnemyView>[m_PoolSize];

            for (int i = 0; i < m_PoolSize; i++)
            {
                UniTask<EnemyView> task = m_EnemyViewFactory.Create(m_CurrentEnemyAsset, m_PoolContainer);
                tasks[i] = task;
            }

            EnemyView[] segments = await UniTask.WhenAll(tasks);
            m_PoolList.AddRange(segments);
        }

        public async UniTask<EnemyView> GeEnemyView()
        {
            EnemyView result;

            if (m_PoolList.Count > 0)
            {
                result = m_PoolList[0];
                m_PoolList.RemoveAt(0);
            }
            else
            {
                result = await m_EnemyViewFactory.Create(m_CurrentEnemyAsset, m_PoolContainer);
            }

            m_InUseList.Add(result);
            return result;
        }

        public void Release(EnemyView enemyView)
        {
            if (TryRemoveFromInUseList(enemyView))
                ReturnToPool(enemyView);
        }

        private bool TryRemoveFromInUseList(EnemyView enemyView)
        {
            if (m_InUseList.Contains(enemyView))
            {
                m_InUseList.Remove(enemyView);
                return true;
            }

            return false;
        }

        private void ReturnToPool(EnemyView enemyView)
        {
            enemyView.Release();
            enemyView.transform.SetParent(m_PoolContainer);
            m_PoolList.Add(enemyView);
        }
    }
}