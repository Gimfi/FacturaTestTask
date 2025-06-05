using UnityEngine;

namespace Features.Enemies.View
{
    public abstract class EnemyComponentBase : MonoBehaviour
    {
        protected GameObject Root;

        public void Init(GameObject root)
        {
            Root = root;
        }

        public abstract void BindModelComponent(EnemyModel model);
        public abstract void Release();
    }
}