using UnityEngine;

namespace Animation
{
    public class Animation_Rotation : MonoBehaviour
    {
        public Vector3 Direction;
        
        [Tooltip("Rotation Degree Per Second")]
        public float RPS;
        
        private Vector3 _startRotation;
        
        void Start()
        {
            _startRotation = transform.localRotation.eulerAngles;
        }
        
        void Update()
        {
            _startRotation += Direction * RPS * Time.deltaTime;
            
            transform.localRotation = Quaternion.Euler(_startRotation);
        }
    }
}