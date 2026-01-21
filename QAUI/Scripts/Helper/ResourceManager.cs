using UnityEngine;

namespace QAUI
{
    public static class ResourceManager
    {
        public static Font GetFont(OpenSansFont font)
        {
            return Resources.Load<Font>($"Fonts/OpenSans-{font.ToString()}");
        }

        public static Sprite GetSprite(string sprite)
        {
            return Resources.Load<Sprite>($"Sprites/{sprite}");
        }
    }
}