using UnityEngine;

namespace Util
{
    public class PrefabManager
    {
        public static GameObject LoadPrefab(string path)
        {
            return Resources.Load<GameObject>("Prefab/" + path);
        }
    }
}