using UnityEngine;

using Fumiko.Common;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class ActivateLocalAreaPostProcessingAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Activate Local Area Post Processing Action";

        public TriggeredActionSettings settings;
        TriggeredActionCachedValues cachedValues = new TriggeredActionCachedValues();

        public GameObjectStatePair[] targets;

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
            foreach (GameObjectStatePair target in targets)
            {
                LocalAreaPostProcessing targetComponent = target.gameObject.GetComponent<LocalAreaPostProcessing>();

                if (target.state == EnabledState.ENABLED)
                {
                    targetComponent.activate();
                }

                if (target.state == EnabledState.DISABLED)
                {
                    targetComponent.deactivate();
                }
            }
        }

        public void Revert(GameObject parent)
        {
            foreach (GameObjectStatePair target in targets)
            {
                LocalAreaPostProcessing targetComponent = target.gameObject.GetComponent<LocalAreaPostProcessing>();

                if (target.state == EnabledState.ENABLED)
                {
                    targetComponent.deactivate();
                }

                if (target.state == EnabledState.DISABLED)
                {
                    targetComponent.activate();
                }
            }
        }
    }
}