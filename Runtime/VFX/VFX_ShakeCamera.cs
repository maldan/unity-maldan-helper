using Camera;
using UnityEngine;
using Util;

namespace VFX
{
    public class VFX_ShakeCamera : MonoBehaviour
    {
        public float MaxDistance;
        public float Power;
        public bool IsPlayOnDamagableDestroy;
        
        private void Start()
        {
            if (IsPlayOnDamagableDestroy)
            {
                if (GetComponent<Damagable>())
                {
                    GetComponent<Damagable>().OnDeath += () =>
                    {
                        UnityEngine.Camera.main.GetComponent<TopCamera>()
                            .ShakeByDistanceToTarget(transform.position, MaxDistance, Power);
                    };
                }
            }
            else
            {
                UnityEngine.Camera.main.GetComponent<TopCamera>().ShakeByDistanceToTarget(transform.position, MaxDistance, Power);
            }
        }
    }
}