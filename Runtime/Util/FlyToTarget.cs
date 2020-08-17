using UnityEngine;

namespace Util
{
    public class FlyToTarget : MonoBehaviour
    {
        public GameObject Target;
        public Vector3 TargetPosition;
        public string TargetName;
        public float Speed;
        public bool UseTargetPosition;
        public bool IsDestroyAtDestination;
        public bool IsLookAtTarget;
        public float RandomizeSpeed;

        private void Start()
        {
            Speed += Random.Range(-RandomizeSpeed, RandomizeSpeed);
        }

        private void Update()
        {
            if (UseTargetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);
                if (IsLookAtTarget)
                {
                    transform.LookAt(TargetPosition);
                }

                if (IsDestroyAtDestination && Vector3.Distance(transform.position, TargetPosition) < 0.1f)
                {
                    Destroy(gameObject);
                }
                
                return;
            }
            
            if (!Target)
            {
                if (TargetName != null)
                {
                    Target = GameObject.Find(TargetName);
                }
                return;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);
        }
    }
}