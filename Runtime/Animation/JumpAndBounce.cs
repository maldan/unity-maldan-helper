using UnityEngine;

namespace Animation
{
    public class JumpAndBounce : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5f, 5f), Random.Range(8, 15f), Random.Range(-5f, 5f)), ForceMode.VelocityChange);
        }
    }
}