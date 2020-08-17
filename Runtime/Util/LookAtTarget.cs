using UnityEngine;

namespace Util
{
    public class LookAtTarget : MonoBehaviour
    {
        public GameObject target;
        public string targetName;
        
        private void Update()
        {
            if (!target)
            {
                if (targetName != null)
                {
                    target = GameObject.Find(targetName);
                }
                return;
            }
            
            transform.LookAt(target.transform);
        }
    }
}