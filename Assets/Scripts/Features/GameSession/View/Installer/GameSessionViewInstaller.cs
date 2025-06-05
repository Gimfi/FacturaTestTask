using UnityEngine;
using Zenject;

namespace Features.GameSession.View
{
    public class GameSessionViewInstaller : MonoInstaller, IInitializable
    {
        [SerializeField]
        private GameSessionView m_GameSessionView;

        [SerializeField]
        private GameStartInputHandler m_GameStartInputHandler;

        public override void InstallBindings()
        {
            Container.Bind<GameSessionView>().FromInstance(m_GameSessionView).AsSingle();
        }

        public void Initialize()
        {
            Container.Inject(m_GameSessionView);
            Container.Inject(m_GameStartInputHandler);
        }
    }
}