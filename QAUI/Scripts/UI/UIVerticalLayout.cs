using System.Collections.Generic;
using System.Linq;
using QAUI.Events;
using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UIVerticalLayout : MonoBehaviour
    {
        public VerticalLayoutGroup Component { get; private set; }

        private readonly List<RectTransform> _elementRectTransforms = new List<RectTransform>();

        private bool _autoResizeHeight = true;

        public RectTransform RectTransform => gameObject.GetRectTransform();

        public UIVerticalLayout SetWidth(float value)
        {
            RectTransform?.Resize(width: value);
            return this;
        }

        public UIVerticalLayout SetPadding(RectOffset padding)
        {
            Component.padding = padding;
            return this;
        }

        public UIVerticalLayout SetSpacing(float value)
        {
            Component.spacing = value;
            return this;
        }

        public UIVerticalLayout SetColor(Color color)
        {
            gameObject.SafeAddComponent<Image>().color = color;
            return this;
        }

        public UIVerticalLayout AddElements(AddLayoutElements action)
        {
            foreach (var element in action(gameObject))
            {
                _elementRectTransforms.Add(element.RectTransform);
            }

            return this;
        }

        public UIVerticalLayout AutoResizeHeight(bool value)
        {
            _autoResizeHeight = value;
            return this;
        }

        private void UpdateHeight()
        {
            if (_elementRectTransforms.Count < 1) return;

            var totalElementsHeight = _elementRectTransforms.Sum(rectTransform => rectTransform.rect.height);
            var totalPadding = Component.padding.vertical;
            var totalSpacing = Component.spacing * (_elementRectTransforms.Count - 1);

            RectTransform.Resize(height: totalElementsHeight + totalPadding + totalSpacing);
        }

        private void Awake()
        {
            Component = gameObject.SafeAddComponent<VerticalLayoutGroup>();
            Component.FitChildren();
        }

        private void LateUpdate()
        {
            if (_autoResizeHeight) UpdateHeight();
        }

        public static UIVerticalLayout Create(GameObject root, string name = null) =>
            root.CreateUI<UIVerticalLayout>(name);
    }
}