using UnityEngine;

namespace VFX
{
    public class FlashLight : MonoBehaviour
    {
        private Light _light;

        private void Start()
        {
            _light = GetComponent<Light>();
        }

        private void Update()
        {
            _light.intensity += (0 - _light.intensity) / 12f;
            if (_light.intensity <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}