using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.Enemies.View
{
    public class EnemiesViewInstaller : MonoInstaller, IInitializable
    {
        [SerializeField]
        private EnemiesContainerView m_EnemiesContainerView;

        [SerializeField]
        private EnemiesViewsPool m_EnemiesViewsPool;

        public override void InstallBindings()
        {
            Container.Bind<EnemiesContainerView>().FromInstance(m_EnemiesContainerView).AsSingle();
            Container.Bind<EnemiesViewsPool>().FromInstance(m_EnemiesViewsPool).AsSingle();
            Container.Bind<IFactory<EnemyView, Transform, UniTask<EnemyView>>>().To<EnemiesViewFactory>()
                .AsSingle();
        }

        public void Initialize()
        {
            Container.Inject(m_EnemiesContainerView);
            Container.Inject(m_EnemiesViewsPool);
        }
    }
}