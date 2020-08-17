using System;
using UnityEngine;

namespace Util
{
    public class Experienceable : MonoBehaviour
    {
        public float exp;
        public float nextExp;
        public float nextStepMultiplier;

        private int _level = 1;
        
        public int Level => _level;
        public string Status => $"{exp}/{nextExp}";
        public float CurrentProgress => exp / nextExp;
        
        public Action onLevelUp;
        
        public void AddExp(float amount)
        {
            exp += amount;
            if (exp > nextExp)
            {
                _level++;
                exp = 0;
                nextExp += nextExp * nextStepMultiplier;
                onLevelUp?.Invoke();
            }
        }
    }
}