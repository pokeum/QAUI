using System;
using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UIButton : MonoBehaviour
    {
        public Button Component { get; private set; }

        private UIText _text;
        private Action _onClick;

        public RectTransform RectTransform => gameObject.GetRectTransform();
        
        public UIButton SetText(Action<UIText> applier)
        {
            applier(_text);
            return this;
        }

        public UIButton SetOnClickListener(Action onClick)
        {
            _onClick = onClick;
            return this;
        }

        public UIButton SetIcon(bool value = true)
        {
            const string iconName = "Icon";
            if (value)
            {
                UIImage icon = UIImage.Create(gameObject, iconName)
                    .SetSprite(ResourceManager.GetSprite(Sprites.Icon.ArrowRight));
                icon.RectTransform.AlignRightCenterToParent(80, 80, 40);
            }
            else
            {
                gameObject.RemoveFirstChild(iconName);
            }

            return this;
        }

        public UIButton SetColors(ColorBlock colors)
        {
            Component.SetColors(colors);
            return this;
        }

        public UIButton SetEnabled(bool value)
        {
            Component.interactable = value;
            return this;
        }
        
        private void Awake()
        {
            Component = gameObject.SafeAddComponent<Button>();
            Component.onClick.AddListener(OnClick);

            var background = gameObject.SafeAddComponent<Image>();
            background.sprite = ResourceManager.GetSprite(Sprites.Container);
            background.type = Image.Type.Sliced;
            Component.targetGraphic = background;

            SetColors(
                new ColorBlock(
                    ColorManager.ParseColor(Colors.Blue600),
                    ColorManager.ParseColor(Colors.Blue600),
                    ColorManager.ParseColor(Colors.Blue700),
                    ColorManager.ParseColor(Colors.Blue600),
                    ColorManager.ParseColor(Colors.Gray400)
                )
            );

            _text = UIText.Create(gameObject, "Text")
                .SetAlignment(TextAnchor.MiddleCenter)
                .SetColor(Color.white);
            _text.RectTransform.StretchToParent();
        }

        private void OnClick()
        {
            _onClick?.Invoke();
        }

        public static UIButton Create(GameObject root, string name = null) =>
            root.CreateUI<UIButton>(name);
    }
}