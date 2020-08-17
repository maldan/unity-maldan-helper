using System;
using UnityEngine;

namespace Danmaku.Util
{
    public class DanmakuSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private float delay;
        [SerializeField] private int maxAmount;
        [SerializeField] private int amountPerShot = 1;
        public float baseBulletSpeed;
        
        private float _timer;
        private int _bulletId;
        
        public bool isActive;
        public Action<GameObject, int> OnSpawn;
        public Action OnSpawnWave;
        
        void Update()
        {
            if (!isActive) return;
            _timer += Time.deltaTime;
            
            if (!(_timer > delay)) return;
            _timer = 0;

            if (maxAmount > 0 && _bulletId >= maxAmount)
            {
                return;
            }

            for (var i = 0; i < amountPerShot; i++)
            {
                var tempBullet = Instantiate(bullet, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                OnSpawn?.Invoke(tempBullet, _bulletId++);
            }
            
            OnSpawnWave?.Invoke();
        }
    }
}