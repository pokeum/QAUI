using UnityEngine;

namespace QAUI
{
    [AddComponentMenu("QAUI/Component/UISafeArea")]
    [RequireComponent(typeof(RectTransform))]
    public class UISafeArea : MonoBehaviour
    {
        private RectTransform _rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            Rect area = Screen.safeArea;

            // Pixel size in screen space of the whole screen
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);

            // Set anchors to percentages of the screen used
            _rectTransform.anchorMin = area.position / screenSize;
            _rectTransform.anchorMax = (area.position + area.size) / screenSize;
        }
    }
}