using UnityEngine;

namespace QAUI
{
    /// <summary>
    /// Extension methods for RectTransform.
    /// </summary>
    public static class RectTransformExtension
    {
        public static void StretchToParent(this RectTransform rectTransform, RectOffset padding = null)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;

            if (padding == null)
            {
                rectTransform.sizeDelta = Vector2.zero;
            }
            else
            {
                rectTransform.offsetMin = new Vector2(padding.left, padding.bottom);
                rectTransform.offsetMax = new Vector2(-padding.right, -padding.top);
            }
        }

        public static void StretchWidthToParentBottom(this RectTransform rectTransform, RectOffset padding = null)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.right;
            rectTransform.anchoredPosition = Vector2.zero;

            if (padding == null)
            {
                rectTransform.sizeDelta = new Vector2(0, rectTransform.sizeDelta.y);
            }
            else
            {
                rectTransform.offsetMin = new Vector2(padding.left, rectTransform.offsetMin.y);
                rectTransform.offsetMax = new Vector2(-padding.right, rectTransform.offsetMax.y);
            }
        }

        public static void StretchHeightToParentRight(this RectTransform rectTransform, float width)
        {
            rectTransform.anchorMin = Vector2.right;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.sizeDelta = new Vector2(width, 0);
        }

        public static void AlignLeftCenterToParent(this RectTransform rectTransform,
            float width, float height, float offset)
        {
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
            rectTransform.anchoredPosition = new Vector2(offset, 0);
            rectTransform.pivot = new Vector2(0, 0.5f);
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        public static void AlignRightCenterToParent(this RectTransform rectTransform,
            float width, float height, float offset)
        {
            rectTransform.anchorMin = new Vector2(1, 0.5f);
            rectTransform.anchorMax = new Vector2(1, 0.5f);
            rectTransform.anchoredPosition = new Vector2(-offset, 0);
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        public static void ExpandDownFromParentBottom(this RectTransform rectTransform, float height)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.right;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 1);
            rectTransform.sizeDelta = new Vector2(0, height);
        }

        public static void ExpandDownFromParentTop(this RectTransform rectTransform, float height)
        {
            rectTransform.anchorMin = Vector2.up;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 1);
            rectTransform.sizeDelta = new Vector2(0, height);
        }

        public static void Resize(this RectTransform rectTransform,
            float? width = null, float? height = null)
        {
            var sizeDelta = rectTransform.sizeDelta;

            if (width.HasValue) sizeDelta.x = width.Value;
            if (height.HasValue) sizeDelta.y = height.Value;

            rectTransform.sizeDelta = sizeDelta;
        }
    }
}