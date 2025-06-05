using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.GameSession
{
    public interface IGameSessionVehicleMover
    {
        IObservable<Vector3> OnPositionUpdated { get; }
        UniTask StartMove();
        void StopMove();
    }
}