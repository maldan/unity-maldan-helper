using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Util.UDict
{
    public class DictDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var labelPos = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var keys = property.FindPropertyRelative("keys");
            var values = property.FindPropertyRelative("values");
            var prevHeight = 16f;
            
            for (var i = 0; i < keys.arraySize; i++)
            {
                var keyProp = keys.FindPropertyRelative($"Array.data[{i}]");
                var valueProp = values.FindPropertyRelative($"Array.data[{i}]");
                
                var newRect = new Rect(position.x, position.y + prevHeight + EditorGUIUtility.standardVerticalSpacing, position.width / 2, EditorGUI.GetPropertyHeight(keyProp, label, true));
                EditorGUI.PropertyField(newRect, keyProp, GUIContent.none);
                
                newRect = new Rect(position.x + position.width / 2, position.y + prevHeight + EditorGUIUtility.standardVerticalSpacing, position.width / 2 - 24, EditorGUI.GetPropertyHeight(valueProp, label, true));
                EditorGUI.PropertyField(newRect, valueProp, GUIContent.none);
                
                newRect = new Rect(position.x + position.width - 24, position.y + prevHeight + EditorGUIUtility.standardVerticalSpacing, 24, EditorGUI.GetPropertyHeight(valueProp, label, true));
                if (GUI.Button(newRect, "x"))
                {
                    keys.DeleteArrayElementAtIndex(i);
                    values.DeleteArrayElementAtIndex(i);
                }
                
                prevHeight += newRect.height + EditorGUIUtility.standardVerticalSpacing;
            }
            
            var buttonRect = new Rect(position.x, position.y + prevHeight + EditorGUIUtility.standardVerticalSpacing, position.width, 24);
            if (GUI.Button(buttonRect, "Add new"))
            {
                keys.arraySize += 1;
                values.arraySize += 1;
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var totalHeight = 48f + 8f;
            var keys = property.FindPropertyRelative("keys");
            for (var i = 0; i < keys.arraySize; i++)
            {
                var keyProp = keys.FindPropertyRelative($"Array.data[{i}]");
                totalHeight += EditorGUI.GetPropertyHeight(keyProp, label, true);
            }

            return totalHeight;
        }
    }

    [CustomPropertyDrawer(typeof(DictString))]
    public class DictStringDrawer : DictDrawer
    {
    }
    
    [CustomPropertyDrawer(typeof(DictGameObject))]
    public class DictGameObjectDrawer : DictDrawer
    {
    }
}
#endif