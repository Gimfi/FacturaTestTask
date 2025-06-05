using System;
using Core.Launcher;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Features.Vehicles
{
    public sealed class VehiclesService : IVehiclesService, IGameLauncherPreLaunchProcessor
    {
        private bool m_IsVehicleCreated;
        private readonly ReactiveProperty<int> m_VehicleHealth = new();
        private readonly Subject<VehiclesCreateRequest> m_OnVehicleCreateRequest = new();
        private readonly Subject<Vector3> m_OnPositionUpdated = new();

        public IReadOnlyReactiveProperty<int> VehicleHealth => m_VehicleHealth;
        public IObservable<VehiclesCreateRequest> OnVehicleCreateRequest => m_OnVehicleCreateRequest;
        public IObservable<Vector3> OnPositionUpdated => m_OnPositionUpdated;

        public async UniTask RunProcess()
        {
            m_IsVehicleCreated = false;
            VehiclesCreateRequest vehiclesCreateRequest = new VehiclesCreateRequest(VehiclesConstants.VehicleId);
            m_OnVehicleCreateRequest.OnNext(vehiclesCreateRequest);
            await UniTask.WaitUntil(() => m_IsVehicleCreated);
        }

        public void Reset(Vector3 resetAnchor)
        {
            RepositionVehicle(resetAnchor);
        }

        public void RepositionVehicle(Vector3 position)
        {
            m_OnPositionUpdated.OnNext(position);
        }

        public void RepairVehicle()
        {
            m_VehicleHealth.Value = VehiclesConstants.VehicleHealth;
        }

        public void SetHpDecrease(int damage)
        {
            m_VehicleHealth.Value -= damage;
        }

        public void VehicleCreated()
        {
            m_IsVehicleCreated = true;
        }
    }
}