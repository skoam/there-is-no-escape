using UnityEngine;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class EnableMovementAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Enable Movement Action";

        public SimpleCameraController cameraController;
        public Transform teleportLocation;

        public TriggeredActionSettings settings;
        TriggeredActionCachedValues cachedValues = new TriggeredActionCachedValues();

        public string ActionName
        {
            get
            {
                return actionName;
            }
        }

        public TriggeredActionSettings Settings
        {
            get
            {
                return settings;
            }
        }

        public TriggeredActionCachedValues CachedValues
        {
            get
            {
                return cachedValues;
            }
        }

        public void Initialize(GameObject parent)
        {

        }

        public void Run(GameObject parent)
        {
            cameraController.transform.position = teleportLocation.position;
            cameraController.m_TargetCameraState.ApplyCurrentPosition();
            cameraController.m_InterpolatingCameraState.ApplyCurrentPosition();
            cameraController.movementDisabled = false;
        }

        public void Revert(GameObject parent)
        {

        }
    }
}