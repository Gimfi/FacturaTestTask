using System;
using Core.Asset;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Road.View
{
    public sealed class RoadViewCreator : IInitializable
    {
        private readonly IRoadService m_Service;
        private readonly IAssetProvider m_AssetProvider;
        private readonly CompositeDisposable m_Disposable = new(); //never Dispose because exemplar never destroyed.

        private readonly Subject<RoadSegmentView> m_OnRoadSegmentViewLoaded = new();
        private readonly Subject<RoadCreateRequest> m_OnRoadCreateRequest = new();
        
        public RoadViewCreator(IRoadService service, IAssetProvider assetProvider)
        {
            m_Service = service;
            m_AssetProvider = assetProvider;
        }

        public IObservable<RoadSegmentView> OnRoadSegmentViewLoaded => m_OnRoadSegmentViewLoaded;
        public IObservable<RoadCreateRequest> OnRoadCreateRequest => m_OnRoadCreateRequest;

        public void Initialize()
        {
            m_Service.OnRoadCreateRequest.Subscribe(data => CreateRoad(data).Forget()).AddTo(m_Disposable);
        }

        private async UniTask CreateRoad(RoadCreateRequest data)
        {
            try
            {
                GameObject go = await m_AssetProvider.GetAssetAsync<GameObject>(RoadViewConstants.AssetName);
                if (go.TryGetComponent(out RoadSegmentView roadSegmentView))
                {
                    m_OnRoadSegmentViewLoaded.OnNext(roadSegmentView);
                    m_OnRoadCreateRequest.OnNext(data);
                }
                else
                {
                    Debug.LogError("[RoadViewCreator]: cannot find RoadSegmentView component!!!");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}