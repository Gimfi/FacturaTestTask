using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies
{
    public sealed class EnemyCreator : IEnemyCreator
    {
        private readonly IEnemiesFactory m_EnemiesFactory;
        private readonly IEnemiesInitializer m_EnemiesInitializer;

        public EnemyCreator(IEnemiesFactory enemiesFactory, IEnemiesInitializer enemiesInitializer)
        {
            m_EnemiesFactory = enemiesFactory;
            m_EnemiesInitializer = enemiesInitializer;
        }

        public List<EnemyModel> CreateEnemies(List<EnemyModel> enemies, int createCount, Vector3 spawnSegment,
            Vector3 anchorPosition)
        {
            List<EnemyModel> newEnemies = CreateEnemiesInternal(enemies, createCount);
            m_EnemiesInitializer.InitializeEnemies(newEnemies, spawnSegment, anchorPosition);
            return newEnemies;
        }

        private List<EnemyModel> CreateEnemiesInternal(List<EnemyModel> enemies, int count)
        {
            List<EnemyModel> newEnemies = new List<EnemyModel>();

            for (int i = 0; i < count; i++)
            {
                EnemyModel enemyModel = m_EnemiesFactory.CreatEnemy();
                newEnemies.Add(enemyModel);
            }

            enemies.AddRange(newEnemies);
            return newEnemies;
        }
    }
}