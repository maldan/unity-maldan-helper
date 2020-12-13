using UnityEngine;

namespace Util
{
    public class LookAtTarget : MonoBehaviour
    {
        public GameObject Target;
        public string TargetName;
        public float Speed;
        public bool IsInstant;
        public bool IsInverseScale;
        
        private Vector3 _startScale;
        
        private void Start()
        {
            _startScale = transform.localScale;
            if (IsInverseScale)
            {
                _startScale.x *= -1;
                transform.localScale = _startScale;
            }
        }
        
        private void Update()
        {
            if (!Target)
            {
                if (TargetName != null) Target = GameObject.Find(TargetName);
                return;
            }
            
            
            if (IsInstant)
            {
                transform.LookAt(Target.transform);
            }
            else
            {
                var targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Speed * Time.deltaTime);
            }
        }
    }
}