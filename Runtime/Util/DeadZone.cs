using UnityEngine;

namespace Util
{
    public class DeadZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Damagable>())
            {
                other.GetComponent<Damagable>().Kill();
            }
        }
    }
}