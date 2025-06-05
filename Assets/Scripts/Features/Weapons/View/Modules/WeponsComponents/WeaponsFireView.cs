using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Weapons.View
{
    public sealed class WeaponsFireView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_ShootPoint;

        [SerializeField]
        private float m_ShootForce;

        private ProjectileViewsPool m_ProjectileViewsPool;

        [Inject]
        private void Construct(ProjectileViewsPool projectileViewsPool)
        {
            m_ProjectileViewsPool = projectileViewsPool;
        }

        public void CustomUpdate()
        {
            if (Input.GetMouseButtonDown(0))
                Shoot().Forget();
        }

        private async UniTask Shoot()
        {
            ProjectileView projectileView = await m_ProjectileViewsPool.GetProjectileView();
            projectileView.OnProjectileReadyToPool.Subscribe(ReturnProjectileToPool).AddTo(this);
            projectileView.transform.position = m_ShootPoint.position;
            projectileView.transform.SetParent(null);
            projectileView.Fire();

            Rigidbody rb = projectileView.Rigidbody;
            rb.AddForce(m_ShootPoint.forward * m_ShootForce);
        }

        private void ReturnProjectileToPool(ProjectileView projectileView)
        {
            m_ProjectileViewsPool.Release(projectileView);
        }
    }
}