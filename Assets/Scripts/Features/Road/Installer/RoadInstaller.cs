using Features.Installers;
using Features.Road.View;
using Zenject;

namespace Features.Road
{
    public sealed class RoadInstaller : BaseFeatureInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            container.BindInterfacesTo<RoadService>().AsSingle();
            container.BindInterfacesAndSelfTo<RoadViewCreator>().AsSingle();
        }
    }
}