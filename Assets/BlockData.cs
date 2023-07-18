using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [CreateAssetMenu]
    public class BlockData : ScriptableObject
    {
        public int ID;
        public GameObject Prefab;
    }
}
