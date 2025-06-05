using System;
using UnityEngine;

namespace Features.Road
{
    public interface IRoadService
    {
        IObservable<RoadCreateRequest> OnRoadCreateRequest { get; }
        IObservable<RoadCreateRequest> OnRoadResetRequest { get; }
        IObservable<Vector3> OnCurrentAnchorPositionUpdated { get; }

        void Reset(Vector3 anchorPosition);
        void RoadCreated();
        void UpdateAnchorPosition(Vector3 position);
    }
}