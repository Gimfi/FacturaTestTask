using UnityEngine;

namespace Features.Weapons.View
{
    public sealed class WeaponsRotationView : MonoBehaviour
    {
        [SerializeField]
        private float m_RotationSpeed;

        [SerializeField]
        private LayerMask m_AimLayerMask;

        private Transform m_Turret;

        public void Init(Transform turret)
        {
            m_Turret = turret;
        }

        public void RotateTurretToMouse(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_AimLayerMask))
            {
                Vector3 direction = hit.point - m_Turret.position;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    m_Turret.rotation = Quaternion.Slerp(m_Turret.rotation, targetRotation,
                        Time.deltaTime * m_RotationSpeed);
                }
            }
        }
    }
}