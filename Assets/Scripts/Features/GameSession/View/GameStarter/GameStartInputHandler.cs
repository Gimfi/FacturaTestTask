using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.GameSession.View
{
    public sealed class GameStartInputHandler : MonoBehaviour
    {
        private IGameSessionService m_GameSessionService;
        private readonly CompositeDisposable m_Disposable = new();

        [Inject]
        public void Construct(IGameSessionService gameSessionService)
        {
            m_GameSessionService = gameSessionService;
        }

        private void Awake()
        {
            m_GameSessionService.OnGameReadyToStart.Subscribe(_ => AwaitStartClick().Forget()).AddTo(m_Disposable);
            m_GameSessionService.OnGameEnded.Subscribe(_ => AwaitRelaunchClick().Forget()).AddTo(m_Disposable);
        }

        private void OnDestroy()
        {
            m_Disposable.Dispose();
        }

        private async UniTask AwaitRelaunchClick()
        {
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(1));
            m_GameSessionService.ResetGame();
        }

        private async UniTask AwaitStartClick()
        {
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(1));
            m_GameSessionService.LaunchGameSession();
        }
    }
}