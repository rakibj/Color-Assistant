using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [CustomPropertyDrawer(typeof(ColorId))]
    public class ColorIdDrawer : PropertyDrawer
    {
        private int _index;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            //property.serializedObject.Update();
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            //This is the way to set value of a field so that it keeps saved even after editor restart
            var indexProperty = property.FindPropertyRelative("index");
            _index = indexProperty.intValue;
            _index = EditorGUI.Popup(new Rect(position.x, position.y, 150f, position.height), 
                _index, ProjectColorSetup.Instance.paletteSettings.colorIds.ToArray());
            indexProperty.intValue = Mathf.Clamp(_index, 0, ProjectColorSetup.Instance.paletteSettings.colorIds.Count);
            
            var valueProperty = property.FindPropertyRelative("value");
            valueProperty.stringValue = ProjectColorSetup.Instance.paletteSettings.colorIds[indexProperty.intValue];
            
            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }
    }
}