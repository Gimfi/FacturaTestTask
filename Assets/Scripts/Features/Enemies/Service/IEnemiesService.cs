using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies
{
    public interface IEnemiesService
    {
        IObservable<List<EnemyModel>> OnEnemiesInitiateRequest { get; }
        IObservable<List<EnemyModel>> OnEnemiesCreateRequest { get; }
        IObservable<List<EnemyModel>> OnEnemiesDestroyRequest { get; }
        
        void Reset(Vector3 resetAnchor);
        void LaunchEnemySpawn();
        void UpdateAnchorPosition(Vector3 position);
    }
}