using System.Collections;
using System.Collections.Generic;
using com.rakib.colorassistant;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [AddComponentMenu("Color Assistant/Skybox Color Setter")]
    public class SkyboxPaletteSetter : RendererPaletteBase
    {
        [SerializeField] private string shaderProperty = "_SkyColor1";
        private Renderer _renderer;

        public override void SetPaletteColor()
        {
            if (!RenderSettings.skybox.HasProperty(shaderProperty))
            {
                Debug.LogWarning("No shader property named " + shaderProperty + " found. " +
                                 "Please check the shader property again on gameobject "+ gameObject.name);
                return;
            }
            RenderSettings.skybox.SetColor(shaderProperty, GetPaletteColor());
        }
    }
}