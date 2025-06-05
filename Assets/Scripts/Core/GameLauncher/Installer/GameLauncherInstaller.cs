using Core.Installers;
using Zenject;

namespace Core.Launcher
{
    public sealed class GameLauncherInstaller : BaseCoreInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            container.BindInterfacesTo<GameLauncher>().AsSingle();
            container.BindExecutionOrder<GameLauncher>(int.MaxValue);
        }
    }
}