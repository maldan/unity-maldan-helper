using System.Linq;
using UnityEngine;

namespace Util
{
    public class ObjectInfo : MonoBehaviour
    {
        public int Id;
        
        public string Title;
        public string Description;
        
        public string[] Tag;
        public string[] Data;
        
        // Params
        public int IntParameter_1;

        public bool HasTag(string tagName)
        {
            if (Tag == null) return false;
            return Tag.Count(x => x == tagName) != 0;
        }
        
        public bool HasTag(string[] tags)
        {
            return tags.Any(HasTag);
        }
    }
}