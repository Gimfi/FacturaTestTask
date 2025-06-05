using System;
using Features.Vehicles;
using UniRx;
using UnityEngine;

namespace Features.GameSession
{
    public sealed class GameCompletionController : IGameCompletionController
    {
        private readonly Subject<bool> m_OnGameEnded = new();
        public IObservable<bool> OnGameEnded => m_OnGameEnded;

        public GameCompletionController(IVehiclesService vehiclesService)
        {
            vehiclesService.VehicleHealth.Subscribe(ProcessVehicleHp);
        }

        private void ProcessVehicleHp(int hp)
        {
            if (hp <= 0)
                m_OnGameEnded.OnNext(false);
        }

        public void UpdateVehiclePosition(Vector3 position)
        {
            if (position.z > GameSessionConstants.MinWinPosition)
                m_OnGameEnded.OnNext(true);
        }
    }
}