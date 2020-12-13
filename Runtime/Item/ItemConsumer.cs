using System;
using Audio;
using UnityEngine;

namespace Item
{
    public class ItemConsumer : MonoBehaviour
    {
        public Func<DefaultItem, bool> CheckConsume;
        public Action<DefaultItem> OnConsume;
          
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.GetComponent<DefaultItem>()) return;
        }

        private void OnTriggerStay(Collider other)
        {
            var item = other.gameObject.GetComponent<DefaultItem>();
            
            if (!item) return;
            if (!CheckConsume.Invoke(item)) return;
            
            if (Vector3.Distance(other.gameObject.transform.position, transform.position) <= 4f && CheckConsume.Invoke(item))
            {
                OnConsume?.Invoke(item);
                Destroy(other.gameObject);
                if (item.ConsumeSound)
                {
                    SoundManager.Play(item.ConsumeSound, new SoundParameters
                    {
                        Position = transform.position,
                        Distance = new Vector2(0.5f, 32),
                        Volume = new Vector2(0.8f, 1f),
                        Pitch = new Vector2(1f, 1.1f)
                    });
                }
            }
            else other.gameObject.transform.position += (transform.position - other.gameObject.transform.position) / 6f;
        }
    }
}