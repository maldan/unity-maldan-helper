using UnityEngine;

namespace Util
{
    public class CopyTransform : MonoBehaviour
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

            transform.position = target.transform.position;
            transform.rotation = target.transform.rotation;

            if (GetComponent<UnityEngine.Camera>() && target.GetComponent<UnityEngine.Camera>())
            {
                GetComponent<UnityEngine.Camera>().fieldOfView = target.GetComponent<UnityEngine.Camera>().fieldOfView;
            }
        }
    }
}