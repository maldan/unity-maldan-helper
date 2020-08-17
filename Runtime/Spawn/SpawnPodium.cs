using Animation;
using Helper;
using UnityEngine;

namespace Spawn
{
    public class SpawnPodium : MonoBehaviour
    {
        public GameObject item;
        public float delay;

        private GameObject _item;
        private float _timer;

        private void Start()
        {
            Spawn();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= delay)
            {
                _timer = 0;

                if (!_item)
                {
                    Spawn();
                }
            }
        }

        private void Spawn()
        {
            _item = Instantiate(item, transform.position, Quaternion.identity);
            _item.AddComponent<FloatItemAnimation>();
            _item.GetComponent<FloatItemAnimation>().offset = new Vector3(0, 1.2f, 0);
            _item.GetComponent<FloatItemAnimation>().rotationDirection = new Vector3(0, 1.2f, 0);
        }
    }
}