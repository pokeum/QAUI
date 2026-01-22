using System;
using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UIInputField : MonoBehaviour
    {
        public InputField Component { get; private set; }

        private UIText _label;
        private UIText _input;

        private float _padding = 0f;

        public string Text
        {
            get => Component.text;
            set => Component.text = value;
        }

        public UIInputField SetLabel(Action<UIText> applier)
        {
            applier(_label);
            return this;
        }

        public UIInputField SetInput(Action<UIText> applier)
        {
            applier(_input);
            return this;
        }

        public UIInputField SetInteractable(bool interactable)
        {
            Component.interactable = interactable;
            return this;
        }

        public UIInputField SetPadding(float padding)
        {
            _padding = padding;
            OnRectTransformDimensionsChange();
            return this;
        }

        private void Awake()
        {
            Component = gameObject.SafeAddComponent<InputField>();

            var background = gameObject.SafeAddComponent<Image>();
            background.color = ColorManager.ParseColor(Colors.Transparent);

            _label = UIText.Create(gameObject, "Label")
                .SetAlignment(TextAnchor.MiddleLeft)
                .SetFont(ResourceManager.GetFont(OpenSansFont.Semibold))
                .SetColor(ColorManager.ParseColor(Colors.Gray700));

            UISeparator.Create(gameObject, "Line");

            _input = UIText.Create(gameObject, "Input")
                .SetAlignment(TextAnchor.MiddleLeft)
                .SetFont(ResourceManager.GetFont(OpenSansFont.Regular))
                .SetColor(ColorManager.ParseColor(Colors.Gray600));

            Component.textComponent = _input.Component;
        }

        private void OnRectTransformDimensionsChange()
        {
            var rect = gameObject.GetRectTransform();
            if (rect == null) return;

            var parentWidth = rect.rect.width;
            var parentHeight = rect.rect.height;

            _label.RectTransform.AlignLeftCenterToParent((parentWidth - _padding) / 2f, parentHeight, 0);
            _input.RectTransform.AlignRightCenterToParent((parentWidth - _padding) / 2f, parentHeight, 0);
        }

        public static UIInputField Create(GameObject root, string name = null) =>
            root.CreateUI<UIInputField>(name);
    }
}