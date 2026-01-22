using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UIDropdown : MonoBehaviour
    {
        public Dropdown Component { get; private set; }

        private UIImage _background;
        private UIText _label;

        private GameObject _template;

        private GameObject _item;
        private UIImage _itemCheckmark;
        private UIText _itemLabel;

        public int Value
        {
            get => Component.value;
            set => Component.value = value;
        }

        public UIDropdown SetOptions(List<string> options)
        {
            Component.ClearOptions();
            Component.AddOptions(options);
            return this;
        }

        public UIDropdown SetColor(Color color)
        {
            _background.SetColor(color);
            return this;
        }

        public UIDropdown SetLabel(Action<UIText> applier)
        {
            applier(_label);
            return this;
        }

        public UIDropdown SetTemplateHeight(float value)
        {
            _template.GetRectTransform().ExpandDownFromParentBottom(value);
            return this;
        }

        public UIDropdown SetTemplateColor(ColorBlock colors)
        {
            var itemToggle = _item.GetComponent<Toggle>();
            if (itemToggle != null) itemToggle.SetColors(colors);
            return this;
        }

        public UIDropdown SetItemCheckmark(Action<UIImage> applier)
        {
            applier(_itemCheckmark);
            return this;
        }

        public UIDropdown SetItemLabel(Action<UIText> applier)
        {
            applier(_itemLabel);
            return this;
        }

        private void Awake()
        {
            Component = gameObject.SafeAddComponent<Dropdown>();

            _background = UIImage.Create(gameObject)
                .SetSprite(ResourceManager.GetSprite(Sprites.Container))
                .SetColor(ColorManager.ParseColor(Colors.LightBlue));
            _background.RectTransform.StretchToParent();

            _label = UIText.Create(gameObject, "Label")
                .SetAlignment(TextAnchor.MiddleCenter)
                .SetFont(ResourceManager.GetFont(OpenSansFont.Semibold))
                .SetColor(ColorManager.ParseColor(Colors.Gray700));
            _label.RectTransform.StretchToParent(new RectOffset(10, 80, 10, 10));

            Component.captionText = _label.Component;

            var arrow = UIImage.Create(gameObject, "Arrow")
                .SetSprite(ResourceManager.GetSprite(Sprites.Icon.ArrowDropdown))
                .SetColor(Color.black);
            arrow.RectTransform.AlignRightCenterToParent(60, 60, 10);

            CreateTemplate();
        }

        private void CreateTemplate()
        {
            _template = GameObjectManager.Create(gameObject, "Template", typeof(ScrollRect), typeof(Image));
            _template.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Clamped;

            var viewport = GameObjectManager.Create(_template, "Viewport", typeof(Mask), typeof(Image));
            viewport.GetRectTransform().StretchToParent();

            var content =
                GameObjectManager.Create(viewport, "Content", typeof(RectTransform), typeof(VerticalLayoutGroup));
            content.GetRectTransform().ExpandDownFromParentTop(100);
            _template.GetComponent<ScrollRect>().content = content.GetRectTransform();

            _item = GameObjectManager.Create(content, "Item", typeof(Toggle), typeof(Image), typeof(LayoutElement));
            _item.GetComponent<Toggle>().targetGraphic = _item.GetComponent<Image>();

            _itemCheckmark = UIImage.Create(_item, "Item Checkmark")
                .SetSprite(ResourceManager.GetSprite(Sprites.Icon.Check))
                .SetColor(ColorManager.ParseColor(Colors.Blue900));
            _itemCheckmark.RectTransform.AlignLeftCenterToParent(50, 50, 20);
            _item.GetComponent<Toggle>().graphic = _itemCheckmark.Component;

            _itemLabel = UIText.Create(_item, "Item Label")
                .SetAlignment(TextAnchor.MiddleLeft);
            _itemLabel.RectTransform.StretchToParent(new RectOffset(90, 10, 10, 10));

            Component.template = _template.GetRectTransform();
            Component.itemText = _itemLabel.Component;

            SetTemplateHeight(400);
            SetTemplateColor(
                new ColorBlock(
                    normal: ColorManager.ParseColor(Colors.LightBlue),
                    highlighted: ColorManager.ParseColor(Colors.LightBlue),
                    selected: ColorManager.ParseColor(Colors.LightBlue),
                    pressed: ColorManager.ParseColor(Colors.Blue100)
                )
            );

            _template.SetActive(false);
        }

        public static UIDropdown Create(GameObject root, string name = null) =>
            root.CreateUI<UIDropdown>(name);
    }
}