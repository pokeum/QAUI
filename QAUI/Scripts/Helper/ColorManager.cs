using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public static class ColorManager
    {
        public static Color ParseColor(string hex)
        {
            return ParseColor(hex, Color.white);
        }

        public static Color ParseColor(string hex, Color defaultColor)
        {
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            try
            {
                switch (hex.Length)
                {
                    /* RRGGBB */
                    case 6:
                    {
                        var bytes = hex.ToBytes();
                        return new Color32(bytes[0], bytes[1], bytes[2], 0xFF);
                    }
                    /* AARRGGBB */
                    case 8:
                    {
                        var bytes = hex.ToBytes();
                        return new Color32(bytes[1], bytes[2], bytes[3], bytes[0]);
                    }
                }
            }
            catch
            {
                // ignored
            }

            return defaultColor;
        }

        internal static void SetColors(this Selectable gameObject, ColorBlock colors)
        {
            gameObject.colors = colors.Overwrite(gameObject.colors);
        }
    }

    public readonly struct ColorBlock
    {
        private readonly Color? _normal;
        private readonly Color? _highlighted;
        private readonly Color? _pressed;
        private readonly Color? _selected;
        private readonly Color? _disabled;

        public ColorBlock(
            Color? normal = null,
            Color? highlighted = null,
            Color? pressed = null,
            Color? selected = null,
            Color? disabled = null
        )
        {
            _normal = normal;
            _highlighted = highlighted;
            _pressed = pressed;
            _selected = selected;
            _disabled = disabled;
        }

        internal UnityEngine.UI.ColorBlock Overwrite(UnityEngine.UI.ColorBlock colors)
        {
            var newColors = colors;

            if (_normal.HasValue) newColors.normalColor = _normal.Value;
            if (_highlighted.HasValue) newColors.highlightedColor = _highlighted.Value;
            if (_pressed.HasValue) newColors.pressedColor = _pressed.Value;
            if (_selected.HasValue) newColors.selectedColor = _selected.Value;
            if (_disabled.HasValue) newColors.disabledColor = _disabled.Value;

            return newColors;
        }
    }
}