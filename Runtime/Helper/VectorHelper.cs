using UnityEngine;

namespace Helper
{
    public static class VectorHelper
    {
        public static float DistanceXZ(this Vector3 vc1, Vector3 vc2)
        {
            return Vector3.Distance(new Vector3(vc1.x, 0, vc1.z), new Vector3(vc2.x, 0, vc2.z));
        }
        
        public static Vector3 Random(this Vector3 vector3)
        {
            return new Vector3(
                UnityEngine.Random.Range(-vector3.x / 2, vector3.x / 2),
                UnityEngine.Random.Range(-vector3.y / 2, vector3.y / 2),
                UnityEngine.Random.Range(-vector3.z / 2, vector3.z / 2)
            );    
        }
    }
}