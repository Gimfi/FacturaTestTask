namespace Features.Vehicles
{
    public readonly struct VehiclesCreateRequest
    {
        public readonly int VehicleId;

        public VehiclesCreateRequest(int vehicleId)
        {
            VehicleId = vehicleId;
        }
    }
}