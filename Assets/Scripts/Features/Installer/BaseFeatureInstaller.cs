using Zenject;

namespace Features.Installers
{
    public abstract class BaseFeatureInstaller
    {
        public abstract void BindDependencies(DiContainer container);
    }
}