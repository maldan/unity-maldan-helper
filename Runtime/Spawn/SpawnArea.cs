using Helper;
using UnityEngine;
using Util;

namespace Spawn
{
    public class SpawnArea : MonoBehaviour
    {
        //public Vector3[] areaPosition;
        //public Vector3[] areaSize;
        public GameObject item;
        public Color color;
        public int amount = 1;
        
        private void OnDrawGizmos()
        {
            /*var oldColor = Gizmos.color;
            Gizmos.color = color;
            for (var i = 0; i < areaPosition.Length; i++)
            {
                Gizmos.DrawWireCube(areaPosition[i], areaSize[i]);
            }
            Gizmos.color = oldColor;*/
            
            var oldColor = Gizmos.color;
            Gizmos.color = color;
            
            var children = gameObject.GetChilds("");
            foreach (var child in children)
            {
                Gizmos.DrawWireCube(child.transform.position, child.transform.localScale);
            }
            Gizmos.color = oldColor;
        }   

        private void Start()
        {
            var children = gameObject.GetChilds("");
            foreach (var child in children)
            {
                for (var i = 0; i < amount; i++)
                {
                    Instantiate(item, child.transform.position + child.transform.localScale.Random(), Quaternion.identity);
                }
            }
        }
    }
}