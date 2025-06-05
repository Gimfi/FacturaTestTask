using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies
{
    public sealed class EnemiesInitializer : IEnemiesInitializer
    {
        private const float MinDistanceBetweenEnemies = 1;
        private const float MaxAttempts = 30;

        public void InitializeEnemies(List<EnemyModel> newEnemies, Vector3 spawnSegment, Vector3 anchorPosition)
        {
            List<Vector3> usedPositions = new List<Vector3>();

            foreach (EnemyModel enemy in newEnemies)
            {
                Vector3 spawnPosition = Vector3.zero;
                bool validPositionFound = false;
                int attemptCount = 0;

                while (!validPositionFound && attemptCount < MaxAttempts)
                {
                    float x = Random.Range(-spawnSegment.x / 2f, spawnSegment.x / 2f);
                    float z = Random.Range(-spawnSegment.z / 2f, spawnSegment.z / 2f);
                    spawnPosition = new Vector3(anchorPosition.x + x, spawnSegment.y, anchorPosition.z + z);

                    bool isFarEnough = true;
                    foreach (Vector3 pos in usedPositions)
                    {
                        if ((pos - spawnPosition).sqrMagnitude < MinDistanceBetweenEnemies * MinDistanceBetweenEnemies)
                        {
                            isFarEnough = false;
                            break;
                        }
                    }

                    if (isFarEnough)
                    {
                        validPositionFound = true;
                        usedPositions.Add(spawnPosition);
                    }

                    attemptCount++;
                }

                if (enemy.TryGetComponent(out EnemyPosition position))
                {
                    position.SetPosition(spawnPosition);

                    Quaternion randomYRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    position.SetRotation(randomYRotation);
                }
            }
        }
    }
}