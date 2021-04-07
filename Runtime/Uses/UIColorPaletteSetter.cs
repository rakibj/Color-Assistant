using UnityEngine;
using UnityEngine.UI;

namespace com.rakib.colorassistant
{
    [RequireComponent(typeof(MaskableGraphic))]
    [AddComponentMenu("Color Assistant/UI Color Setter")]
    public class UIColorPaletteSetter : RendererPaletteBase
    {
        private MaskableGraphic _graphic;

        public override void SetPaletteColor()
        {
            _graphic = GetComponent<MaskableGraphic>();
            _graphic.color = GetPaletteColor();
        }
    }
}