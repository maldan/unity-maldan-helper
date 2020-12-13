using UnityEngine;

namespace Animation
{
    public class FloatItemAnimation : MonoBehaviour
    {
        public Vector3 Offset;
        public Vector3 Direction;
        public Vector3 RotationDirection;
        public Vector3 StartRotation;
        public float Speed = 4;

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
            transform.localPosition = _currentPosition + Direction * Mathf.Sin(_random + Time.time * Speed) * 0.2f + Offset;
            transform.Rotate(RotationDirection);
        }
    }
}