using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.rakib.colorassistant
{
    public static class EditorList
    {
        public static void Show(string listName, string elementName, SerializedProperty list)
        {
            EditorGUILayout.LabelField(listName, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            var i = 0;
            for (i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
                if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, GUILayout.Width(50)))
                {
                    if(EditorUtility.DisplayDialog("Warning!", "Are you sure? Removing keys will break the connection of settings " +
                                                    "with palettes", "Yes", "No"))
                    {
                        var oldSize = list.arraySize;
                        list.DeleteArrayElementAtIndex(i);
                        if(list.arraySize == oldSize)
                            list.DeleteArrayElementAtIndex(i);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if(GUILayout.Button("Add " + elementName))
            {
                list.InsertArrayElementAtIndex(i);
            }
            EditorGUI.indentLevel--;
        }
    }
}