using System;
using Cysharp.Threading.Tasks;
using Features.Enemies;
using Features.GamePrepare;
using Features.Road;
using Features.Vehicles;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.GameSession
{
    public sealed class GameSessionService : IGameSessionService, IInitializable
    {
        private readonly IGamePrepareService m_GamePrepareService;
        private readonly IGameSessionVehicleMover m_VehicleMover;
        private readonly IGameCompletionController m_GameCompletionController;
        private readonly IVehiclesService m_VehiclesService;
        private readonly IEnemiesService m_EnemiesService;
        private readonly IRoadService m_RoadService;

        private bool m_LastGameResult;
        private readonly CompositeDisposable m_Disposable = new(); //never Dispose because exemplar never destroyed.

        private readonly Subject<Unit> m_OnGameReadyToStart = new();
        private readonly Subject<Unit> m_OnGameStarted = new();
        private readonly Subject<Unit> m_OnGameEnded = new();

        public GameSessionService(IGamePrepareService gamePrepareService, IGameSessionVehicleMover vehicleMover,
            IGameCompletionController gameCompletionController,
            IVehiclesService vehiclesService, IEnemiesService enemiesService, IRoadService roadService)
        {
            m_GamePrepareService = gamePrepareService;
            m_VehicleMover = vehicleMover;
            m_GameCompletionController = gameCompletionController;
            m_VehiclesService = vehiclesService;
            m_EnemiesService = enemiesService;
            m_RoadService = roadService;
        }

        public bool LastGameResult => m_LastGameResult;
        public IObservable<Unit> OnGameReadyToStart => m_OnGameReadyToStart;
        public IObservable<Unit> OnGameStarted => m_OnGameStarted;
        public IObservable<Unit> OnGameEnded => m_OnGameEnded;

        public void Initialize()
        {
            m_GamePrepareService.OnLoadEnded.Subscribe(_ => ProcessLoadEnded()).AddTo(m_Disposable);
            m_VehicleMover.OnPositionUpdated.Subscribe(ProcessMoveVehicle).AddTo(m_Disposable);
            m_GameCompletionController.OnGameEnded.Subscribe(GameEnded).AddTo(m_Disposable);
        }

        private void ProcessLoadEnded()
        {
            m_OnGameReadyToStart.OnNext(Unit.Default);
        }

        public void ResetGame()
        {
            Vector3 resetAnchor = Vector3.zero;
            m_VehiclesService.Reset(resetAnchor);
            m_RoadService.Reset(resetAnchor);
            m_EnemiesService.Reset(resetAnchor);

            m_OnGameReadyToStart.OnNext(Unit.Default);
        }

        public void LaunchGameSession()
        {
            m_EnemiesService.LaunchEnemySpawn();
            m_VehiclesService.RepairVehicle();
            m_VehicleMover.StartMove().Forget();

            m_OnGameStarted.OnNext(Unit.Default);
        }

        private void ProcessMoveVehicle(Vector3 position)
        {
            m_VehiclesService.RepositionVehicle(position);
            m_RoadService.UpdateAnchorPosition(position);
            m_EnemiesService.UpdateAnchorPosition(position);
            m_GameCompletionController.UpdateVehiclePosition(position);
        }

        private void GameEnded(bool isWin)
        {
            m_LastGameResult = isWin;
            m_VehicleMover.StopMove();

            m_OnGameEnded.OnNext(Unit.Default);
        }
    }
}