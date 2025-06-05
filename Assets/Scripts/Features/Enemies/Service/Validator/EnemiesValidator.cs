using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies
{
    public sealed class EnemiesValidator : IEnemiesValidator
    {
        private Vector3 m_LastAnchor;
        private readonly List<EnemyModel> m_EmptyDestroyList = new(1);

        public List<EnemyModel> ValidateEnemies(List<EnemyModel> enemies, Vector3 validSegment, Vector3 anchor)
        {
            List<EnemyModel> result = m_EmptyDestroyList;

            if (m_LastAnchor != anchor)
            {
                m_LastAnchor = anchor;
                result = ScanCurrentEnemies(enemies, validSegment, anchor);
            }

            return result;
        }

        private List<EnemyModel> ScanCurrentEnemies(List<EnemyModel> enemies, Vector3 validSegment, Vector3 anchor)
        {
            List<EnemyModel> enemiesToDestroy = new();

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                EnemyModel enemyModel = enemies[i];
                if (enemyModel.TryGetComponent(out EnemyPosition position))
                {
                    bool isInValidPosition = IsInsideArea(position.Position.Value, anchor, validSegment);
                    if (!isInValidPosition)
                    {
                        enemiesToDestroy.Add(enemyModel);
                        enemies.RemoveAt(i);
                    }
                }
            }

            return enemiesToDestroy;
        }

        private bool IsInsideArea(Vector3 position, Vector3 anchor, Vector3 size)
        {
            Vector3 min = anchor - size * 0.5f;
            Vector3 max = anchor + size * 0.5f;

            return position.x >= min.x && position.x <= max.x &&
                   position.y >= min.y && position.y <= max.y &&
                   position.z >= min.z && position.z <= max.z;
        }
    }
}