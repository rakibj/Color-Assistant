using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.rakib.colorassistant
{
    [CreateAssetMenu(menuName = "Color Assistant/New Palette Settings")]
    public class ColorPaletteSettings : ScriptableObject
    {
        public List<string> colorIds;
    }
}