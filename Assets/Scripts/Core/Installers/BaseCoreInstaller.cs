using Zenject;

namespace Core.Installers
{
    public abstract class BaseCoreInstaller
    {
        public abstract void BindDependencies(DiContainer container);
    }
}