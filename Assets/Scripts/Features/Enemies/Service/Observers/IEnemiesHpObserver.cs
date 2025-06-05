using System;
using System.Collections.Generic;

namespace Features.Enemies
{
    public interface IEnemiesHpObserver
    {
        IObservable<EnemyModel> OnEnemyDestroyRequest { get; }
        
        void AddEnemies(List<EnemyModel> enemies);
        void RemoveEnemies(List<EnemyModel> enemies);
    }
}