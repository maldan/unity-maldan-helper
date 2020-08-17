using UnityEngine;

namespace ThirdPlayer.Camera
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public GameObject target;
        public bool isAuto;
        public bool isSmooth;
        
        public float angleX;
        public float angleY;

        public Vector3 direction;
        
        private bool _isLocked = true;
        
        void Start()
        {
            
        }

        private void OnDrawGizmos()
        {
            
            
            // Gizmos.DrawLine(transform.position, ra.point);
        }

        private void LateUpdate()
        {
            var ray = UnityEngine.Camera.main.ViewportPointToRay (new Vector3(0.5f,0.5f,0f));   
            direction = ray.direction;
            
            if (!target)
            {
                return;
            }
            
            transform.LookAt(target.transform.position + new Vector3(0, 4f, 0));
            // var lTargetDir = (target.transform.position + new Vector3(0, 2f, 0)) - transform.position;
            // lTargetDir.y = 1.0f;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.deltaTime * 150f);
        }

        void FixedUpdate()
        {
            if (!target)
            {
                return;
            }
            
            // transform.LookAt(target.transform.position + new Vector3(0, 2f, 0));

            if (_isLocked)
            {
                if (isAuto)
                {
                    var gas = target.transform.position + target.transform.rotation * new Vector3(0, 4, -12f);
                    transform.position += (gas - transform.position) / 8f;
                }
                else
                {
                    angleX += Input.GetAxis("Mouse X") * 500f * Time.fixedDeltaTime;
                    angleY -= Input.GetAxis("Mouse Y") * 160f * Time.fixedDeltaTime;
                    angleY = Mathf.Clamp(angleY, 0, 12);

                    var gagas = -12f;
                    /*if (angleY < 3)
                    {
                        gagas += 3 - angleY;
                    }*/

                    var gas = target.transform.position + new Vector3(0, angleY, 0)
                                                        + Quaternion.AngleAxis(angleX, Vector3.up) *
                                                        new Vector3(0, 0, gagas);
                    if (isSmooth)
                    {
                        transform.position += (gas - transform.position) / 8f;
                    }
                    else
                    {
                        transform.position += (gas - transform.position) / 2f;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isLocked = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _isLocked = true;
            }
            
            if (_isLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }
}