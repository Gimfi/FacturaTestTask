using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies
{
    public interface IEnemiesInitializer
    {
        void InitializeEnemies(List<EnemyModel> newEnemies, Vector3 spawnSegment, Vector3 anchorPosition);
    }
}