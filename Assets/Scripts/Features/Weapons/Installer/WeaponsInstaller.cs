using Features.Installers;
using Zenject;

namespace Features.Weapons
{
    public sealed class WeaponsInstaller : BaseFeatureInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            container.BindInterfacesTo<WeaponsService>().AsSingle();
        }
    }
}