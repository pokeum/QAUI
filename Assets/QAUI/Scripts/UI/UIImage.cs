using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UIImage : MonoBehaviour
    {
        public Image Component { get; private set; }
        
        public RectTransform RectTransform => gameObject.GetRectTransform();

        public UIImage SetSprite(Sprite sprite, Image.Type type = Image.Type.Sliced)
        {
            Component.sprite = sprite;
            Component.type = type;
            Component.preserveAspect = true;
            return this;
        }

        public UIImage SetColor(Color color)
        {
            Component.color = color;
            return this;
        }

        private void Awake()
        {
            Component = gameObject.SafeAddComponent<Image>();
        }

        public static UIImage Create(GameObject root, string name = null) =>
            root.CreateUI<UIImage>(name);
    }
}