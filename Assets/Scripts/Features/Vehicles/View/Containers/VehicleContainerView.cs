using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Vehicles.View
{
    public sealed class VehicleContainerView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_VehicleContainer;

        private IVehiclesService m_VehiclesService;
        private VehiclesViewCreator m_VehiclesViewCreator;
        private VehicleView m_VehicleAsset;
        private VehicleView m_CurrentVehicle;
        private IFactory<VehicleView, Transform, UniTask<VehicleView>> m_VehiclesViewFactory;
        private readonly CompositeDisposable m_Disposable = new();

        private readonly ReactiveCommand<GameObject> m_OnNewVehicleSpawned = new();
        public IObservable<GameObject> OnNewVehicleSpawned => m_OnNewVehicleSpawned;

        [Inject]
        private void Construct(IVehiclesService vehiclesService, VehiclesViewCreator vehiclesViewCreator,
            IFactory<VehicleView, Transform, UniTask<VehicleView>> vehiclesViewFactory)
        {
            m_VehiclesService = vehiclesService;
            m_VehiclesViewCreator = vehiclesViewCreator;
            m_VehiclesViewFactory = vehiclesViewFactory;
        }

        private void Awake()
        {
            m_VehiclesViewCreator.OnVehicleViewLoaded.Subscribe(ProcessVehicleCreateRequest).AddTo(m_Disposable);
            m_VehiclesService.OnPositionUpdated.Subscribe(UpdateVehiclePosition).AddTo(m_Disposable);
        }

        private void ProcessVehicleCreateRequest(VehicleView vehicleAsset)
        {
            m_VehicleAsset = vehicleAsset;
            CreateVehicle().Forget();
        }

        private async UniTask CreateVehicle()
        {
            m_CurrentVehicle = await m_VehiclesViewFactory.Create(m_VehicleAsset, m_VehicleContainer);
            m_VehiclesService.VehicleCreated();
            m_OnNewVehicleSpawned.Execute(m_CurrentVehicle.gameObject);
        }

        private void UpdateVehiclePosition(Vector3 position)
        {
            if (m_CurrentVehicle)
                m_CurrentVehicle.transform.localPosition = position;
        }
    }
}