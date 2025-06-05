using System;
using UnityEngine;

namespace Features.GameSession
{
    public interface IGameCompletionController
    {
        public IObservable<bool> OnGameEnded { get; }
        void UpdateVehiclePosition(Vector3 position);
    }
}