using UnityEngine;

namespace Util
{
    public class LockRotation : MonoBehaviour
    {
        public Vector3 Rotation;
        
        private void Update()
        {
            transform.rotation = Quaternion.Euler(Rotation);
        }
    }
}