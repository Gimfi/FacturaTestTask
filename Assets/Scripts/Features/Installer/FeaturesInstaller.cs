using Features.Enemies;
using Features.GamePrepare;
using Features.GameSession;
using Features.Road;
using Features.Vehicles;
using Features.Weapons;
using Zenject;

namespace Features.Installers
{
    public sealed class FeaturesInstaller
    {
        private readonly DiContainer m_Container;

        public FeaturesInstaller(DiContainer container)
        {
            m_Container = container;
        }

        public void BindDependencies()
        {
            BindDependence<GamePrepareInstaller>();
            BindDependence<RoadInstaller>();
            BindDependence<WeaponsInstaller>();
            BindDependence<VehiclesInstaller>();
            BindDependence<EnemiesInstaller>();
            BindDependence<GameSessionInstaller>();
        }

        private void BindDependence<T>() where T : BaseFeatureInstaller, new()
        {
            BaseFeatureInstaller installer = new T();
            installer.BindDependencies(m_Container);
        }
    }
}