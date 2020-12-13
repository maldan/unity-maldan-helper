using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Audio
{
    [Serializable]
    public class SoundItem
    {
        public string CauseName;
        public AudioClip Sound;
        public string SoundPath;

        public SoundItem(string name, string path)
        {
            CauseName = name;
            SoundPath = path;
        }
    }
    
    public class Soundable : MonoBehaviour
    {
        //public SoundItem[] SoundList;
        private static readonly Dictionary<string, List<SoundItem>> SoundDict = new Dictionary<string, List<SoundItem>>();
        
        /*public void Play(string soundName, Vector3 position, float pitchRandom = 0)
        {
            
            foreach (var sound in SoundList)
            {
                if (sound.CauseName != soundName) continue;
                SoundManager.Play(sound.Sound, position, pitchRandom);
                
                return;
            }
        }*/
        
        public static void AddSound(string objectName, string soundName, string soundPath)
        {
            if (!SoundDict.ContainsKey(objectName)) SoundDict.Add(objectName, new List<SoundItem>());
            if (!SoundDict.TryGetValue(objectName, out var soundList)) return;
            foreach (var sound in soundList) if (sound.CauseName == soundName) return;
            soundList.Add(new SoundItem(soundName, soundPath));
        }
        
        public static void PlaySoundForObject(string causeName, GameObject gameObject, SoundParameters soundParameters)
        {
            if (!gameObject) return;
            if (!gameObject.GetComponent<ObjectInfo>()) return;
            
            var tags = gameObject.GetComponent<ObjectInfo>().Tag;
            if (tags == null) return;
            
            foreach (var tag in tags)
            {
                if (!SoundDict.TryGetValue(tag, out var soundList)) continue;
                
                foreach (var sound in soundList)
                    if (sound.CauseName == causeName)
                        SoundManager.PlayFromFile(sound.SoundPath, soundParameters); 
            }
        }
    }
}