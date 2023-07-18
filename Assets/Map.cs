using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WFC
{
    public class Map : MonoBehaviour
    {
        //Private
        Block[,] _map;
        Queue<Block> _blockQ;
        

        //public
        public int SizeX, SizeY;
        public MapRules MapRules;
        public GameObject BlocksGameObjectParent;


        //Methods
        public void CreateMap()
        {
            if (BlocksGameObjectParent == null)
            {
                BlocksGameObjectParent = new GameObject("Blocks Parent");
            }

            _map = new Block[SizeX, SizeY];

            for(int i = 0; i < SizeX; i++)
                for(int j = 0; j < SizeY; j++)
                {
                    GameObject tmp = new GameObject("Block");
                    tmp.transform.parent = BlocksGameObjectParent.transform;
                    _map[i, j] = tmp.AddComponent<Block>();
                    _map[i, j].InitializeBlock(MapRules, i, j);
                }
        }

        private void Awake()
        {
            CreateMap();
            CollapseBlocks();
        }
        public void CollapseBlocks()
        {
            _blockQ = new Queue<Block>();
            List<Block> remainingHighestEntropies = RemainingHighestEntropies();
            Debug.Log(remainingHighestEntropies.Count);
            while (remainingHighestEntropies.Count > 0)
            {
                int i = 0;
                int rand = Random.Range(0, remainingHighestEntropies.Count + 1);
                foreach (Block b in remainingHighestEntropies)
                {
                    if (i == rand)
                    {
                        b.CollapseBlock();
                        AddToQueue(b);
                    }
                    i++;
                }

                while (_blockQ.Count > 0)
                {
                    Block tmp = _blockQ.Dequeue();
                    bool wasChanged = UpdatePotentials(tmp);
                    if (wasChanged)
                    {
                        AddToQueue(tmp);
                    }
                }

                remainingHighestEntropies = RemainingHighestEntropies();
            }
        }

        public List<Block> RemainingHighestEntropies()
        {
            List<Block> blocks = new List<Block>();
            int hightestEntropy = 1;

            for(int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    if (_map[i, j].Entropy >= hightestEntropy)
                    {
                        if (_map[i, j].Entropy > hightestEntropy)
                        {
                            hightestEntropy = _map[i, j].Entropy;
                            blocks.Clear();
                        }
                        blocks.Add(_map[i, j]);
                    }
                }

            return blocks;
        }

        public void AddToQueue(Block block)
        {
            int x = block.X; int y = block.Y;
            if (x > 0)
                _blockQ.Enqueue(_map[x - 1, y]);
            if (x < SizeX-1)
                _blockQ.Enqueue(_map[x + 1, y]);
            if (y > 0)
                _blockQ.Enqueue(_map[x, y - 1]);
            if (y < SizeY-1)
                _blockQ.Enqueue(_map[x, y + 1]);
        }


        //false means no update happened, true means update happened
        public bool UpdatePotentials(Block block)
        {
            if (block.Entropy == 1)
            {
                return false;
            }

            List<List<BlockData>> datasToIntersect = new List<List<BlockData>>();
            if (block.X > 0)
                datasToIntersect.Add(_map[block.X - 1, block.Y].PotentialBlocks);
            if (block.X < SizeX-1)
                datasToIntersect.Add(_map[block.X + 1, block.Y].PotentialBlocks);
            if (block.Y > 0)
                datasToIntersect.Add(_map[block.X, block.Y - 1].PotentialBlocks);
            if (block.Y < SizeY-1)
                datasToIntersect.Add(_map[block.X, block.Y + 1].PotentialBlocks);

            List<BlockData> tmp = new List<BlockData>();
            int i = 0; 
            foreach (List<BlockData> bd in datasToIntersect)
            {
                if (i == 0)
                {
                    tmp = bd;
                }
                else
                {
                    tmp = tmp.Intersect(bd).ToList();
                }
                i++;
            }

            if (block.PotentialBlocks == tmp)
            {
                return false;
            }

            block.PotentialBlocks = tmp;
            block.Entropy = block.PotentialBlocks.Count;
            return true;
        }
    }
}

