using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.Vehicles.View
{
    public class VehiclesViewInstaller : MonoInstaller, IInitializable
    {
        [SerializeField]
        private VehicleContainerView m_VehicleContainerView;

        public override void InstallBindings()
        {
            Container.Bind<VehicleContainerView>().FromInstance(m_VehicleContainerView).AsSingle();
            Container.Bind<IFactory<VehicleView, Transform, UniTask<VehicleView>>>().To<VehiclesSegmentFactory>()
                .AsSingle();
        }

        public void Initialize()
        {
            Container.Inject(m_VehicleContainerView);
        }
    }
}