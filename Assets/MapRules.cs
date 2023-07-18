using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [CreateAssetMenu]
    public class BlockAdjacentPairRule : ScriptableObject
    {
        public BlockData Block;
        public List<BlockData> ValidAdjacentBlocks;
    }

    [CreateAssetMenu]
    public class MapRules : ScriptableObject
    {
        public BlockAdjacentPairRule[] Rules;
    }
}
