namespace Features.Road
{
    public readonly struct RoadCreateRequest
    {
        public readonly int RoadId;
        public readonly int RoadSize;
        public readonly int RoadLength;

        public RoadCreateRequest(int roadId, int roadSize, int roadLength)
        {
            RoadId = roadId;
            RoadSize = roadSize;
            RoadLength = roadLength;
        }
    }
}