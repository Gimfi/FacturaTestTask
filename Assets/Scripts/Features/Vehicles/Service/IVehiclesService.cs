using System;
using UniRx;
using UnityEngine;

namespace Features.Vehicles
{
    public interface IVehiclesService
    {
        IReadOnlyReactiveProperty<int> VehicleHealth { get; }
        IObservable<VehiclesCreateRequest> OnVehicleCreateRequest { get; }
        IObservable<Vector3> OnPositionUpdated { get; }

        void RepairVehicle();
        void SetHpDecrease(int damage);
        void Reset(Vector3 resetAnchor);
        void VehicleCreated();
        void RepositionVehicle(Vector3 position);
    }
}