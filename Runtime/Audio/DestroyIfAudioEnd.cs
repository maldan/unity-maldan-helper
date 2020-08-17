using UnityEngine;

namespace Audio
{
    public class DestroyIfAudioEnd : MonoBehaviour
    {
        public bool IsAudioStartPlaying;
        
        private void Update()
        {
            if (!IsAudioStartPlaying)
            {
                return;
            }
            
            if (!GetComponent<AudioSource>().isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }
}