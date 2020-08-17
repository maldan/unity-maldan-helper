using UnityEngine;

namespace Animation
{
    public class ObjectPopOutAnimation : MonoBehaviour
    {
        // public float maxHeight;
        
        private Vector3 _currentPosition;
        private bool _isDown;
        private float _timer;
        private float _x;
        private float _z;
        
        void Start()
        {
            _currentPosition = transform.position;
            _x = Random.Range(-0.01f, 0.01f);
            _z = Random.Range(-0.01f, 0.01f);
        }

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > 0.2f)
            {
                _isDown = true;
            }
            
            if (_isDown)
            {
                transform.position += new Vector3(_x, -0.05f, _z);
            }
            else
            {
                transform.position += new Vector3(_x, 0.1f, _z);
            }
        }
    }
}