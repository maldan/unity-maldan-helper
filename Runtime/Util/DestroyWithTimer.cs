using UnityEngine;

namespace Util
{
    public class DestroyWithTimer : MonoBehaviour
    {
        public float Delay;
        public float RandomTime;

        void Start()
        {
            Destroy(gameObject, Delay + Random.Range(0, RandomTime));
        }
    }
}