using UnityEngine;

namespace Fumiko.Common
{
    public class BasicMovementSin : BasicMovementModule
    {
        public BasicMovementModuleSettings settings;

        private float current;

        public override Vector3 Calculate(Transform origin)
        {
            if (settings.currentCirclePosition < 360)
            {
                settings.currentCirclePosition += settings.speed * Time.fixedDeltaTime;
            }
            else
            {
                settings.currentCirclePosition = 0;
            }

            current = Mathf.Sin(settings.currentCirclePosition);

            if (settings.bounce)
            {
                current = Mathf.Abs(current);
            }

            return settings.direction * current * settings.strength;
        }
    }
}