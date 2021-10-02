using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fumiko.Common
{
    [ExecuteAlways]
    public class CreateEmpties : MonoBehaviour
    {
        [Header("ID will be determined by GameObject name.")]
        public EmptyLibrary emptyLibrary;

        private string id = "";

        void Start()
        {
#if UNITY_EDITOR
            if (id == "")
            {
                id = this.gameObject.name;
            }

            if (!emptyLibrary)
            {
                string[] libraries = AssetDatabase.FindAssets("DefaultEmptyLibrary");

                if (libraries.Length > 0)
                {
                    string atPath = AssetDatabase.GUIDToAssetPath(libraries[0]);
                    emptyLibrary = (EmptyLibrary)AssetDatabase.LoadAssetAtPath(atPath, typeof(EmptyLibrary));
                }
            }

            if (!emptyLibrary)
            {
                return;
            }

            List<string> children = new List<string>();

            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i).name);
            }

            EmptyLibrary.EmptyList emptyEntry = emptyLibrary.emptyLists.Find(x => x.id == id);

            if (emptyEntry != null)
            {
                for (int i = 0; i < emptyEntry.empties.Length; i++)
                {
                    GameObject empty;

                    if (!children.Contains(emptyEntry.empties[i]))
                    {
                        empty = new GameObject();

                        empty.transform.parent = this.gameObject.transform;

                        empty.transform.localPosition = Vector3.zero;
                        empty.transform.localRotation = Quaternion.identity;
                        empty.transform.localScale = Vector3.one;

                        empty.name = emptyEntry.empties[i];
                    }
                    else
                    {
                        empty = transform.Find(emptyEntry.empties[i]).gameObject;
                    }

                    for (int u = 0; u < emptyEntry.components.Length; u++)
                    {
                        if (!empty.GetComponent(emptyEntry.components[u].GetType()))
                        {
                            empty.AddComponent(emptyEntry.components[u].GetType());
                        }
                    }
                }
            }
#endif
        }
    }
}