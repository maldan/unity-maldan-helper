using System;
using Helper;
using UnityEngine;

namespace Util
{
    public class Droppable : MonoBehaviour
    {
        public GameObject[] Items;
        public float Multiplier = 1;
        public Vector3 DropRadius;
        
        public void Drop()
        {
            var m = Math.Round(Multiplier);

            for (var i = 0; i < m; i++)
            {
                foreach (var item in Items)
                {
                    Instantiate(item, transform.position + DropRadius.Random(), Quaternion.identity);
                }
            }
        }
    }
}