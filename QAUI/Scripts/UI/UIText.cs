using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UIText : MonoBehaviour
    {
        public Text Component { get; private set; }

        public RectTransform RectTransform => gameObject.GetRectTransform();

        public string Text
        {
            get => Component.text;
            set => SetText(value);
        }

        private bool _useContentHeight = false;

        public UIText UseContentHeight(bool value = true)
        {
            _useContentHeight = value;
            return this;
        }

        public UIText SetText(string text)
        {
            Component.text = text;
            OnRectTransformDimensionsChange();
            return this;
        }

        public UIText SetAlignment(TextAnchor alignment)
        {
            Component.alignment = alignment;
            OnRectTransformDimensionsChange();
            return this;
        }

        public UIText SetFont(Font font)
        {
            Component.font = font;
            OnRectTransformDimensionsChange();
            return this;
        }

        public UIText SetFontSize(int fontSize)
        {
            Component.fontSize = fontSize;
            OnRectTransformDimensionsChange();
            return this;
        }

        public UIText SetColor(Color color)
        {
            Component.color = color;
            return this;
        }

        private void Awake()
        {
            Component = gameObject.SafeAddComponent<Text>();

            SetAlignment(TextAnchor.MiddleLeft);
            SetFont(ResourceManager.GetFont(OpenSansFont.Regular));
            SetFontSize(40);
            SetColor(Color.black);
        }

        private void OnRectTransformDimensionsChange()
        {
            var rect = gameObject.GetRectTransform();
            if (rect == null) return;

            if (_useContentHeight)
            {
                rect.Resize(height: Component.preferredHeight);
            }
        }

        public static UIText Create(GameObject root, string name = null) =>
            root.CreateUI<UIText>(name);
    }
}