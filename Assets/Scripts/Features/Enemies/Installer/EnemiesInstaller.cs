using Features.Enemies.View;
using Features.Installers;
using Zenject;

namespace Features.Enemies
{
    public sealed class EnemiesInstaller : BaseFeatureInstaller
    {
        public override void BindDependencies(DiContainer container)
        {
            //
            container.BindInterfacesTo<EnemiesFactory>().AsSingle();
            container.BindInterfacesTo<EnemiesInitializer>().AsSingle();
            container.BindInterfacesTo<EnemyCreator>().AsSingle();
            //

            container.BindInterfacesTo<EnemiesValidator>().AsSingle();
            container.BindInterfacesTo<EnemiesHpObserver>().AsSingle();
            container.BindInterfacesTo<EnemiesAttackBehavior>().AsSingle();
            container.BindInterfacesTo<EnemiesService>().AsSingle();

            container.BindInterfacesAndSelfTo<EnemiesViewCreator>().AsSingle();
        }
    }
}