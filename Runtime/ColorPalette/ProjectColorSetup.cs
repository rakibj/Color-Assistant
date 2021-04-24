using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [CreateAssetMenu(menuName = "Color Assistant/Project Color Setup")]
    public class ProjectColorSetup : SingletonScriptableObject<ProjectColorSetup>
    {
        public ColorPaletteSettings paletteSettings;
        public ColorPalette activePalette;
        
        public bool checkContrast = false;
        public bool modifyProperties = false;
        public float brightness = 1f;
        public float saturation = 1f;
        public float contrast = 1f;

        public void UpdateSceneMaterialColors()
        {
            var renderers = FindObjectsOfType<RendererPaletteBase>();
            foreach (var renderer in renderers)
            {
                renderer.SetPaletteColor();
            }
        }

    }
}