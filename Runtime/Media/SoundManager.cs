using System.Collections.Generic;
using UnityEngine;
using Util.UDict;

namespace Media
{
    public class SoundManager : MonoBehaviour
    {
        public AudioClip[] audio;

        public void Play(int id)
        {
            GetComponent<AudioSource>().clip = audio[id];
            GetComponent<AudioSource>().Play();
        }

        public void Stop()
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}