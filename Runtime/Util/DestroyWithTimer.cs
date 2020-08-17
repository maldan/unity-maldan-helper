using UnityEngine;

namespace Util
{
    public class DestroyWithTimer : MonoBehaviour
    {
        public float delay;

        void Start()
        {
            Destroy(gameObject, delay);
        }
    }
}