using System.Collections.Generic;
using System.Linq;
using QAUI.Events;
using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UIHorizontalLayout : MonoBehaviour
    {
        public HorizontalLayoutGroup Component { get; private set; }

        private readonly List<RectTransform> _elementRectTransforms = new List<RectTransform>();
        
        private bool _autoResizeWidth = true;

        public RectTransform RectTransform => gameObject.GetRectTransform();

        public UIHorizontalLayout SetHeight(float value)
        {
            RectTransform?.Resize(height: value);
            return this;
        }

        public UIHorizontalLayout SetPadding(RectOffset padding)
        {
            Component.padding = padding;
            return this;
        }

        public UIHorizontalLayout SetSpacing(float value)
        {
            Component.spacing = value;
            return this;
        }

        public UIHorizontalLayout SetColor(Color color)
        {
            gameObject.SafeAddComponent<Image>().color = color;
            return this;
        }

        public UIHorizontalLayout AddElements(AddLayoutElements action)
        {
            foreach (var element in action(gameObject))
            {
                _elementRectTransforms.Add(element.RectTransform);
            }

            return this;
        }
        
        public UIHorizontalLayout AutoResizeWidth(bool value)
        {
            _autoResizeWidth = value;
            return this;
        }
        
        private void UpdateWidth()
        {
            if (_elementRectTransforms.Count < 1) return;

            float totalElementsWidth = _elementRectTransforms.Sum(rectTransform => rectTransform.rect.width);
            float totalPadding = Component.padding.horizontal;
            float totalSpacing = Component.spacing * (_elementRectTransforms.Count - 1);

            RectTransform.Resize(width: totalElementsWidth + totalPadding + totalSpacing);
        }

        private void Awake()
        {
            Component = gameObject.SafeAddComponent<HorizontalLayoutGroup>();
            Component.FitChildren();
        }

        private void LateUpdate()
        {
            if (_autoResizeWidth) UpdateWidth();
        }

        public static UIHorizontalLayout Create(GameObject root, string name = null) =>
            root.CreateUI<UIHorizontalLayout>(name);
    }
}