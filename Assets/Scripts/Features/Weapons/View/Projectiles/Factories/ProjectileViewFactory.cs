using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.Weapons.View
{
    public class ProjectileViewFactory : IFactory<ProjectileView, Transform, UniTask<ProjectileView>>
    {
        private readonly DiContainer m_Container;

        public ProjectileViewFactory(DiContainer container)
        {
            m_Container = container;
        }

        public async UniTask<ProjectileView> Create(ProjectileView enemyView, Transform container)
        {
            AsyncInstantiateOperation<ProjectileView> asyncOp = Object.InstantiateAsync(enemyView, container);
            await asyncOp;

            ProjectileView segment = asyncOp.Result[0];
            m_Container.InjectGameObject(segment.gameObject);
            return segment;
        }
    }
}