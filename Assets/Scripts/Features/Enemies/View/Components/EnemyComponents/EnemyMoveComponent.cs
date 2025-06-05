using UniRx;
using UnityEngine;

namespace Features.Enemies.View
{
    public sealed class EnemyMoveComponent : EnemyComponentBase
    {
        private readonly int m_SpeedHash = Animator.StringToHash("Speed");

        [SerializeField]
        private Animator m_Animator;

        private EnemyPosition m_PositionModel;
        private readonly CompositeDisposable m_Disposable = new();

        public override void BindModelComponent(EnemyModel model)
        {
            if (model.TryGetComponent(out m_PositionModel))
            {
                UpdatePosition(m_PositionModel.Position.Value);
                UpdateRotation(m_PositionModel.Rotation.Value);

                m_PositionModel.Position.Subscribe(UpdatePosition).AddTo(m_Disposable);
                m_PositionModel.Rotation.Subscribe(UpdateRotation).AddTo(m_Disposable);
            }
        }

        private void UpdatePosition(Vector3 position)
        {
            ShowMoveAnim(position);
            Root.transform.localPosition = position;
        }

        private void UpdateRotation(Quaternion rotation)
        {
            Root.transform.rotation = rotation;
        }

        private void ShowMoveAnim(Vector3 position)
        {
            float distance = Vector3.Distance(position, Root.transform.localPosition);
            float speed = distance / Time.deltaTime;
            
            m_Animator.SetFloat(m_SpeedHash, speed);
        }

        public override void Release()
        {
            m_Disposable.Clear();
            m_PositionModel = null;
        }
    }
}