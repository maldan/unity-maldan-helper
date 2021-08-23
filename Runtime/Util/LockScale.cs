using UnityEngine;

namespace Util
{
    public class LockScale : MonoBehaviour
    {
        public Vector3 Scale;
        
        private void Update()
        {
            transform.localScale = Scale;
        }
    }
}