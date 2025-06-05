using System;
using System.Collections.Generic;

namespace Features.Enemies
{
    public sealed class EnemyModel
    {
        private readonly Dictionary<Type, IEnemyComponent> m_Components = new();
        
        public void AddComponent<T>(T component) where T : IEnemyComponent
        {
            m_Components[typeof(T)] = component;
        }

        public bool TryGetComponent<T>(out T component) where T : class, IEnemyComponent
        {
            if (m_Components.TryGetValue(typeof(T), out IEnemyComponent value))
            {
                component = value as T;
                return true;
            }

            component = null;
            return false;
        }

        public bool HasComponent<T>() where T : IEnemyComponent
        {
            return m_Components.ContainsKey(typeof(T));
        }
    }
}