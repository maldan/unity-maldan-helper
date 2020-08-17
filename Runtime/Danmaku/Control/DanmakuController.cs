using UnityEngine;

namespace Danmaku.Control
{
    public class DanmakuController : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private float moveSpeed;
        
        private float _timer;
        private float _xPower;
        private float _zPower;
             
        void Start()
        {
            
        }
        
        void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A))
            {
                //gameObject.transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
                _xPower += (-moveSpeed - _xPower) / 8;
            } else if (Input.GetKey(KeyCode.D))
            {
               // gameObject.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
               _xPower += (moveSpeed - _xPower) / 8;
            }
            else
            {
                _xPower += (0 - _xPower) / 8;
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                _zPower += (moveSpeed - _zPower) / 8;
                //gameObject.transform.position += new Vector3(0, 0, moveSpeed * Time.deltaTime);
            } else
            
            if (Input.GetKey(KeyCode.S))
            {
                _zPower += (-moveSpeed - _zPower) / 8;
                //gameObject.transform.position += new Vector3(0, 0, -moveSpeed * Time.deltaTime);
            }
            else
            {
                _zPower += (0 - _zPower) / 8;
            }
            
            // gameObject.transform.position += new Vector3(_xPower * Time.deltaTime, 0, _zPower * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(_xPower * Time.deltaTime, 0, _zPower * Time.fixedDeltaTime));
            
            // Mouse down
            if (Input.GetMouseButton(0))
            {
                _timer += Time.deltaTime;
                if (_timer > 0.05f)
                {
                    _timer = 0;
                    var tempBullet = Instantiate(bullet, gameObject.transform.position + new Vector3(0, 0, 0.5f), Quaternion.identity);
                    
                    // Default layer
                    tempBullet.layer = 0;
                }
            }
        }
    }
}