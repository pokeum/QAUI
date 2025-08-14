using UnityEngine;

namespace QAUI
{
    public class UISeparator
    {
        private readonly UIImage _image;

        public UISeparator SetColor(Color color)
        {
            _image.SetColor(color);
            return this;
        }

        public UISeparator SetFrame(float height)
        {
            _image.RectTransform.Resize(height: height);
            return this;
        }

        private UISeparator(GameObject root, string name)
        {
            _image = UIImage.Create(root, name ?? nameof(UISeparator));
            _image.RectTransform.StretchWidthToParentBottom();

            SetColor(Color.black);
            SetFrame(3f);
        }

        public static UISeparator Create(GameObject root, string name = null) =>
            new UISeparator(root, name);
    }
}