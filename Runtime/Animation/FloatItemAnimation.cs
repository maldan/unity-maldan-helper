using UnityEngine;

namespace Animation
{
    public class FloatItemAnimation : MonoBehaviour
    {
        public Vector3 offset;
        public Vector3 direction;
        public Vector3 rotationDirection;
        public Vector3 startRotation;
        public float speed = 4;

        private float _random;
        private Vector3 _currentPosition;
        
        void Start()
        {
            _random = Random.Range(0, 100f);
            _currentPosition = transform.localPosition;
            // transform.rotation = Quaternion.Euler(startRotation);
        }

        void Update()
        {
            transform.localPosition = _currentPosition + direction * Mathf.Sin(_random + Time.time * speed) * 0.2f + offset;
            // transform.Rotate(rotationDirection);
        }
    }
}