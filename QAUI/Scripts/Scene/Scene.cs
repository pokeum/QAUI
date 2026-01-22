using System.IO;
using UnityEngine;

namespace QAUI
{
    public abstract class Scene : MonoBehaviour
    {
        internal const string SceneFile = "Scene.unity";

        internal static readonly string AssetPath = Path.Combine("Assets", "QAUI", SceneFile);

        private SceneNavigation SceneNavigation => GetComponent<SceneNavigation>();

        public abstract string Title();

        public abstract void Initialize(GameObject content);

        protected void ShowDialog(IDialog dialogInstance) => SceneNavigation.ShowDialog(dialogInstance);
        protected void CloseDialog() => SceneNavigation.CloseDialog();
    }
}