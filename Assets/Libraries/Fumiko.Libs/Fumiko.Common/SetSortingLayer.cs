using UnityEngine;

namespace Fumiko.Common
{
    public class SetSortingLayer : MonoBehaviour
    {
        public int OrderInLayer = 0;
        public string SortingLayer = "Default";

        private void Awake()
        {
            MeshRenderer mesh = GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                mesh.sortingLayerName = SortingLayer;
                mesh.sortingOrder = OrderInLayer;
            }

            SkinnedMeshRenderer skinnedMesh = GetComponent<SkinnedMeshRenderer>();
            if (skinnedMesh != null)
            {
                skinnedMesh.sortingLayerName = SortingLayer;
                skinnedMesh.sortingOrder = OrderInLayer;
            }
        }
    }
}