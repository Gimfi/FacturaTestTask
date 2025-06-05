using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies
{
    public interface IEnemiesValidator
    {
        List<EnemyModel> ValidateEnemies(List<EnemyModel> enemies, Vector3 validSegment, Vector3 anchor);
    }
}