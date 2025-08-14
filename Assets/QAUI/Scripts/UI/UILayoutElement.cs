using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UILayoutElement
    {
        private readonly GameObject _gameObject;
        private readonly LayoutElement _layoutElement;

        internal RectTransform RectTransform => _gameObject.GetRectTransform();

        public UILayoutElement SetHeight(float value)
        {
            _layoutElement.minHeight = value;
            _layoutElement.preferredHeight = value;
            return this;
        }

        public UILayoutElement SetWidth(float value)
        {
            _layoutElement.minWidth = value;
            _layoutElement.preferredWidth = value;
            return this;
        }

        public UILayoutElement SetFlexibleHeight(float value)
        {
            _layoutElement.flexibleHeight = value;
            return this;
        }

        public UILayoutElement SetFlexibleWidth(float value)
        {
            _layoutElement.flexibleWidth = value;
            return this;
        }

        private UILayoutElement(GameObject gameObject)
        {
            _gameObject = gameObject;
            _layoutElement = _gameObject.SafeAddComponent<LayoutElement>();
        }

        public static UILayoutElement Create(GameObject gameObject) => new UILayoutElement(gameObject);
    }
}