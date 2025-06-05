using Core.Installers;
using Zenject;

namespace Core.Asset
{
    public sealed class AssetProviderInstaller : BaseCoreInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
        }
    }
}