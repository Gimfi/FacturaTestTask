using Core.Installers;
using Zenject;

namespace Core.Asset
{
    public sealed class AssetPreloaderInstaller : BaseCoreInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            container.Bind<IAssetPreloader>().To<AssetPreloader>().AsSingle();
        }
    }
}