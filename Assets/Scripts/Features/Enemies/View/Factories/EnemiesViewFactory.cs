using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.Enemies.View
{
    public class EnemiesViewFactory : IFactory<EnemyView, Transform, UniTask<EnemyView>>
    {
        private readonly DiContainer m_Container;

        public EnemiesViewFactory(DiContainer container)
        {
            m_Container = container;
        }

        public async UniTask<EnemyView> Create(EnemyView enemyView, Transform container)
        {
            AsyncInstantiateOperation<EnemyView> asyncOp = Object.InstantiateAsync(enemyView, container);
            await asyncOp;

            EnemyView segment = asyncOp.Result[0];
            m_Container.InjectGameObject(segment.gameObject);
            return segment;
        }
    }
}