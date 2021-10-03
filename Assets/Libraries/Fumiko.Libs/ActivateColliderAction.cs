using UnityEngine;

using Fumiko.Common;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class ActivateColliderAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Activate Collider Action";

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
                Collider targetComponent = target.gameObject.GetComponent<Collider>();
                Collider2D targetComponent2D = target.gameObject.GetComponent<Collider2D>();

                if (target.state == EnabledState.ENABLED)
                {
                    if (targetComponent)
                    {
                        targetComponent.enabled = true;
                    }

                    if (targetComponent2D)
                    {
                        targetComponent2D.enabled = true;
                    }
                }

                if (target.state == EnabledState.DISABLED)
                {
                    if (targetComponent)
                    {
                        targetComponent.enabled = false;
                    }

                    if (targetComponent2D)
                    {
                        targetComponent2D.enabled = false;
                    }
                }
            }
        }

        public void Revert(GameObject parent)
        {
            foreach (GameObjectStatePair target in targets)
            {
                Collider targetComponent = target.gameObject.GetComponent<Collider>();
                Collider2D targetComponent2D = target.gameObject.GetComponent<Collider2D>();

                if (target.state == EnabledState.ENABLED)
                {
                    if (targetComponent)
                    {
                        targetComponent.enabled = false;
                    }

                    if (targetComponent2D)
                    {
                        targetComponent2D.enabled = false;
                    }
                }

                if (target.state == EnabledState.DISABLED)
                {
                    if (targetComponent)
                    {
                        targetComponent.enabled = true;
                    }

                    if (targetComponent2D)
                    {
                        targetComponent2D.enabled = true;
                    }
                }
            }
        }
    }
}