using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Tines.Lib.Utils
{
    public static class NumberHelper
    {
        public static bool Enough(this float num, float amount)
        {
            return num - amount >= 0;
        }
        
        public static bool Enough(this double num, double amount)
        {
            return num - amount >= 0;
        }
        
        public static float Lerp(float a, float b, float t)
        {
            t = Mathf.Clamp01(t);
            return a + (b - a) * t;
        }
           
        public static void Swap<T> (ref T lhs, ref T rhs) {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        
        public static string ToHumanReadibleSize(this double value, float precision = 1, float minimum = 10000)
        {
            var fpr = precision - 1;
            if (fpr <= 0) fpr = 0;
            
            if (value >= 1000000000000d) return (value / 1000000000000d).ToString("N" + fpr) + 'T';
            if (value >= 1000000000d) return (value / 1000000000d).ToString("N" + fpr) + 'B';
            if (value >= 1000000d) return (value / 1000000d).ToString("N" + fpr) + 'M';
            if (value >= minimum) return (value / 1000d).ToString("N" + fpr) + 'K';
            return value.ToString("N" + precision);
        }

        public static string ToHMS(this float value)
        {
            var hours = Mathf.Floor(value / 3600).ToString("00");
            var minutes = Mathf.Floor(value / 60).ToString("00");
            var seconds = (value % 60).ToString("00");
            
            return $"{hours}:{minutes}:{seconds}";
        }
    }
}