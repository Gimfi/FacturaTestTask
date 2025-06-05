using UnityEngine;
using Zenject;

namespace Features.GameCamera.View
{
    public class GameCameraViewInstaller : MonoInstaller, IInitializable
    {
        [SerializeField]
        private GameCameraContainerView m_GameCameraContainerView;

        public override void InstallBindings()
        {
            Container.Bind<GameCameraContainerView>().FromInstance(m_GameCameraContainerView).AsSingle();
        }

        public void Initialize()
        {
            Container.Inject(m_GameCameraContainerView);
        }
    }
}