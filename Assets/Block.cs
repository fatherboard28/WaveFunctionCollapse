using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Block : MonoBehaviour
    {
        private BlockData _blockData; 
        
        public int Entropy;
        public List<BlockData> PotentialBlocks;
        public int X, Y;

        public BlockData GetBlockData() { return _blockData; }


        public void InitializeBlock(MapRules mapRules, int x, int y)
        {
            PotentialBlocks = new List<BlockData>();
            foreach (BlockAdjacentPairRule pair in mapRules.Rules)
            {
                PotentialBlocks.Add(pair.Block);
            }
            Entropy = PotentialBlocks.Count;
            X = x; Y = y;
            Vector3 pos = new Vector3(X*2, Y*2, 0);
            gameObject.transform.position = pos;
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
                    gameObject.GetComponent<MeshRenderer>().sharedMaterials = _blockData.Prefab.GetComponent<MeshRenderer>().sharedMaterials;
                    gameObject.GetComponent<MeshFilter>().sharedMesh = _blockData.Prefab.GetComponent<MeshFilter>().sharedMesh;
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

