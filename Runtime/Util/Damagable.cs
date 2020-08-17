using System;
using Animation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Util
{
    public class Damagable : ShaderPropertyAnimation
    {
        public float health;
        public float maxHealth;
        
        public Action onDeath;
        public Action<float> onDamage;

        public GameObject[] damagableParts;
        
        public float Percentage => health / maxHealth;

        public bool isAutoDestroy;

        public void Start()
        {
            foreach (var part in damagableParts)
            {
                part.AddComponent<ShaderPropertyAnimation>();
            }   
           
        }
        
        public void Damage(float amount)
        {
            health -= amount;
            onDamage?.Invoke(amount);
            
            if (health <= 0)
            {
                health = 0;
                onDeath?.Invoke();
                
                if (isAutoDestroy)
                {
                    Destroy(gameObject);
                }
            }
            
            // Check if gameObject has Render to make animation
            if (GetComponent<Renderer>())
            {
                AnimateFloatProperty("Damage", 0, 1, 0.2f);
            }
            
            foreach (var part in damagableParts)
            {
                part.GetComponent<ShaderPropertyAnimation>().AnimateFloatProperty("Damage", 0, 1, 0.2f);
            }
            
            //Debug.Log(GetComponent<Renderer>().sharedMaterial.GetColor("Damage"));
            //

            // GetComponent<Renderer>().material.SetColor("Damage", Color.red);
            // Debug.Log(GetComponent<Renderer>().material.GetColor("Damage"));
        }
        
        public void Restore(float amount)
        {
            health += amount;
            if (health >= maxHealth)
            {
                health = maxHealth;
            }
        }

        public void RestorePercentage(float amount)
        {
            var partOfHealth = maxHealth * amount;
            Restore(partOfHealth);
        }
    }
}