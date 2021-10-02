using UnityEngine;

[ExecuteInEditMode]
public class ReapplyArmatureWithBones : MonoBehaviour
{
    public Transform newArmature;
    public Transform[] bonesToApply;

    [ContextMenu("Apply")]
    public void Reassign()
    {
        SkinnedMeshRenderer rend = gameObject.GetComponent<SkinnedMeshRenderer>();
        Transform[] bones = rend.bones;

        rend.rootBone = bonesToApply[0];

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i] = bonesToApply[i];
        }

        rend.bones = bones;
    }
}