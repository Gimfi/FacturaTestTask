using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Enemies
{
    public sealed class EnemiesService : IEnemiesService, IInitializable
    {
        private readonly IEnemyCreator m_EnemiesCreator;
        private readonly IEnemiesHpObserver m_EnemiesHpObserver;
        private readonly IEnemiesAttackBehavior m_EnemiesAttackBehavior;
        private readonly IEnemiesValidator m_EnemiesValidator;

        private Vector3 m_AnchorPosition;
        private readonly Subject<List<EnemyModel>> m_OnEnemiesInitiateRequest = new();
        private readonly Subject<List<EnemyModel>> m_OnEnemiesCreateRequest = new();
        private readonly Subject<List<EnemyModel>> m_OnEnemiesDestroyRequest = new();

        private readonly List<EnemyModel> m_Enemies = new();

        public EnemiesService(IEnemyCreator enemiesCreator, IEnemiesHpObserver enemiesHpObserver,
            IEnemiesAttackBehavior enemiesAttackBehavior,
            IEnemiesValidator enemiesValidator)
        {
            m_EnemiesCreator = enemiesCreator;
            m_EnemiesHpObserver = enemiesHpObserver;
            m_EnemiesAttackBehavior = enemiesAttackBehavior;
            m_EnemiesValidator = enemiesValidator;
            m_AnchorPosition = Vector3.zero;
        }

        public IObservable<List<EnemyModel>> OnEnemiesInitiateRequest => m_OnEnemiesInitiateRequest;
        public IObservable<List<EnemyModel>> OnEnemiesCreateRequest => m_OnEnemiesCreateRequest;
        public IObservable<List<EnemyModel>> OnEnemiesDestroyRequest => m_OnEnemiesDestroyRequest;

        public void Initialize()
        {
            m_EnemiesHpObserver.OnEnemyDestroyRequest.Subscribe(DestroyEnemy);
        }

        public void Reset(Vector3 resetAnchor)
        {
            m_AnchorPosition = resetAnchor;
        }

        public void LaunchEnemySpawn()
        {
            ValidateEnemies();
            List<EnemyModel> newEnemies = CreateEnemies();
            m_EnemiesHpObserver.AddEnemies(newEnemies);
            m_EnemiesAttackBehavior.AddEnemies(newEnemies);
            m_OnEnemiesInitiateRequest.OnNext(newEnemies);
        }

        private void ValidateEnemies()
        {
            List<EnemyModel> enemiesToDestroy =
                m_EnemiesValidator.ValidateEnemies(m_Enemies, EnemiesConstants.ValidSegment, m_AnchorPosition);

            m_OnEnemiesDestroyRequest.OnNext(enemiesToDestroy);
        }

        private void DestroyEnemy(EnemyModel enemyModel)
        {
            m_Enemies.Remove(enemyModel);
            m_OnEnemiesDestroyRequest.OnNext(new List<EnemyModel> { enemyModel });
        }

        private List<EnemyModel> CreateEnemies()
        {
            const int enemiesCount = EnemiesConstants.EnemiesPerSpawnCount;
            Vector3 spawnSegment = EnemiesConstants.SpawnSegment;
            Vector3 spawnAnchorPosition = m_AnchorPosition + EnemiesConstants.SpawnSegmentShift;

            return m_EnemiesCreator.CreateEnemies(m_Enemies, enemiesCount, spawnSegment, spawnAnchorPosition);
        }

        public void UpdateAnchorPosition(Vector3 position)
        {
            m_EnemiesAttackBehavior.UpdateTargetPosition(position);

            if (position.z - m_AnchorPosition.z > EnemiesConstants.SpawnSegment.z)
            {
                m_AnchorPosition = new Vector3(0, 0, position.z);

                ValidateEnemies();
                List<EnemyModel> newEnemies = CreateEnemies();
                m_EnemiesHpObserver.AddEnemies(newEnemies);
                m_EnemiesAttackBehavior.AddEnemies(newEnemies);
                m_OnEnemiesCreateRequest.OnNext(newEnemies);
            }
        }
    }
}