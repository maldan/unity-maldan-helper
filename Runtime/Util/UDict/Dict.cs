using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Util.UDict
{
    [Serializable]
    public class DictString: Dict<string> {  }
    
    [Serializable]
    public class DictGameObject: Dict<GameObject> {  }
    
    [Serializable]
    public abstract class Dict<T>
    {
        [FormerlySerializedAs("_keys")] [SerializeField]
        private List<string> keys = new List<string>();
        
        [FormerlySerializedAs("_values")] [SerializeField]
        private List<T> values = new List<T>();
        
        public T this[string key]
        {
            get
            {
                var index = keys.IndexOf(key);
                return index <= -1 ? default : values[index];
            }
        }
    }
}