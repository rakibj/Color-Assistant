using System.Collections.Generic;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [CreateAssetMenu(fileName = "Palette", menuName = "Color Palette/New Palette", order = 0)]
    public class ColorPalette : ScriptableObject
    {
        private ProjectColorSetup _projectColorSetup;
        public ColorPaletteSettings settings;
        public List<ColorProperty> colorProperties;

        [ContextMenu("Initialize")]
        public void Init()
        {
            colorProperties = new List<ColorProperty>();
            for (int i = 0; i < settings.colorIds.Count; i++)
                colorProperties.Add(new ColorProperty(settings.colorIds[i], Color.black));
        }
        
        [ContextMenu("Update")]
        public void UpdatePropertiesIfSettingsChanged()
        {
            if (settings == null) return;
            for (int i = 0; i < settings.colorIds.Count; i++)
            {
                //for the indices those already existed
                if (i < colorProperties.Count)
                    colorProperties[i].colorId = settings.colorIds[i];
                else
                    colorProperties.Add(new ColorProperty(settings.colorIds[i], Color.black));
            }

            var tempColorProperties = colorProperties;
            for (int i = 0; i < colorProperties.Count; i++)
            {
                if (i >= settings.colorIds.Count)
                {
                    tempColorProperties.Remove(colorProperties[i]);
                }
            }

            colorProperties = tempColorProperties;
        }

        public void UpdateColorsInScene()
        {
            _projectColorSetup = ProjectColorSetup.Instance;
            if (_projectColorSetup == null) _projectColorSetup = Resources.FindObjectsOfTypeAll<ProjectColorSetup>()[0];
            _projectColorSetup.UpdateSceneMaterialColors();
        }
    }

    [System.Serializable]
    public class ColorProperty
    {
        public string colorId;
        public Color color;

        public ColorProperty(string colorId, Color color)
        {
            this.colorId = colorId;
            this.color = color;
        }
    }
}