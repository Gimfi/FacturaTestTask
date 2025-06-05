using Features.GamePrepare.UI;
using Features.Installers;
using Zenject;

namespace Features.GamePrepare
{
    public sealed class GamePrepareInstaller : BaseFeatureInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            container.BindInterfacesTo<GamePrepareService>().AsSingle();
            container.BindInterfacesTo<GamePrepareUICreator>().AsSingle();
        }
    }
}