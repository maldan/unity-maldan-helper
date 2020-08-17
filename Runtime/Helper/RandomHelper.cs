using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public class RandomHelper
    {
        public static List<Vector2> GenerateRandomField(int width, int height, int maxAmount, float maxDotRadius)
        {
            var craterDots = new List<Vector2>();
            for (var i = 0; i < maxAmount; i++)
            {
                var d = new Vector2(Random.Range(0, width), Random.Range(0, height));
                var isSkip = false;
                foreach (var dot in craterDots)
                {
                    if (!(Vector2.Distance(dot, d) < maxDotRadius)) continue;
                    isSkip = true;
                    break;
                }
                
                if (isSkip)
                {
                    continue;    
                }
                
                craterDots.Add(d);
            }

            return craterDots;
        }
    }
}