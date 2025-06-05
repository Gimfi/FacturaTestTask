using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies
{
    public interface IEnemyCreator
    {
        List<EnemyModel> CreateEnemies(List<EnemyModel> enemies, int createCount, Vector3 spawnSegment,
            Vector3 anchorPosition);
    }
}