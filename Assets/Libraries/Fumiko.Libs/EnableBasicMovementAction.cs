using UnityEngine;

using Fumiko.Common;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class EnableBasicMovementAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Enable Basic Movement Action";

        public BasicMovement basicMovement;
        public bool disable;

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
            basicMovement.disabled = disable;
        }

        public void Revert(GameObject parent)
        {

        }
    }
}