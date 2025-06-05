using Core.Installers;
using Features.Installers;
using UnityEngine;
using Zenject;

namespace Project
{
    [CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
    public sealed class ProjectInstaller : ScriptableObjectInstaller
    {
        private CoreInstaller m_CoreInstaller;
        private FeaturesInstaller m_FeaturesInstaller;

        public override void InstallBindings()
        {
            CreateInstallers();
            BindDependencies();
        }

        private void CreateInstallers()
        {
            m_CoreInstaller = new CoreInstaller(Container);
            m_FeaturesInstaller = new FeaturesInstaller(Container);
        }

        private void BindDependencies()
        {
            m_CoreInstaller.BindDependencies();
            m_FeaturesInstaller.BindDependencies();
        }
    }
}