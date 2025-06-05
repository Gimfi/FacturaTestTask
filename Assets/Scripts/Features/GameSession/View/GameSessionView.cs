using Cysharp.Threading.Tasks;
using Features.GameCamera.View;
using Features.Vehicles.View;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.GameSession.View
{
    public sealed class GameSessionView : MonoBehaviour
    {
        private IGameSessionService m_GameSessionService;
        private VehicleContainerView m_VehicleContainerView;
        private GameCameraContainerView m_CameraContainerView;

        private readonly CompositeDisposable m_Disposable = new();

        [Inject]
        private void Construct(IGameSessionService gameSessionService, VehicleContainerView vehicleContainerView,
            GameCameraContainerView gameCameraContainerView)
        {
            m_GameSessionService = gameSessionService;
            m_VehicleContainerView = vehicleContainerView;
            m_CameraContainerView = gameCameraContainerView;
        }

        private void Awake()
        {
            m_VehicleContainerView.OnNewVehicleSpawned.Subscribe(ProcessNewVehicleSpawned).AddTo(m_Disposable);
            m_GameSessionService.OnGameEnded.Subscribe(_ => ProcessGameEnded()).AddTo(m_Disposable);
            m_GameSessionService.OnGameReadyToStart.Subscribe(_ => ProcessGameReadyToStart()).AddTo(m_Disposable);
            m_GameSessionService.OnGameStarted.Subscribe(_ => ProcessGameStarted()).AddTo(m_Disposable);
        }

        private void OnDestroy()
        {
            m_Disposable.Dispose();
        }

        private void ProcessNewVehicleSpawned(GameObject vehicleGameObject)
        {
            m_CameraContainerView.SetNewTarget(vehicleGameObject);
            m_CameraContainerView.PrepareToStart();
        }

        private void ProcessGameEnded()
        {
            m_CameraContainerView.StopFollow();
        }

        private void ProcessGameReadyToStart()
        {
            m_CameraContainerView.PrepareToStart();
        }

        private void ProcessGameStarted()
        {
            m_CameraContainerView.StartFlySequence().Forget();
        }
    }
}