using System;
using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UIScrollView : MonoBehaviour
    {
        public ScrollRect Component { get; private set; }

        private UIVerticalLayout _content;

        public UIScrollView SetHeight(float value)
        {
            gameObject.GetRectTransform()?.Resize(height: value);
            return this;
        }

        public UIScrollView SetContent(Action<UIVerticalLayout> applier)
        {
            applier(_content);
            return this;
        }

        private void Awake()
        {
            Component = gameObject.SafeAddComponent<ScrollRect>();
            Component.movementType = ScrollRect.MovementType.Clamped;

            // Viewport
            GameObject viewport = GameObjectManager.Create(gameObject, "Viewport",
                typeof(Image), typeof(Mask));
            viewport.GetRectTransform().StretchToParent();
            viewport.GetComponent<Mask>().showMaskGraphic = false;

            Component.viewport = viewport.GetRectTransform();

            // Content
            _content = UIVerticalLayout.Create(viewport, "Content");
            _content.RectTransform.ExpandDownFromParentTop(0);

            Component.content = _content.RectTransform;

            Component.vertical = true;
            Component.horizontal = false;

            // Vertical scrollbar
            UIScrollbar scrollbar = UIScrollbar.Create(gameObject, "Scrollbar");
            Component.verticalScrollbar = scrollbar.Component;
            Component.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        }

        public static UIScrollView Create(GameObject root, string name = null) =>
            root.CreateUI<UIScrollView>(name);
    }
}