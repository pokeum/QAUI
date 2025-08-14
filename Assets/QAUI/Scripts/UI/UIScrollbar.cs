using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UIScrollbar : MonoBehaviour
    {
        public Scrollbar Component { get; private set; }

        private void Awake()
        {
            Component = gameObject.SafeAddComponent<Scrollbar>();
            Image image = gameObject.SafeAddComponent<Image>();
            image.color = ColorManager.ParseColor("#80CCCCCC");
            gameObject.GetRectTransform().StretchHeightToParentRight(60);

            Component.direction = Scrollbar.Direction.BottomToTop;

            // Handle
            UIImage handle = UIImage.Create(gameObject, "Handle")
                .SetColor(ColorManager.ParseColor("#999999"));
            handle.RectTransform.StretchToParent();

            Component.targetGraphic = handle.Component;
            Component.handleRect = handle.RectTransform;
        }

        public static UIScrollbar Create(GameObject root, string name = null) =>
            root.CreateUI<UIScrollbar>(name);
    }
}