using System;
using Features.Enemies.View;
using UniRx;
using UnityEngine;

namespace Features.Weapons.View
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody m_Rigidbody;

        [SerializeField]
        private TrailRenderer m_TrailRenderer;

        private readonly Subject<ProjectileView> m_OnProjectileReadyToPool = new();

        public Rigidbody Rigidbody => m_Rigidbody;
        public IObservable<ProjectileView> OnProjectileReadyToPool => m_OnProjectileReadyToPool;

        private void OnCollisionEnter(Collision collision)
        {
            EnemyHealthComponent enemy = collision.gameObject.GetComponent<EnemyHealthComponent>();

            if (enemy)
                enemy.MakeDamage(2);

            m_OnProjectileReadyToPool.OnNext(this);
        }

        public void Prepare()
        {
            m_TrailRenderer.emitting = false;
        }

        public void Fire()
        {
            m_TrailRenderer.emitting = true;
        }

        public void Release()
        {
            m_Rigidbody.velocity = Vector3.zero;
        }
    }
}