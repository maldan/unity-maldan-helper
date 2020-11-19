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
        public float HitEffectTime = 0.3f;
        public Action OnDeath;
        public Action<float> OnDamage;
		public Action<float, float> onChange;
       
        public GameObject[] DamagableParts;        
        public float Percentage
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
        public bool IsAutoDestroy;

        public void Start()
        {
            foreach (var part in DamagableParts)
            {
                part.AddComponent<ShaderPropertyAnimation>();
            }   
        }
		
		public void Init(float maxValue)
        {
            Health = MaxHealth = maxValue;
			onChange?.Invoke(health, maxHealth);
        }
        
        public void Damage(float amount)
        {
            Health -= amount;
            OnDamage?.Invoke(amount);
            onChange?.Invoke(Health, MaxHealth);
            
            if (Health <= 0)
            {
                Health = 0;
                OnDeath?.Invoke();
                
                if (IsAutoDestroy)
                {
                    Destroy(gameObject);
                }
                
                // Drop items
                GetComponent<Droppable>()?.Drop();
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
            
            //Debug.Log(GetComponent<Renderer>().sharedMaterial.GetColor("Damage"));
            //

            // GetComponent<Renderer>().material.SetColor("Damage", Color.red);
            // Debug.Log(GetComponent<Renderer>().material.GetColor("Damage"));
        }
        
        public void Restore(float amount)
        {
            Health += amount;
            if (Health >= MaxHealth)
            {
                Health = MaxHealth;
            }
			Health = MaxHealth = maxValue;
        }

        public void RestorePercentage(float amount)
        {
            var partOfHealth = MaxHealth * amount;
            Restore(partOfHealth);
        }
    }
}