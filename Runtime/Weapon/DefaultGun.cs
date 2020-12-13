using System;
using Helper;
using UnityEngine;

namespace Weapon
{
    public class DefaultGun : MonoBehaviour
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public GameObject[] Muzzle;
        public GameObject Bullet;
        public GameObject User;
        public int BulletLayer;
        public bool IsInfinityAmmo;
        
        public float DamageMultiplier { get; set; } = 1f;
        public float DelayMultiplier { get; set; } = 1f;
        public float SpeedMultiplier { get; set; } = 1f;
        
        public float ShakeCameraPower { get; protected set; }
        
        public float Delay { get; protected set; }
        public float Damage { get; protected set; }
        public float Speed { get; protected set; }
        public float RepulsiveForce { get; protected set; }
        public Vector3 Scatter { get; protected set; }
        public float MaxFlashLightPower { get; set; } = 320f;
        
        // Ammo
        public int CurrentAmmo { get; set; }
        public int MaxAmmo { get; set; }
        public float AmmoPercentage => CurrentAmmo / (float)MaxAmmo;
        
        public bool UseBulletDirection;
       
        private float _delayTimer;
        private float _flashLightPower;
        
        // Events
        public Action<float> OnShot;
        public Action<GameObject, Vector3, Vector3> OnBulletDestroy;
        public Action<GameObject> OnBulletCreated;
        
        // State
        private bool _isReady;
        public bool IsShoot { get; protected set; }
        public Vector3 TargetPosition;
         
        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            Delay = 0.1f;
            Damage = 1f;
            Speed = 34f;
            MaxAmmo = CurrentAmmo = 100;
        }

        public virtual void RestoreAmmo(int amount)
        {
            CurrentAmmo += amount;
            if (CurrentAmmo >= MaxAmmo) CurrentAmmo = MaxAmmo;
        }
        
        public virtual void Shoot(Vector3 targetPosition)
        {
            TargetPosition = targetPosition;
            
            if (CurrentAmmo <= 0) return;
            
            if (_isReady)
            {
                _isReady = false;
                
                RealShoot(targetPosition + Scatter.Random());
                
                if (!IsInfinityAmmo) CurrentAmmo -= 1;
            }
        }

        public virtual void Stop()
        {
            IsShoot = false;
        }
        
        public virtual void RealShoot(Vector3 targetPosition)
        {
            IsShoot = true;
            
            // Create bullet
            var bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<DefaultBullet>().Init(targetPosition, Speed, Damage, RepulsiveForce, User, UseBulletDirection);
            bullet.GetComponent<DefaultBullet>().OnDestroy = OnBulletDestroy;
            
            // Set Bullet layer
            if (BulletLayer > 0) bullet.layer = BulletLayer; 
            
            // Ignore collision with user
            if (User) UnityEngine.Physics.IgnoreCollision(User.GetComponent<Collider>(), bullet.GetComponent<Collider>());
            
            OnShot?.Invoke(Damage);
            OnBulletCreated?.Invoke(bullet);
            
            // Light update
            _flashLightPower = MaxFlashLightPower;
            
            // Update muzzle
            foreach (var muzzle in Muzzle)
                muzzle.transform.localScale = Vector3.one;
        }

        public virtual void LightLogic()
        {
            if (!GetComponent<Light>()) return;
            if (_flashLightPower <= 0f) return;
            
            if (_flashLightPower <= 0.1f)
            {
                _flashLightPower = 0;
                GetComponent<Light>().intensity = 0;
                return;
            }
            GetComponent<Light>().intensity = _flashLightPower;
            
            _flashLightPower += (0 - _flashLightPower) / 6f;
        }

        private void ReloadLogic()
        {
            if (_isReady) return;
            
            _delayTimer += Time.deltaTime;
            if (_delayTimer >= Delay * DelayMultiplier)
            {
                _delayTimer = 0;
                _isReady = true;
            }
        }

        public void Load()
        {
            _delayTimer = Delay * DelayMultiplier + 1f;
            ReloadLogic();
        }
        
        public virtual void UpdateGun()
        {
            
        }
        
        private void Update()
        {
            UpdateGun();
            LightLogic();
            ReloadLogic();
            
            // Hide muzzle
            foreach (var muzzle in Muzzle)
                muzzle.transform.localScale += (Vector3.zero - muzzle.transform.localScale) / (0.05f / Time.deltaTime);
        }
    }
}