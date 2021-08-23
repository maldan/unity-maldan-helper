using UnityEngine;

namespace Util
{
    public class UnityObject : MonoBehaviour
    {
        protected virtual void Start()
        {
            var attr = typeof(UnityObject).GetCustomAttributes(false);

            for (var i = 0; i < attr.Length; i++)
            {
                UnityEngine.Debug.Log(attr[i]);
            }
            
        }
    }
}