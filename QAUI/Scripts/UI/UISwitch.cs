using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    public class UISwitch : MonoBehaviour
    {
        public Toggle Component { get; private set; }

        private UIImage _background;
        private UIImage _handle;

        private Vector2 _handlePosition;

        public bool IsOn
        {
            get => Component.isOn;
            set => Component.isOn = value;
        }

        public UISwitch SetInteractable(bool interactable)
        {
            Component.interactable = interactable;
            return this;
        }

        private void Awake()
        {
            Component = gameObject.SafeAddComponent<Toggle>();

            _background = UIImage.Create(gameObject, "Background")
                .SetSprite(ResourceManager.GetSprite(Sprites.SwitchBackground1), Image.Type.Simple);
            _background.RectTransform.AlignRightCenterToParent(100, 100, 0);

            _handle = UIImage.Create(_background.gameObject, "Handle")
                .SetSprite(ResourceManager.GetSprite(Sprites.SwitchHandle1), Image.Type.Simple);
            _handle.RectTransform.AlignRightCenterToParent(100, 100, 25);

            _handlePosition = _handle.RectTransform.anchoredPosition;

            Component.onValueChanged.AddListener(OnSwitch);
            OnSwitch(Component.isOn);
        }

        private void OnDestroy()
        {
            Component.onValueChanged.RemoveListener(OnSwitch);
        }

        private void OnSwitch(bool isOn)
        {
            _handle.RectTransform.anchoredPosition = isOn ? _handlePosition * -1 : _handlePosition;
            _background.SetColor(
                isOn
                    ? ColorManager.ParseColor(Colors.Blue300)
                    : ColorManager.ParseColor(Colors.Gray300)
            );
            _handle.SetColor(
                isOn
                    ? ColorManager.ParseColor(Colors.Blue800)
                    : ColorManager.ParseColor(Colors.Gray800)
            );
        }

        public static UISwitch Create(GameObject root, string name = null) =>
            root.CreateUI<UISwitch>(name);
    }
}