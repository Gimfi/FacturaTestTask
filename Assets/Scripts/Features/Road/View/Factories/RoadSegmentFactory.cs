using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.Road.View
{
    public class RoadSegmentFactory : IFactory<RoadSegmentView, Transform, UniTask<RoadSegmentView>>
    {
        private readonly DiContainer m_Container;

        public RoadSegmentFactory(DiContainer container)
        {
            m_Container = container;
        }

        public async UniTask<RoadSegmentView> Create(RoadSegmentView enemyView, Transform container)
        {
            AsyncInstantiateOperation<RoadSegmentView> asyncOp = Object.InstantiateAsync(enemyView, container);
            await asyncOp;

            RoadSegmentView segment = asyncOp.Result[0];
            m_Container.InjectGameObject(segment.gameObject);
            return segment;
        }
    }
}