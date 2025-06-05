using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class UIRootInstaller : MonoInstaller, IInitializable
    {
        [SerializeField]
        private UIRoot m_UIRoot;

        public override void InstallBindings()
        {
            Container.Bind<IUIRoot>().FromInstance(m_UIRoot).AsSingle();
        }

        public void Initialize()
        {
            Container.Inject(m_UIRoot);
        }
    }
}