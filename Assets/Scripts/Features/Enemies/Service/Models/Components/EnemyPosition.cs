using UniRx;
using UnityEngine;

namespace Features.Enemies
{
    public sealed class EnemyPosition : IEnemyComponent
    {
        private readonly ReactiveProperty<Vector3> m_Position = new();
        private readonly ReactiveProperty<Quaternion> m_Rotation = new();

        public void SetPosition(Vector3 position)
        {
            m_Position.Value = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            m_Rotation.Value = rotation;
        }

        public IReadOnlyReactiveProperty<Vector3> Position => m_Position;
        public IReadOnlyReactiveProperty<Quaternion> Rotation => m_Rotation;
    }
}