using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Road.View
{
    public sealed class RoadContainerView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_RoadContainer;

        [SerializeField]
        private RoadSegmentViewsPool m_Pool;

        [Header("Config")]
        [SerializeField]
        private int m_SegmentCount;

        private IRoadService m_RoadService;
        private RoadViewCreator m_RoadViewCreator;

        private RoadCreateRequest m_CurrentRoadData;
        private int m_LastSegmentIndex;
        private readonly CompositeDisposable m_Disposable = new();

        private readonly List<RoadSegmentView> m_RoadSegmentViews = new();

        [Inject]
        private void Construct(IRoadService roadService, RoadViewCreator roadViewCreator)
        {
            m_RoadService = roadService;
            m_RoadViewCreator = roadViewCreator;
        }

        private void Awake()
        {
            m_RoadViewCreator.OnRoadCreateRequest.Subscribe(ProcessRoadCreateRequest).AddTo(m_Disposable);
            m_RoadService.OnRoadResetRequest.Subscribe(ProcessRoadCreateRequest).AddTo(m_Disposable);
            m_RoadService.OnCurrentAnchorPositionUpdated.Subscribe(_ => CreateNextSegment().Forget())
                .AddTo(m_Disposable);
        }

        private void ProcessRoadCreateRequest(RoadCreateRequest data)
        {
            m_LastSegmentIndex = 0;
            m_CurrentRoadData = data;
            
            RemoveAllOldSegments();
            CreateRoad().Forget();
        }

        private async UniTask CreateRoad()
        {
            await CreateStartSegments();
            PlaceStartSegments();
            m_RoadService.RoadCreated();
        }

        private async UniTask CreateStartSegments()
        {
            UniTask[] tasks = new UniTask[m_SegmentCount];

            for (int i = 0; i < m_SegmentCount; i++)
            {
                UniTask task = CreateSegment();
                tasks[i] = task;
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask CreateSegment()
        {
            RoadSegmentView segmentView = await m_Pool.GetSegmentViewOnlyPool();
            segmentView.transform.SetParent(m_RoadContainer);
            m_RoadSegmentViews.Add(segmentView);
        }

        private void PlaceStartSegments()
        {
            for (int i = 0; i < m_RoadSegmentViews.Count; i++)
            {
                RoadSegmentView segmentView = m_RoadSegmentViews[i];
                PlaceSegment(segmentView);
            }
        }

        private async UniTask CreateNextSegment()
        {
            RemoveOldSegments();
            await CreateSegment();

            RoadSegmentView segmentView = m_RoadSegmentViews[^1];
            PlaceSegment(segmentView);
        }

        private void RemoveAllOldSegments()
        {
            for (int i = m_RoadSegmentViews.Count - 1; i >= 0; i--)
            {
                RoadSegmentView oldSegmentView = m_RoadSegmentViews[i];
                m_RoadSegmentViews.RemoveAt(i);
                m_Pool.Release(oldSegmentView);
            }
        }

        private void RemoveOldSegments()
        {
            RoadSegmentView oldSegmentView = m_RoadSegmentViews[0];
            m_RoadSegmentViews.RemoveAt(0);
            m_Pool.Release(oldSegmentView);
        }

        private void PlaceSegment(RoadSegmentView segmentView)
        {
            float nextZ = m_LastSegmentIndex * m_CurrentRoadData.RoadSize;
            Vector3 localPosition = new Vector3(0, 0, nextZ);
            segmentView.transform.localPosition = localPosition;

            m_LastSegmentIndex++;
        }
    }
}