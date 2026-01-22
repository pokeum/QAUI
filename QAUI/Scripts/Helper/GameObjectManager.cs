using UnityEngine;

namespace QAUI
{
    internal static class GameObjectManager
    {
        public static GameObject Create(GameObject root, string name, params System.Type[] components)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(root.transform, false);

            foreach (var component in components)
            {
                obj.AddComponent(component);
            }

            return obj;
        }

        public static RectTransform GetRectTransform(this GameObject obj) =>
            obj.GetComponent<RectTransform>();

        public static bool RemoveFirstChild(this GameObject obj, string name)
        {
            foreach (Transform child in obj.transform)
            {
                if (!child.name.Equals(name)) continue;

                Object.Destroy(child.gameObject);
                return true;
            }

            return false;
        }

        public static void RemoveAllChildren(this GameObject obj)
        {
            foreach (Transform child in obj.transform) Object.Destroy(child.gameObject);
        }
    }
}