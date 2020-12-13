using UnityEngine;
using Util;

namespace Audio
{
    public class PlaySoundOnCollision : MonoBehaviour
    {
        public AudioClip Sound;
        public Vector2 Volume = new Vector2(1, 1);
        public Vector2 Pitch = new Vector2(1, 1);
        public string[] AllowedTags;
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.GetComponent<ObjectInfo>()) return;

            if (other.gameObject.GetComponent<ObjectInfo>().HasTag(AllowedTags))
            {
                SoundManager.Play(Sound, new SoundParameters
                {
                    Position = transform.position,
                    Volume = Volume,
                    Pitch = Pitch
                });
            }
        }
    }
}