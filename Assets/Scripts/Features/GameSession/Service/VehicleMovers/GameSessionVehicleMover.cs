using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Features.GameSession
{
    public sealed class GameSessionVehicleMover : IGameSessionVehicleMover
    {
        private readonly float m_Acceleration = 2;
        private readonly float m_MaxSpeed = 10;
        private readonly float m_SwerveAmplitude = 0.5f;
        private readonly float m_SwerveFrequency = 0.3f;
        private readonly float m_MaxOffsetFromCenter = 2;

        private bool m_IsMoving;
        private Vector3 m_VehiclePosition;
        private float m_CurrentSpeed;
        private float m_StartX;

        private readonly Subject<Vector3> m_OnPositionUpdated = new();
        public IObservable<Vector3> OnPositionUpdated => m_OnPositionUpdated;

        public async UniTask StartMove()
        {
            m_IsMoving = true;
            m_VehiclePosition = Vector3.zero;
            m_CurrentSpeed = 0;
            m_StartX = m_VehiclePosition.x;

            float time = 0;

            while (m_IsMoving)
            {
                float deltaTime = Time.deltaTime;
                m_CurrentSpeed = Mathf.Min(m_CurrentSpeed + m_Acceleration * deltaTime, m_MaxSpeed);
                
                time += deltaTime;
                float xOffset = Mathf.Sin(time * m_SwerveFrequency) * m_SwerveAmplitude;
                xOffset = Mathf.Clamp(xOffset, -m_MaxOffsetFromCenter, m_MaxOffsetFromCenter);

                Vector3 pos = m_VehiclePosition;
                pos += Vector3.forward * (m_CurrentSpeed * deltaTime);
                pos.x = m_StartX + xOffset;

                m_VehiclePosition = pos;
                m_OnPositionUpdated.OnNext(m_VehiclePosition);

                await UniTask.Yield();
            }
        }

        public void StopMove()
        {
            m_IsMoving = false;
        }
    }
}