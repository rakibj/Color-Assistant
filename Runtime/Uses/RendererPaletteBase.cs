using UnityEngine;

namespace com.rakib.colorassistant
{
    public abstract class RendererPaletteBase : MonoBehaviour
    {
        public ColorId colorKey;

        protected Color GetPaletteColor()
        {
            var projectColorSetup = ProjectColorSetup.Instance;
            if (projectColorSetup == null) 
                projectColorSetup = Resources.FindObjectsOfTypeAll<ProjectColorSetup>()[0];
            
            var targetColorProperty =
                projectColorSetup.activePalette.colorProperties.Find(cp => cp.colorId.Equals(colorKey.value));
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