using UnityEngine;

namespace Fumiko.Common
{
    [System.Serializable]
    public class BasicMovementModuleSettings
    {
        [Info("OPTIONAL")]
        public string description;
        public BasicMovementType type;

        [Info("GENERAL")]
        public float strength;
        public Vector3 direction;

        [Info("SIN")]
        public float speed;
        public bool bounce;
        [Range(0, 360)]
        public float currentCirclePosition = 0;

        [Info("FOLLOW_TARGET")]
        public string followTag;
        public float updateInterval = 1;
        public float followRestDistance = 0.5f;
    }
}