using System;
using System.Collections.Generic;
using UniRx;

namespace Features.Enemies
{
    public sealed class EnemiesHpObserver : IEnemiesHpObserver
    {
        private readonly Subject<EnemyModel> m_OnEnemyDestroyRequest = new();
        private readonly Dictionary<EnemyModel, IDisposable> m_Subscriptions = new();

        public IObservable<EnemyModel> OnEnemyDestroyRequest => m_OnEnemyDestroyRequest;

        public void AddEnemies(List<EnemyModel> enemies)
        {
            foreach (EnemyModel enemy in enemies)
            {
                if (enemy.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    if (m_Subscriptions.ContainsKey(enemy))
                        continue;

                    IDisposable subscription = enemyHealth.Health
                        .Subscribe(hp =>
                        {
                            if (hp <= 0)
                                KillEnemy(enemy);
                        });

                    m_Subscriptions[enemy] = subscription;
                }
            }
        }

        private void KillEnemy(EnemyModel enemy)
        {
            m_OnEnemyDestroyRequest.OnNext(enemy);
            RemoveEnemy(enemy);
        }

        public void RemoveEnemies(List<EnemyModel> enemies)
        {
            foreach (EnemyModel enemy in enemies)
                RemoveEnemy(enemy);
        }

        private void RemoveEnemy(EnemyModel enemy)
        {
            if (m_Subscriptions.TryGetValue(enemy, out IDisposable subscription))
            {
                subscription.Dispose();
                m_Subscriptions.Remove(enemy);
            }
        }
    }
}