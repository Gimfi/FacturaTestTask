using System;
using System.Collections.Generic;
using Core.Asset;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Launcher
{
    public sealed class GameLauncher : IGameLauncher, IInitializable
    {
        private readonly IAssetPreloader m_AssetPreloader;
        private readonly Subject<Unit> m_OnLoadStarted = new();
        private readonly Subject<Unit> m_OnLoadEnded = new();

        private readonly List<IGameLauncherPreLaunchProcessor> m_PreLaunchProcessors = new();

        public GameLauncher(IAssetPreloader assetPreloader,
            IGameLauncherPreLaunchProcessor[] gameLauncherPreLaunchProcessors)
        {
            m_AssetPreloader = assetPreloader;
            m_PreLaunchProcessors.AddRange(gameLauncherPreLaunchProcessors);
        }

        public IObservable<Unit> OnLoadStarted => m_OnLoadStarted;
        public IObservable<Unit> OnLoadEnded => m_OnLoadEnded;

        public void AddPreLaunchProcessor(IGameLauncherPreLaunchProcessor processor)
        {
            if (!m_PreLaunchProcessors.Contains(processor))
                m_PreLaunchProcessors.Add(processor);
            else
                Debug.LogError("[GameLauncher]: Duplicate game launcher pre launch processor");
        }

        public void Initialize()
        {
            LaunchGame().Forget();
        }

        public async UniTask LaunchGame()
        {
            try
            {
                m_OnLoadStarted.OnNext(Unit.Default);
                await PreDownloadNecessaryAssets();
                await RunAllPreLaunchProcessors();
                m_OnLoadEnded.OnNext(Unit.Default);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async UniTask PreDownloadNecessaryAssets()
        {
            await m_AssetPreloader.PreDownloadNecessaryAssets();
        }

        private async UniTask RunAllPreLaunchProcessors()
        {
            UniTask[] prelaunchTasks = new UniTask[m_PreLaunchProcessors.Count];

            for (int i = 0; i < m_PreLaunchProcessors.Count; i++)
            {
                var processor = m_PreLaunchProcessors[i];
                UniTask task = processor.RunProcess();
                prelaunchTasks[i] = task;
            }

            await UniTask.WhenAll(prelaunchTasks);
        }
    }
}