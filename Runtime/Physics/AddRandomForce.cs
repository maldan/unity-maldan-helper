using UnityEngine;

namespace Physics
{
    public class AddRandomForce : MonoBehaviour
    {
        public Vector3 MinForce;
        public Vector3 MaxForce;
        public ForceMode ForceMode;
        
        private void Start()
        {
            GetComponent<Rigidbody>().AddForce(
                new Vector3(Random.Range(MinForce.x, MaxForce.x), Random.Range(MinForce.y, MaxForce.y), Random.Range(MinForce.z, MaxForce.z)), 
                ForceMode);    
        }
    }
}