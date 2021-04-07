using UnityEngine;

namespace com.rakib.colorassistant
{
    public abstract class RendererPaletteBase : MonoBehaviour
    {
        public ColorId colorKey;

        protected Color GetPaletteColor()
        {
            var targetColorProperty =
                ProjectColorSetup.Instance.activePalette.colorProperties.Find(cp => cp.colorId.Equals(colorKey.value));
            if (targetColorProperty == null)
            {
                Debug.LogWarning("No color property named " + colorKey.value + " found. " +
                                 "Please check the color id again on gameobject "+ gameObject.name);
                return Color.black;
            }

            return targetColorProperty.color;
        }
        public abstract void SetPaletteColor();
    }
}