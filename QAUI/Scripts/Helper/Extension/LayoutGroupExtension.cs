using UnityEngine.UI;

namespace QAUI
{
    internal static class LayoutGroupExtension
    {
        public static void FitChildren(this HorizontalOrVerticalLayoutGroup layoutGroup)
        {
            // Control Child Size
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;

            // Use Child Scale
            layoutGroup.childScaleWidth = false;
            layoutGroup.childScaleHeight = false;

            // Child Force Expand
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = true;
        }
    }
}