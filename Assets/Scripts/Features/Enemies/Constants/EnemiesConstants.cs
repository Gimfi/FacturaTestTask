using UnityEngine;

namespace Features.Enemies
{
    public static class EnemiesConstants
    {
        static EnemiesConstants()
        {
            ValidSegment = new Vector3(20, 0, 300);
            SpawnSegment = new Vector3(ValidSegment.x, ValidSegment.y, ValidSegment.z / 5);
            SpawnSegmentShift = new Vector3(0, 0, (ValidSegment.z / 2) - (SpawnSegment.z / 2));
        }

        public const int EnemiesPerSpawnCount = 5;
        public static readonly Vector3 ValidSegment;
        public static readonly Vector3 SpawnSegmentShift;
        public static readonly Vector3 SpawnSegment;
        
        public static class EnemiesParams
        {
            public const int Hp = 6;
            public const int Speed = 5;
            public const float AggressionRange = 800;
        }
    }
}