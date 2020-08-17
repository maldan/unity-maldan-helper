using Helper;
using UnityEngine;

namespace Camera
{
    public class TopCamera : MonoBehaviour
    {
        public GameObject Target;
        public string TargetName;
        public float Speed = 10f;
        public float Smooth = 16f;
        public float OffsetY;
        public float OffsetZ;

        public LayerMask LayerMask;
        
        private float _shakePower;
        private Vector3 _point;
        public Vector3 RayHitPoint => _point;

        public void Shake(float power)
        {
            _shakePower = power;
        }
        
        private void FixedUpdate()
        {
            if (!Target)
            {
                if (TargetName != null)
                {
                    Target = GameObject.Find(TargetName);
                }
                return;
            }

            _shakePower += (0 - _shakePower) / 12f;
            
            transform.position +=
                ((Target.transform.position + new Vector3(0, OffsetY, OffsetZ) + new Vector3(_shakePower, _shakePower, _shakePower).Random()) - transform.position) / Smooth;
            
            RaycastHit hit;
            var ray = GetComponent<UnityEngine.Camera>().ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask)) {
                // Transform objectHit = hit.transform;
                
                _point = hit.point;

                // Do something with the object that was hit by the raycast.
            }
            
            // transform.position = Vector3.MoveTowards(transform.position, Target.transform.position + new Vector3(0, OffsetY, OffsetZ), Speed * Time.deltaTime);
            // transform.LookAt(Target.transform);
            
            //var lTargetDir = Target.transform.position - transform.position;
            // lTargetDir.y = 0.0f;
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Speed * Time.deltaTime);
        }
        
        private void OnDrawGizmos()
        {
            // Gizmos.DrawWireCube(_point, Vector3.one);
        }
    }
}