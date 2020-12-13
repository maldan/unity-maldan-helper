using UnityEngine;

namespace VFX
{
    public class FlashLight : MonoBehaviour
    {
        private Light _light;
        public float Smooth = 1f;

        private void Start()
        {
            _light = GetComponent<Light>();
        }

        private void Update()
        {
            _light.intensity += (0 - _light.intensity) / (Smooth / Time.deltaTime);
            if (_light.intensity <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}