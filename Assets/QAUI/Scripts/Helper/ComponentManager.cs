using UnityEngine;
using UnityEngine.UI;

namespace QAUI
{
    internal static class ComponentManager
    {
        public static T CreateUI<T>(this GameObject gameObject, string name = null) where T : Component
        {
            return GameObjectManager.Create(
                root: gameObject,
                name: name ?? typeof(T).Name,
                components: typeof(LayoutElement)
            ).SafeAddComponent<T>();
        }

        public static Component SafeAddComponent(this GameObject gameObject, System.Type componentType)
        {
            Component component = gameObject.GetComponent(componentType);
            return component != null ? component : gameObject.AddComponent(componentType);
        }

        public static T SafeAddComponent<T>(this GameObject gameObject) where T : Component =>
            gameObject.SafeAddComponent(typeof(T)) as T;
    }
}