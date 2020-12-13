using UnityEngine;

namespace Audio
{
    public class PlaySoundAtStart : MonoBehaviour
    {
        public AudioClip AudioClip;
        public Vector2 Distance = new Vector2(0.5f, 48f);
        
        private void Start()
        {
            SoundManager.Play(AudioClip, new SoundParameters
            {
                Position = transform.position,
                Distance = Distance
            });
        }
    }
}