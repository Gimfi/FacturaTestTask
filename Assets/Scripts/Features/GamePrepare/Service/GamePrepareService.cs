using System;
using Core.Launcher;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.GamePrepare
{
    public sealed class GamePrepareService : IGamePrepareService, IInitializable
    {
        private readonly IGameLauncher m_GameLauncher;
        private readonly CompositeDisposable m_Disposable = new(); //never Dispose because exemplar never destroyed.

        private readonly Subject<Unit> m_OnLoadStarted = new();
        private readonly Subject<Unit> m_OnLoadEnded = new();

        public IObservable<Unit> OnLoadStarted => m_OnLoadStarted;
        public IObservable<Unit> OnLoadEnded => m_OnLoadEnded;

        public GamePrepareService(IGameLauncher gameLauncher)
        {
            m_GameLauncher = gameLauncher;
        }

        public void Initialize()
        {
            m_GameLauncher.OnLoadStarted.Subscribe(_ => ProcessLoadStarted()).AddTo(m_Disposable);
            m_GameLauncher.OnLoadEnded.Subscribe(_ => ProcessLoadEnded()).AddTo(m_Disposable);
        }

        private async void ProcessLoadStarted()
        {
            try
            {
                await ProcessPreLoadStarted();
                m_OnLoadStarted.OnNext(Unit.Default);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private UniTask ProcessPreLoadStarted()
        {
            //Do something, if needed. Can add async.
            return UniTask.CompletedTask;
        }

        private async void ProcessLoadEnded()
        {
            try
            {
                await ProcessPreProcessLoadEnded();
                m_OnLoadEnded.OnNext(Unit.Default);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private UniTask ProcessPreProcessLoadEnded()
        {
            //Do something, if needed. Can add async.
            return UniTask.CompletedTask;
        }
    }
}