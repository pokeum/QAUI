using UnityEngine;

namespace QAUI
{
    public class UISpacer : MonoBehaviour
    {
        public UISpacer SetFrame(float height)
        {
            gameObject.GetRectTransform()?.Resize(height: height);
            return this;
        }

        private void Awake()
        {
            gameObject.GetRectTransform()?.Resize(height: 60f);
        }

        public static UISpacer Create(GameObject root, string name = null) =>
            root.CreateUI<UISpacer>(name);
    }
}