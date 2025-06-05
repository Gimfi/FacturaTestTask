using UnityEngine;

namespace Features.Enemies.View
{
    public sealed class EnemyHealthComponent : EnemyComponentBase
    {
        [SerializeField]
        private HealthBarUI m_HealthBar;

        private EnemyHealth m_HealthModel;

        public override void BindModelComponent(EnemyModel model)
        {
            if (model.TryGetComponent(out m_HealthModel))
            {
                int maxHp = m_HealthModel.Health.Value;
                m_HealthBar.Bind(m_HealthModel.Health, maxHp);
            }
        }

        public void MakeDamage(int damage)
        {
            m_HealthModel?.SetHpDecrease(damage);
        }

        public override void Release()
        {
            m_HealthModel = null;
        }
    }
}