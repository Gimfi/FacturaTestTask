using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.Vehicles.View
{
    public class VehiclesSegmentFactory : IFactory<VehicleView, Transform, UniTask<VehicleView>>
    {
        private readonly DiContainer m_Container;

        public VehiclesSegmentFactory(DiContainer container)
        {
            m_Container = container;
        }

        public async UniTask<VehicleView> Create(VehicleView enemyView, Transform container)
        {
            AsyncInstantiateOperation<VehicleView> asyncOp = Object.InstantiateAsync(enemyView, container);
            await asyncOp;

            VehicleView segment = asyncOp.Result[0];
            m_Container.InjectGameObject(segment.gameObject);
            return segment;
        }
    }
}