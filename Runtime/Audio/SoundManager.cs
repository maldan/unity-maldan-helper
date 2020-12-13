using UnityEngine;
using UnityEngine.Audio;
using Util;

namespace Audio
{
    public class SoundParameters
    {
        public Vector2 Volume = new Vector2(1, 1);
        public Vector2 Pitch = new Vector2(1, 1);
        public Vector3 Position;
        public float Spread = 3f;
        public Vector2 Distance = new Vector2(0.5f, 64f);
        public float SpatialBlend = 1f;
        public GameObject Target;
        public bool IsLoop = false;
    }
    
    public class SoundManager : MonoBehaviour
    {
        public AudioClip[] Audio;
        public static AudioMixerGroup DefaultAudioMixerGroup;
              
        public void Play(int id)
        {
            GetComponent<AudioSource>().clip = Audio[id];
            GetComponent<AudioSource>().Play();
        }
        
        public void Stop()
        {
            GetComponent<AudioSource>().Stop();
        }
        
        public static void PlayFromFile(string soundName,  SoundParameters soundParameter)
        {
            var sound = Resources.Load<AudioClip>("Audio/" + soundName);
            
            Play(sound, soundParameter);
        }
        
        public static void Play(AudioClip sound, SoundParameters soundParameter)
        {
            var go = new GameObject();
            go.transform.position = soundParameter.Position;
            var audioSource = go.AddComponent<AudioSource>();

            // Attach sound to target
            if (soundParameter.Target)
            {
                go.AddComponent<FlyToTarget>();
                go.GetComponent<FlyToTarget>().Target = soundParameter.Target;
                go.GetComponent<FlyToTarget>().IsInstant = true;
            }
            
            // Set default mixer
            if (DefaultAudioMixerGroup)
            {
                audioSource.outputAudioMixerGroup = DefaultAudioMixerGroup;
            }
            
            audioSource.clip = sound;
            audioSource.volume = Random.Range(soundParameter.Volume.x, soundParameter.Volume.y);
            audioSource.pitch = Random.Range(soundParameter.Pitch.x, soundParameter.Pitch.y);
            audioSource.spatialize = true;
            audioSource.spatialBlend = soundParameter.SpatialBlend;
            audioSource.minDistance = soundParameter.Distance.x;
            audioSource.maxDistance = soundParameter.Distance.y;
            audioSource.spread = soundParameter.Spread;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.loop = soundParameter.IsLoop;
            audioSource.Play();

            go.AddComponent<DestroyIfAudioEnd>().IsAudioStartPlaying = true;
        }
    }
}