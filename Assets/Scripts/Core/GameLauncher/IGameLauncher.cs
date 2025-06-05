using System;
using UniRx;

namespace Core.Launcher
{
    public interface IGameLauncher
    {
        public IObservable<Unit> OnLoadStarted { get; }
        public IObservable<Unit> OnLoadEnded { get; }

        void AddPreLaunchProcessor(IGameLauncherPreLaunchProcessor processor);
    }
}