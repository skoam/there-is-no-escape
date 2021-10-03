using UnityEngine;

using Fumiko.Sound;

namespace Fumiko.Interaction.Interactable.Actions
{
    public class PlaySoundAction : MonoBehaviour, ITriggeredAction
    {
        public string actionName = "Play Sound Action";

        public TriggeredActionSettings settings;
        TriggeredActionCachedValues cachedValues = new TriggeredActionCachedValues();

        public AudioSource targetAudioSource;
        public string findByTag;
        public AudioClip sound;
        public float level = 1;

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
            if (!targetAudioSource)
            {
                GameObject sourceGameObject = GameObject.FindGameObjectWithTag(findByTag);

                if (sourceGameObject)
                {
                    targetAudioSource = sourceGameObject.GetComponent<AudioSource>();
                }
            }
        }

        public void Run(GameObject parent)
        {
            if (targetAudioSource)
            {
                targetAudioSource.PlayOneShot(sound, level);
            }
        }

        public void Revert(GameObject parent)
        {

        }
    }
}