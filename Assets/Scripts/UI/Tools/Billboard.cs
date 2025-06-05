using UnityEngine;

namespace UI.Tools
{
    public sealed class Billboard : MonoBehaviour
    {
        private Camera m_Camera;

        private void Awake()
        {
            m_Camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (m_Camera)
                transform.rotation = Quaternion.LookRotation(transform.position - m_Camera.transform.position);
        }
    }
}