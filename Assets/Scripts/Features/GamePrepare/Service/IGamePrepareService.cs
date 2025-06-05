using System;
using UniRx;

namespace Features.GamePrepare
{
    public interface IGamePrepareService
    {
        IObservable<Unit> OnLoadStarted { get; }
        IObservable<Unit> OnLoadEnded { get; }
    }
}