using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Enemies.View
{
    public sealed class EnemiesContainerView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_EnemyContainer;

        [SerializeField]
        private EnemiesViewsPool m_Pool;

        private IEnemiesService m_EnemiesService;
        private EnemiesViewCreator m_EnemiesViewCreator;

        private int m_LastSegmentIndex;
        private readonly CompositeDisposable m_Disposable = new();

        private readonly List<EnemyView> m_EnemiesViews = new();

        [Inject]
        private void Construct(IEnemiesService enemiesService, EnemiesViewCreator enemiesViewCreator)
        {
            m_EnemiesService = enemiesService;
            m_EnemiesViewCreator = enemiesViewCreator;
        }

        private void Awake()
        {
            m_EnemiesViewCreator.OnEnemiesCreateRequest.Subscribe(ProcessEnemiesCreate).AddTo(m_Disposable);
            m_EnemiesService.OnEnemiesCreateRequest.Subscribe(ProcessEnemiesCreate).AddTo(m_Disposable);
            m_EnemiesService.OnEnemiesDestroyRequest.Subscribe(ProcessEnemiesDestroy).AddTo(m_Disposable);
        }

        private void OnDestroy()
        {
            m_Disposable.Dispose();
        }

        private void ProcessEnemiesCreate(List<EnemyModel> enemyModels)
        {
            CreateStartEnemies(enemyModels).Forget();
        }

        private async UniTask CreateStartEnemies(List<EnemyModel> enemyModels)
        {
            UniTask[] tasks = new UniTask[enemyModels.Count];

            for (int i = 0; i < enemyModels.Count; i++)
            {
                EnemyModel model = enemyModels[i];
                UniTask task = CreateEnemy(model);
                tasks[i] = task;
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask CreateEnemy(EnemyModel model)
        {
            EnemyView segmentView = await m_Pool.GeEnemyView();
            segmentView.transform.SetParent(m_EnemyContainer);
            segmentView.ReInit(model);

            m_EnemiesViews.Add(segmentView);
        }

        private void ProcessEnemiesDestroy(List<EnemyModel> enemyModels)
        {
            for (int i = 0; i < enemyModels.Count; i++)
            {
                EnemyModel enemyModel = enemyModels[i];

                for (int j = 0; j < m_EnemiesViews.Count; j++)
                {
                    EnemyView view = m_EnemiesViews[j];

                    if (view.Model == enemyModel)
                        m_Pool.Release(view);
                }
            }
        }
    }
}