using Helper;
using UnityEngine;

namespace Camera
{
    public class TopCamera : MonoBehaviour
    {
        public GameObject Target;
        public string TargetName;
        public Vector3 TargetOffset;
        public float Speed = 10f;
        public float Smooth = 16f;
        public float OffsetY;
        public float OffsetZ;
        public float Zoom = 1f;

        public LayerMask LayerMask;
        
        private float _shakePowerX;
        private float _shakePowerY;
        private float _shakePowerZ;

        private Vector3 _finalPosition;
        private float _finalSmoothRate = 1f;
        
        private Vector3 _point;
        private Vector3 _pointWithoutCollision;
        public Vector3 RayHitPoint => _point;
        public Vector3 RayHitPointWithoutCollision => _pointWithoutCollision;
        public Vector3 RayHitPointAny => IsRayHit ?_point :_pointWithoutCollision;
        public Vector3 RayHitDirection;
        public Vector3 RayHitNormal;
        public bool IsRayHit { get; private set; }

        private void Start()
        {
            _finalPosition = transform.position;
        }

        public void ShakeByDistance(Vector3 from, Vector3 to, float maxDistance, float power)
        {
            var distancePower = 1f - Mathf.Clamp(Vector3.Distance(from, to), 0, maxDistance) / maxDistance;
            Shake(distancePower * power);
        }
        
        public void ShakeByDistanceToTarget(Vector3 to, float maxDistance, float power)
        {
            if (!Target) return;
            
            var from = Target.transform.position;
            var distancePower = 1f - Mathf.Clamp(Vector3.Distance(from, to), 0, maxDistance) / maxDistance;
            Shake(distancePower * power);
        }
        
        public void Shake(float power)
        {
            _shakePowerX += power;
            _shakePowerY += power;
            _shakePowerZ += power;

            if (_shakePowerX > 100) _shakePowerX = 100f;
            if (_shakePowerY > 100) _shakePowerY = 100f;
            if (_shakePowerZ > 100) _shakePowerZ = 100f;
        }
         
        public void ShakeY(float power)
        {
            _shakePowerY += power;
        }
        
        private void FixedUpdate()
        {
            if (!Target)
            {
                if (TargetName != null) Target = GameObject.Find(TargetName);
                
                return;
            }

            _shakePowerX += (0 - _shakePowerX) / 12f;
            _shakePowerY += (0 - _shakePowerY) / 12f;
            _shakePowerZ += (0 - _shakePowerZ) / 12f;
            
            // If camera has rigid body
            /*if (GetComponent<Rigidbody>())
            {
                _finalPosition +=
                    ((Target.transform.position + TargetOffset + new Vector3(0, OffsetY * Zoom, OffsetZ * Zoom) + new Vector3(_shakePowerX, _shakePowerY, _shakePowerZ).Random()) - transform.position) / Smooth;
                GetComponent<Rigidbody>().MovePosition(_finalPosition);
            }
            else*/
            //{
                transform.position +=
                    ((Target.transform.position + TargetOffset + new Vector3(0, OffsetY * Zoom, OffsetZ * Zoom) + new Vector3(_shakePowerX, _shakePowerY, _shakePowerZ).Random()) - transform.position) / _finalSmoothRate;
            //}

            // Check raycast from camera
            IsRayHit = false;
            RaycastHit hit;
            var ray = GetComponent<UnityEngine.Camera>().ScreenPointToRay(Input.mousePosition);
            if (UnityEngine.Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask)) {
                _point = hit.point;
                RayHitNormal = hit.normal;
                IsRayHit = true;
            }
            _pointWithoutCollision = ray.origin + ray.direction * 100f;

            RayHitDirection = (RayHitPointAny - transform.position).normalized;

            // transform.position = Vector3.MoveTowards(transform.position, Target.transform.position + new Vector3(0, OffsetY, OffsetZ), Speed * Time.deltaTime);
            // transform.LookAt(Target.transform);

            //var lTargetDir = Target.transform.position - transform.position;
            // lTargetDir.y = 0.0f;
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Speed * Time.deltaTime);
            
            _finalSmoothRate = Smooth / Time.deltaTime;
        }
        
        private void OnDrawGizmos()
        {
            // Gizmos.DrawWireCube(_point, Vector3.one);
        }
    }
}