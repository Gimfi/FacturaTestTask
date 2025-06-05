using Features.Vehicles.View;
using UnityEngine;

namespace Features.Enemies.View
{
    public class EnemyKamikazeView : EnemyComponentBase
    {
        private EnemyModel m_Model;

        private void OnTriggerEnter(Collider other)
        {
            VehicleHealthComponent enemy = other.gameObject.GetComponent<VehicleHealthComponent>();
            if (enemy)
            {
                enemy.MakeDamage(2);
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            if (m_Model.TryGetComponent(out EnemyHealth healthModel))
            {
                int hp = healthModel.Health.Value;
                healthModel.SetHpDecrease(hp);
            }
        }

        public override void BindModelComponent(EnemyModel model)
        {
            m_Model = model;
        }

        public override void Release()
        {
            m_Model = null;
        }
    }
}