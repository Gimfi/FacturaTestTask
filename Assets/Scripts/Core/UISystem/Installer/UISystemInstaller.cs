using Core.Installers;
using Zenject;

namespace Core.UI
{
    public sealed class UISystemInstaller : BaseCoreInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            container.Bind<IUISystem>().To<UISystem>().AsSingle();
        }
    }
}