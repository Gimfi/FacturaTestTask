using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies
{
    public interface IEnemiesAttackBehavior
    {
        public void AddEnemies(List<EnemyModel> enemies);
        public void RemoveEnemies(List<EnemyModel> enemies);
        void UpdateTargetPosition(Vector3 position);
    }
}