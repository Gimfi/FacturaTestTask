using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Road.View
{
    public sealed class RoadSegmentViewsPool : MonoBehaviour
    {
        [SerializeField]
        private int m_PoolSize;

        [SerializeField]
        private Transform m_PoolContainer;

        private RoadViewCreator m_RoadViewCreator;
        private RoadSegmentView m_CurrentSegmentAsset;
        private IFactory<RoadSegmentView, Transform, UniTask<RoadSegmentView>> m_RoadSegmentViewFactory;
        private readonly CompositeDisposable m_Disposable = new();

        private readonly List<RoadSegmentView> m_InUseList = new();
        private readonly List<RoadSegmentView> m_PoolList = new();

        [Inject]
        private void Construct(RoadViewCreator roadViewCreator,
            IFactory<RoadSegmentView, Transform, UniTask<RoadSegmentView>> roadSegmentViewFactory)
        {
            m_RoadViewCreator = roadViewCreator;
            m_RoadSegmentViewFactory = roadSegmentViewFactory;
        }

        private void Awake()
        {
            m_RoadViewCreator.OnRoadSegmentViewLoaded.Subscribe(ProcessRoadSegmentViewLoaded).AddTo(m_Disposable);
        }

        private void OnDestroy()
        {
            m_Disposable.Dispose();
        }

        private void ProcessRoadSegmentViewLoaded(RoadSegmentView segmentAsset)
        {
            m_CurrentSegmentAsset = segmentAsset;
            CreateFreeViews().Forget();
        }

        private async UniTask CreateFreeViews()
        {
            UniTask<RoadSegmentView>[] tasks = new UniTask<RoadSegmentView>[m_PoolSize];

            for (int i = 0; i < m_PoolSize; i++)
            {
                UniTask<RoadSegmentView> task = m_RoadSegmentViewFactory.Create(m_CurrentSegmentAsset, m_PoolContainer);
                tasks[i] = task;
            }

            RoadSegmentView[] segments = await UniTask.WhenAll(tasks);
            m_PoolList.AddRange(segments);
        }

        public async UniTask<RoadSegmentView> GetSegmentView()
        {
            RoadSegmentView result;

            if (m_PoolList.Count > 0)
            {
                result = m_PoolList[0];
                m_PoolList.RemoveAt(0);
            }
            else
            {
                result = await m_RoadSegmentViewFactory.Create(m_CurrentSegmentAsset, m_PoolContainer);
            }

            m_InUseList.Add(result);
            return result;
        }

        public async UniTask<RoadSegmentView> GetSegmentViewOnlyPool()
        {
            if (m_PoolList.Count == 0)
                await UniTask.WaitUntil(() => m_PoolList.Count > 0);

            RoadSegmentView result = m_PoolList[0];
            m_PoolList.RemoveAt(0);
            m_InUseList.Add(result);

            return result;
        }

        public void Release(RoadSegmentView segmentView)
        {
            if (TryRemoveFromInUseList(segmentView))
                ReturnToPool(segmentView);
        }

        private bool TryRemoveFromInUseList(RoadSegmentView segmentView)
        {
            if (m_InUseList.Contains(segmentView))
            {
                m_InUseList.Remove(segmentView);
                return true;
            }

            return false;
        }

        private void ReturnToPool(RoadSegmentView segmentView)
        {
            segmentView.Release();
            segmentView.transform.SetParent(m_PoolContainer);
            m_PoolList.Add(segmentView);
        }
    }
}