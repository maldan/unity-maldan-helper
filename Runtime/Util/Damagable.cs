using System;
using Animation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Util
{
    public class Damagable : ShaderPropertyAnimation
    {
        public float Health;
        public float MaxHealth;

        public float Armor;
        public float MaxArmor;
        public float ArmorDamageAbsorb = 0.3f;

        public float SelfDamagePerSecond;
        
        public float HitEffectTime = 0.3f;
        public Action OnDeath;
        public Action<float, GameObject> OnDamage;
		public Action<float, float, float, float> OnChange;
        
        public GameObject[] DamagableParts;

        private bool _isDead;
                       
        public float HealthPercentage
        {
            get
            {
                try
                {
                    return Health / MaxHealth;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public float ArmorPercentage
        {
            get
            {
                try
                {
                    return Armor / MaxArmor;
                }
                catch
                {
                    return 0;
                }
            }
        }
        
        public bool IsAutoDestroy;

        public void Start()
        {
            foreach (var part in DamagableParts)
            {
                part.AddComponent<ShaderPropertyAnimation>();
            }   
        }
		
		public void Init(float maxValue, float maxArmorValue)
        {
            Health = MaxHealth = maxValue;
            Armor = MaxArmor = maxArmorValue;
            OnChange?.Invoke(Health, MaxHealth, Armor, MaxArmor);
        }
        
        public void Kill()
        {
            Damage(MaxHealth * 2f);
        }
		
        public void Damage(float amount, GameObject byWhom = null)
        {
            if (_isDead) return;
            
            if (Armor > 0)
            {
                Armor -= amount;
                var reminder = 0f;
                if (Armor <= 0)
                {
                    reminder = Mathf.Abs(Armor);
                    Armor = 0;
                }
                Health -= (amount - reminder) * ArmorDamageAbsorb;
                Health -= reminder;
            }
            else
            {
                Health -= amount;
            }
            
            OnDamage?.Invoke(amount, byWhom);
            OnChange?.Invoke(Health, MaxHealth, Armor, MaxArmor);
               
            if (Health <= 0)
            {
                Health = 0;        
                if (IsAutoDestroy) Destroy(gameObject);
                OnDeath?.Invoke();
                
                // Drop items
                var drops = GetComponents<Droppable>();
                foreach (var drop in drops) drop.Drop();
                
                _isDead = true;
            }
            
            // Check if gameObject has Render to make animation
            if (GetComponent<Renderer>())
            {
                AnimateFloatProperty("Damage", 0, 1, HitEffectTime);
            }
            
            foreach (var part in DamagableParts)
            {
                part.GetComponent<ShaderPropertyAnimation>().AnimateFloatProperty("Damage", 0, 1, HitEffectTime);
            }
        }
        
        public void Restore(float amount)
        {
            Health += amount;
            if (Health >= MaxHealth) Health = MaxHealth;
            OnChange?.Invoke(Health, MaxHealth, Armor, MaxArmor);
        }

        public void RestoreArmor(float amount)
        {
            Armor += amount;
            if (Armor >= MaxArmor) Armor = MaxArmor;
            OnChange?.Invoke(Health, MaxHealth, Armor, MaxArmor);
        }
        
        /*public void RestorePercentage(float amount)
        {
            var partOfHealth = MaxHealth * amount;
            Restore(partOfHealth);
        }*/

        protected virtual void Update()
        {
            base.Update();
            if (SelfDamagePerSecond > 0)
            {
                Damage(SelfDamagePerSecond * Time.deltaTime);
            }
        }

        public static void Set(GameObject gameObject, int life, int armor = 0)
        {
            gameObject.AddComponent<Damagable>();
            gameObject.GetComponent<Damagable>().DamagableParts = new GameObject[0];
            gameObject.GetComponent<Damagable>().IsAutoDestroy = true;
            gameObject.GetComponent<Damagable>().Init(life, armor);
        }
    }
}