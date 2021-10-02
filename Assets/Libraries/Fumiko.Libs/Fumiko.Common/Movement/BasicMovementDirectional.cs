using UnityEngine;

namespace Fumiko.Common
{
    public class BasicMovementDirectional : BasicMovementModule
    {
        public BasicMovementModuleSettings settings;

        public override Vector3 Calculate(Transform origin)
        {
            return settings.direction * settings.strength;
        }
    }
}