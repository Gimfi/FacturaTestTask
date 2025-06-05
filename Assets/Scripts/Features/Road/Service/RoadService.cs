using System;
using Core.Launcher;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Features.Road
{
    public sealed class RoadService : IRoadService, IGameLauncherPreLaunchProcessor
    {
        private bool m_IsRoadCreated;
        private Vector3 m_AnchorPosition;
        private readonly Subject<RoadCreateRequest> m_OnRoadCreateRequest = new();
        private readonly Subject<RoadCreateRequest> m_OnRoadResetRequest = new();
        private readonly Subject<Vector3> m_OnCurrentAnchorPositionUpdated = new();

        public IObservable<RoadCreateRequest> OnRoadCreateRequest => m_OnRoadCreateRequest;
        public IObservable<RoadCreateRequest> OnRoadResetRequest => m_OnRoadResetRequest;
        public IObservable<Vector3> OnCurrentAnchorPositionUpdated => m_OnCurrentAnchorPositionUpdated;

        public async UniTask RunProcess()
        {
            m_IsRoadCreated = false;
            m_AnchorPosition = Vector3.zero;

            RoadCreateRequest roadCreateRequest = GetRoadCreateRequest();
            m_OnRoadCreateRequest.OnNext(roadCreateRequest);
            await UniTask.WaitUntil(() => m_IsRoadCreated);
        }

        public void Reset(Vector3 anchorPosition)
        {
            m_AnchorPosition = anchorPosition;
            RoadCreateRequest roadCreateRequest = GetRoadCreateRequest();
            m_OnRoadResetRequest.OnNext(roadCreateRequest);
        }

        private RoadCreateRequest GetRoadCreateRequest()
        {
            return new RoadCreateRequest(RoadConstants.RoadId, RoadConstants.RoadSize, 20);
        }

        public void RoadCreated()
        {
            m_IsRoadCreated = true;
        }

        public void UpdateAnchorPosition(Vector3 position)
        {
            if (position.z - m_AnchorPosition.z > RoadConstants.RoadSize)
            {
                m_AnchorPosition = position;
                m_OnCurrentAnchorPositionUpdated.OnNext(m_AnchorPosition);
            }
        }
    }
}