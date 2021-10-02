using UnityEngine;

namespace Fumiko.Common
{
    public class BasicMovementFollowTarget : BasicMovementModule
    {
        public BasicMovementModuleSettings settings;

        private Transform followTarget;
        private float currentInterval = -1;

        private Vector3 lastKnownFollowPosition;
        private Vector3 origin;

        public override Vector3 Calculate(Transform origin)
        {
            if (currentInterval == -1)
            {
                currentInterval = settings.updateInterval;
            }

            currentInterval += Time.fixedDeltaTime;

            if (currentInterval > settings.updateInterval)
            {
                if (!followTarget && settings.followTag.Length > 0)
                {
                    GameObject target = GameObject.FindGameObjectWithTag(settings.followTag);

                    if (target)
                    {
                        followTarget = target.transform;
                    }
                    else
                    {
                        return Vector3.zero;
                    }
                }

                lastKnownFollowPosition = followTarget.transform.position;
                currentInterval = 0;
            }

            if (!followTarget)
            {
                return Vector3.zero;
            }

            if (lastKnownFollowPosition != Vector3.zero)
            {
                if (Vector3.Distance(origin.position, lastKnownFollowPosition) > settings.followRestDistance)
                {
                    return Vector3.ClampMagnitude(lastKnownFollowPosition - origin.position, settings.strength);
                }
            }

            return Vector3.zero;
        }
    }
}