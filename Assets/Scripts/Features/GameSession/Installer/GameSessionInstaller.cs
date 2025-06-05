using Features.GameSession.UI;
using Features.Installers;
using Zenject;

namespace Features.GameSession
{
    public sealed class GameSessionInstaller : BaseFeatureInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            container.BindInterfacesTo<GameSessionVehicleMover>().AsSingle();
            container.BindInterfacesTo<GameCompletionController>().AsSingle();
            container.BindInterfacesTo<GameSessionService>().AsSingle();

            container.BindInterfacesTo<GameSessionUICreator>().AsSingle();
        }
    }
}