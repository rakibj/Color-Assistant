using UnityEngine;
using UnityEditor;

namespace com.rakib.colorassistant
{
	public class ColorAssistantHotkeys
	{
		// % = Ctrl
		// # = Shift
		// & = Alt
		[MenuItem("Rakib/Select Color Assistant &_c")]
		static void SelectColorSetup()
		{
			//Selection.activeObject=AssetDatabase.LoadMainAssetAtPath("Assets/Rakib/Colors/Data/Resources/ProjectColorSetup.asset");
#if UNITY_EDITOR
			var configsGUIDs = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(ProjectColorSetup).Name);
			var path = AssetDatabase.GUIDToAssetPath(configsGUIDs[0]);
			Selection.activeObject=AssetDatabase.LoadMainAssetAtPath(path);
#endif
		}
	}
}