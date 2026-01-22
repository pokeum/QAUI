using UnityEngine;

namespace QAUI
{
    [AddComponentMenu("QAUI/Component/ContentHeightResizer")]
    [RequireComponent(typeof(RectTransform))]
    public class ContentHeightResizer : MonoBehaviour
    {
        [SerializeField] private RectTransform content;

        [SerializeField] public float paddingTop;
        [SerializeField] public float paddingBottom;
        [SerializeField] public float paddingLeft;
        [SerializeField] public float paddingRight;

        private RectTransform _rectTransform;
        private RectTransform _parentRectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parentRectTransform = transform.parent.GetComponent<RectTransform>();
        }

        public void LateUpdate()
        {
            // Calculate default size
            var defaultWidth = _parentRectTransform.rect.width - (paddingLeft + paddingRight);
            var defaultHeight = _parentRectTransform.rect.height - (paddingTop + paddingBottom);

            // Get content preferred size
            var contentHeight = content.rect.height;

            _rectTransform.Resize(defaultWidth, Mathf.Min(defaultHeight, contentHeight));
        }
    }
}