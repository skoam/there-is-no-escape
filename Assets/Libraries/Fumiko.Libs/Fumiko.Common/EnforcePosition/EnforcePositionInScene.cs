using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fumiko.Common
{
    [ExecuteAlways]
    public class EnforcePositionInScene : MonoBehaviour
    {
        [Header("ID will be determined by GameObject name.")]
        public EnforcedPositions positionLibrary;

        private string id = "";

        void Start()
        {
#if UNITY_EDITOR
            if (id == "")
            {
                id = this.gameObject.name;
            }

            if (!positionLibrary)
            {
                string[] libraries = AssetDatabase.FindAssets("DefaultPositionLibrary");

                if (libraries.Length > 0)
                {
                    string atPath = AssetDatabase.GUIDToAssetPath(libraries[0]);
                    positionLibrary = (EnforcedPositions)AssetDatabase.LoadAssetAtPath(atPath, typeof(EnforcedPositions));
                }

                return;
            }

            if (!positionLibrary)
            {
                return;
            }

            EnforcedPositions.EnforcedPosition positionEntry = positionLibrary.positions.Find(x => x.id == id);

            if (positionEntry != null)
            {
                this.transform.localPosition = positionEntry.position;
            }
#endif
        }
    }
}