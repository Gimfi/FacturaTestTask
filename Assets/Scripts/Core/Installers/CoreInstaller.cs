using Core.Asset;
using Core.Launcher;
using Core.UI;
using Zenject;

namespace Core.Installers
{
    public sealed class CoreInstaller
    {
        private readonly DiContainer m_Container;

        public CoreInstaller(DiContainer container)
        {
            m_Container = container;
        }

        public void BindDependencies()
        {
            BindDependence<GameLauncherInstaller>();
            BindDependence<AssetPreloaderInstaller>();
            BindDependence<AssetProviderInstaller>();
            BindDependence<UISystemInstaller>();
        }

        private void BindDependence<T>() where T : BaseCoreInstaller, new()
        {
            BaseCoreInstaller installer = new T();
            installer.BindDependencies(m_Container);
        }
    }
}