using UnityEngine;

namespace Util
{
    public class Droppable : MonoBehaviour
    {
        public GameObject[] items;

        public void Drop()
        {
            foreach (var item in items)
            {
                Instantiate(item, transform.position, Quaternion.identity);
            }
        }
    }
}