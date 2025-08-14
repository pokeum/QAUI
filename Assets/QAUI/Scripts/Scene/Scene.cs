using UnityEngine;

namespace QAUI
{
    public abstract class Scene : MonoBehaviour
    {
        private SceneNavigation SceneNavigation => GetComponent<SceneNavigation>();

        public abstract string Title();

        public abstract void Initialize(GameObject content);

        protected void ShowDialog(IDialog dialogInstance) => SceneNavigation.ShowDialog(dialogInstance);
        protected void CloseDialog() => SceneNavigation.CloseDialog();
    }
}