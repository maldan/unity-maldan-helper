using System;
using Helper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    [Serializable]
    public class DropItemInfo
    {
        public GameObject Item;
        [Range(0, 1)]
        public float Probability;
        public Vector2 Amount;
    }
    
    public class Droppable : MonoBehaviour
    {
        public DropItemInfo[] Items;
        public float Multiplier = 1;
        public Vector3 DropRadius;
        
        public void Drop()
        {
            /*var m = Math.Round(Multiplier);

            for (var i = 0; i < m; i++)
            {
                foreach (var item in Items)
                {
                    Instantiate(item, transform.position + DropRadius.Random(), Quaternion.identity);
                }
            }*/

            foreach (var item in Items)
            {
                if (Random.Range(0f, 1f) > 1 - item.Probability)
                {
                    var amount = Math.Round(Random.Range(item.Amount.x, item.Amount.y) * Multiplier);
                    for (var i = 0; i < amount; i++) Instantiate(item.Item, transform.position + DropRadius.Random(), Quaternion.identity);
                }
            }
        }
    }
}