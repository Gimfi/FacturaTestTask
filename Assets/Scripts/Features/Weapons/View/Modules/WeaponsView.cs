using UnityEngine;

namespace Features.Weapons.View
{
    public sealed class WeaponsView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Turret;

        [SerializeField]
        private WeaponsRotationView m_RotationComponent;
        
        [SerializeField]
        private WeaponsFireView m_FireView;
        

        private Camera m_MainCamera;

        private void Awake()
        {
            m_MainCamera = Camera.main;
            m_RotationComponent.Init(m_Turret);
        }

        private void Update()
        {
            if (!IsInScreen())
                return;

            Ray ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);
            m_RotationComponent.RotateTurretToMouse(ray);
            m_FireView.CustomUpdate();
        }


        private bool IsInScreen()
        {
            Vector3 mousePos = Input.mousePosition;
            return !(mousePos.x < 0) && !(mousePos.y < 0) && !(mousePos.x > Screen.width) &&
                   !(mousePos.y > Screen.height);
        }
    }
}