using UnityEngine;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class AddOrDecreaseHealthAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Add or Decrease Health Action";

        public TriggeredActionSettings settings;
        TriggeredActionCachedValues cachedValues = new TriggeredActionCachedValues();

        [System.Serializable]
        public class AttributesAmountPair
        {
            public Attributes attributes;
            public string findByTag;
            public bool addInvincibilitySeconds;
            public bool respectInvincibility;
            public int amount;
        }

        public AttributesAmountPair[] targets;

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
            foreach (AttributesAmountPair target in targets)
            {
                if (target.findByTag != "")
                {
                    target.attributes = GameObject.FindGameObjectWithTag(target.findByTag).GetComponent<Attributes>();
                }
            }
        }

        public void Run(GameObject parent)
        {
            foreach (AttributesAmountPair target in targets)
            {
                if (target.respectInvincibility && target.attributes.invincible > 0)
                {
                    return;
                }

                target.attributes.health += target.amount;

                if (target.addInvincibilitySeconds)
                {
                    target.attributes.invincible = target.attributes.invincibilitySecondsOnHit;
                }
            }
        }

        public void Revert(GameObject parent)
        {

        }
    }
}