using UnityEngine;

namespace Features.Enemies.View
{
    public sealed class EnemyView : MonoBehaviour
    {
        private EnemyModel m_Model;
        private EnemyComponentBase[] m_Components;

        public EnemyModel Model => m_Model;

        public void ReInit(EnemyModel model)
        {
            m_Model = model;
            m_Components = GetComponents<EnemyComponentBase>();

            ProcessModel();
        }

        private void ProcessModel()
        {
            foreach (EnemyComponentBase component in m_Components)
            {
                component.Init(gameObject);
                component.BindModelComponent(m_Model);
            }
        }

        public void Release()
        {
            foreach (EnemyComponentBase component in m_Components)
                component.Release();
        }
    }
}