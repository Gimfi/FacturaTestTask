using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.Weapons.View
{
    public sealed class ProjectileViewsPool : MonoBehaviour
    {
        [SerializeField]
        private int m_PoolSize;

        [SerializeField]
        private Transform m_PoolContainer;

        [SerializeField]
        private ProjectileView m_ProjectileViewAsset;

        private IFactory<ProjectileView, Transform, UniTask<ProjectileView>> m_ProjectileViewFactory;

        private readonly List<ProjectileView> m_InUseList = new();
        private readonly List<ProjectileView> m_PoolList = new();

        [Inject]
        private void Construct(IFactory<ProjectileView, Transform, UniTask<ProjectileView>> enemyViewFactory)
        {
            m_ProjectileViewFactory = enemyViewFactory;
        }

        private void Start()
        {
            CreateFreeViews().Forget();
        }

        private async UniTask CreateFreeViews()
        {
            UniTask<ProjectileView>[] tasks = new UniTask<ProjectileView>[m_PoolSize];

            for (int i = 0; i < m_PoolSize; i++)
            {
                UniTask<ProjectileView> task = m_ProjectileViewFactory.Create(m_ProjectileViewAsset, m_PoolContainer);
                tasks[i] = task;
            }

            ProjectileView[] projectiles = await UniTask.WhenAll(tasks);
            m_PoolList.AddRange(projectiles);
        }

        public async UniTask<ProjectileView> GetProjectileView()
        {
            ProjectileView result;

            if (m_PoolList.Count > 0)
            {
                result = m_PoolList[0];
                m_PoolList.RemoveAt(0);
            }
            else
            {
                result = await m_ProjectileViewFactory.Create(m_ProjectileViewAsset, m_PoolContainer);
            }

            result.Prepare();
            m_InUseList.Add(result);
            return result;
        }

        public void Release(ProjectileView projectileView)
        {
            if (TryRemoveFromInUseList(projectileView))
                ReturnToPool(projectileView);
        }

        private bool TryRemoveFromInUseList(ProjectileView projectileView)
        {
            if (m_InUseList.Contains(projectileView))
            {
                m_InUseList.Remove(projectileView);
                return true;
            }

            return false;
        }

        private void ReturnToPool(ProjectileView projectileView)
        {
            projectileView.Release();
            projectileView.transform.SetParent(m_PoolContainer);
            m_PoolList.Add(projectileView);
        }
    }
}