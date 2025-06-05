using System;
using Core.Asset;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Vehicles.View
{
    public sealed class VehiclesViewCreator : IInitializable
    {
        private readonly IVehiclesService m_Service;
        private readonly IAssetProvider m_AssetProvider;
        private readonly CompositeDisposable m_Disposable = new(); //never Dispose because exemplar never destroyed.

        private readonly Subject<VehicleView> m_OnVehicleViewLoaded = new();
        
        public VehiclesViewCreator(IVehiclesService service, IAssetProvider assetProvider)
        {
            m_Service = service;
            m_AssetProvider = assetProvider;
        }

        public IObservable<VehicleView> OnVehicleViewLoaded => m_OnVehicleViewLoaded;

        public void Initialize()
        {
            m_Service.OnVehicleCreateRequest.Subscribe(data => CreateVehicle(data).Forget()).AddTo(m_Disposable);
        }

        private async UniTask CreateVehicle(VehiclesCreateRequest data)
        {
            try
            {
                GameObject go = await m_AssetProvider.GetAssetAsync<GameObject>(VehiclesViewConstants.AssetName);
                if (go.TryGetComponent(out VehicleView roadSegmentView))
                    m_OnVehicleViewLoaded.OnNext(roadSegmentView);
                else
                    Debug.LogError("[VehiclesViewCreator]: cannot find VehiclesSegmentView component!!!");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}