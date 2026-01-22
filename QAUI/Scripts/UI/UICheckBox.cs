using System;
using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UICheckBox : MonoBehaviour
    {
        public Toggle Component { get; private set; }

        private UIText _label;

        public UICheckBox SetLabel(Action<UIText> applier)
        {
            applier(_label);
            return this;
        }

        public UICheckBox SetValue(bool isOn)
        {
            Component.isOn = isOn;
            return this;
        }

        public bool IsOn
        {
            get => Component.isOn;
            set => SetValue(value);
        }

        public UICheckBox SetInteractable(bool value)
        {
            Component.interactable = value;
            return this;
        }
        
        private void Awake()
        {
            Component = gameObject.SafeAddComponent<Toggle>();

            _label = UIText.Create(gameObject, "Label")
                .SetAlignment(TextAnchor.MiddleLeft)
                .SetFont(ResourceManager.GetFont(OpenSansFont.Semibold))
                .SetColor(ColorManager.ParseColor(Colors.Gray700));
            _label.RectTransform.StretchToParent(new RectOffset(0, 60, 0, 0));

            var background = UIImage.Create(gameObject, "Background")
                .SetSprite(ResourceManager.GetSprite(Sprites.ToggleBackgroundUp));
            background.RectTransform.AlignRightCenterToParent(50, 50, 0);

            var checkmark = UIImage.Create(background.gameObject, "Checkmark")
                .SetSprite(ResourceManager.GetSprite(Sprites.ToggleCheckmark));
            checkmark.RectTransform.Resize(30, 30);

            Component.graphic = checkmark.Component;

            Component.isOn = true;
        }

        public static UICheckBox Create(GameObject root, string name = null) =>
            root.CreateUI<UICheckBox>(name);
    }
}