using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies
{
    public sealed class EnemiesAttackBehavior : IEnemiesAttackBehavior
    {
        private readonly Dictionary<EnemyModel, EnemyPosition> m_Enemies = new();

        public void AddEnemies(List<EnemyModel> enemies)
        {
            foreach (EnemyModel enemy in enemies)
            {
                if (enemy.TryGetComponent(out EnemyPosition enemyPosition))
                    m_Enemies[enemy] = enemyPosition;
            }
        }

        public void RemoveEnemies(List<EnemyModel> enemies)
        {
            foreach (EnemyModel enemy in enemies)
                m_Enemies.Remove(enemy);
        }

        public void UpdateTargetPosition(Vector3 position)
        {
            foreach (EnemyPosition enemyPosition in m_Enemies.Values)
            {
                Vector3 delta = position - enemyPosition.Position.Value;
                if (delta.sqrMagnitude < EnemiesConstants.EnemiesParams.AggressionRange)
                {
                    Vector3 direction = delta.normalized;
                    Vector3 currentPosition = enemyPosition.Position.Value;
                    Vector3 distance = direction * (EnemiesConstants.EnemiesParams.Speed * Time.deltaTime);
                    enemyPosition.SetPosition(currentPosition + distance);
                    
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    enemyPosition.SetRotation(lookRotation);
                }
            }
        }
    }
}