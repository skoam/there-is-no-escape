using UnityEngine;

namespace Fumiko.Procedural
{
    [System.Serializable]
    public class ProceduralLimb
    {
        public ProceduralLimbType type;
        public int[] bonesFromLibrary;
        public ProceduralBoneAlgorithmType algorithmType;
        public int count = 1;
        public int minLength = 5;
        public int maxLength = 10;
        public int chanceForSplit = 0;
        public int minSplitSize = 0;
        public int maxSplitSize = 0;
        public int algorithmDirectionChanges = 2;
        public int[] splitsFromLibrary;
        public Vector3 offset;
    }
}