using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.Weapons.View
{
    public class WeaponsViewInstaller : MonoInstaller, IInitializable
    {
        [SerializeField]
        private ProjectileViewsPool m_ProjectileViewsPool;

        public override void InstallBindings()
        {
            Container.Bind<ProjectileViewsPool>().FromInstance(m_ProjectileViewsPool).AsSingle();
            Container.Bind<IFactory<ProjectileView, Transform, UniTask<ProjectileView>>>().To<ProjectileViewFactory>()
                .AsSingle();
        }

        public void Initialize()
        {
            Container.Inject(m_ProjectileViewsPool);
        }
    }
}