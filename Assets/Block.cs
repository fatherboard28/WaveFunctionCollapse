using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    public class Block : MonoBehaviour
    {
        private BlockData _blockData; 
        
        public int Entropy;
        public List<BlockData> PotentialBlocks;
        public int X, Y;

        public BlockData GetBlockData() { return _blockData; }


        public void InitializeBlock(MapRules mapRules, int x, int y)
        {
            foreach (BlockAdjacentPairRule pair in mapRules.Rules)
            {
                PotentialBlocks.Add(pair.Block);
            }
            Entropy = PotentialBlocks.Count;
            X = x; Y = y;
        }

        //Returns true if entropy > 1, false if entropy == 1
        public void CollapseBlock()
        {
            int i = 0;
            int rand = Random.Range(0, PotentialBlocks.Count + 1);
            foreach (BlockData block in PotentialBlocks)
            {
                if (i == rand)
                {
                    _blockData = block;
                }
                i++;
            }

            Entropy = 1;
        }

        public bool IsBlockSet()
        {
            return Entropy == 1;
        }
    }
}

