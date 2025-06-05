using System;
using UniRx;

namespace Features.GameSession
{
    public interface IGameSessionService
    {
        bool LastGameResult { get; }
        IObservable<Unit> OnGameReadyToStart { get; }
        IObservable<Unit> OnGameStarted { get; }
        IObservable<Unit> OnGameEnded { get; }

        void LaunchGameSession();
        void ResetGame();
    }
}