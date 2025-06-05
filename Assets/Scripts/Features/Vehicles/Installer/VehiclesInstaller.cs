using Features.Installers;
using Features.Vehicles.View;
using Zenject;

namespace Features.Vehicles
{
    public sealed class VehiclesInstaller : BaseFeatureInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            container.BindInterfacesTo<VehiclesService>().AsSingle();
            container.BindInterfacesAndSelfTo<VehiclesViewCreator>().AsSingle();
        }
    }
}