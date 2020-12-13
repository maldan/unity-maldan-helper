using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class UpdateVFXPosition : MonoBehaviour
    {
        private void Update()
        {
            GetComponent<VisualEffect>().SetVector3("Position", transform.position);
        }
    }
}