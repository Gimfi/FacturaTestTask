using UnityEngine;
using Zenject;

namespace Features.Vehicles.View
{
    public sealed class VehicleHealthComponent : MonoBehaviour
    {
        [SerializeField]
        private HealthBarUI m_HealthBar;

        private IVehiclesService m_Service;

        [Inject]
        private void Construct(IVehiclesService service)
        {
            m_Service = service;
            m_HealthBar.Bind(m_Service.VehicleHealth, VehiclesConstants.VehicleHealth);
        }

        public void MakeDamage(int damage)
        {
            m_Service.SetHpDecrease(damage);
        }
    }
}