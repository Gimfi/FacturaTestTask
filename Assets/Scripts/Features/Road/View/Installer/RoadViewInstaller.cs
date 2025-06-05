using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.Road.View
{
    public class RoadViewInstaller : MonoInstaller, IInitializable
    {
        [SerializeField]
        private RoadContainerView m_RoadContainerView;

        [SerializeField]
        private RoadSegmentViewsPool m_RoadSegmentViewsPool;

        public override void InstallBindings()
        {
            Container.Bind<RoadContainerView>().FromInstance(m_RoadContainerView).AsSingle();
            Container.Bind<RoadSegmentViewsPool>().FromInstance(m_RoadSegmentViewsPool).AsSingle();
            Container.Bind<IFactory<RoadSegmentView, Transform, UniTask<RoadSegmentView>>>().To<RoadSegmentFactory>()
                .AsSingle();
        }

        public void Initialize()
        {
            Container.Inject(m_RoadContainerView);
            Container.Inject(m_RoadSegmentViewsPool);
        }
    }
}