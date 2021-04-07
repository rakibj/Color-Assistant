using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [RequireComponent(typeof(Renderer))]
    [AddComponentMenu("Color Assistant/Mesh Color Setter")]
    public class RendererPaletteSetter : RendererPaletteBase
    {
        [SerializeField] private int materialIndex = 0;
        [SerializeField] private string shaderProperty = "_Color";
        private Renderer _renderer;

        public override void SetPaletteColor()
        {
            _renderer = GetComponent<Renderer>();

            if (!_renderer.sharedMaterials[materialIndex].HasProperty(shaderProperty))
            {
                Debug.LogWarning("No shader property named " + shaderProperty + " found. " +
                                 "Please check the shader property again on gameobject "+ gameObject.name);
                return;
            }
            _renderer.sharedMaterials[materialIndex].SetColor(shaderProperty, GetPaletteColor());
        }
    }
}