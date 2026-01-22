#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace QAUI
{
    internal class MainSceneSelector : EditorWindow
    {
        private MonoScript Script { get; set; }

        private Type Type => Script?.GetClass();

        private void OnEnable()
        {
            Script = QAUIScene.MainScene.Script;
        }

        private void OnGUI()
        {
            minSize = new Vector2(400, 100);

            EditorGUILayout.LabelField(QAUIScene.MainScene.Name, EditorStyles.boldLabel);
            Script = (MonoScript)EditorGUILayout.ObjectField($"{typeof(Scene).FullName} Script", Script,
                typeof(MonoScript), false);

            EditorGUILayout.Space();
            using (new EditorGUI.DisabledScope(!Script))
            {
                if (GUILayout.Button("Select") && IsValidType())
                {
                    Apply();
                    Close();
                }
            }
        }

        private bool IsValidType()
        {
            if (Type != null &&
                typeof(MonoBehaviour).IsAssignableFrom(Type) &&
                typeof(Scene).IsAssignableFrom(Type))
            {
                return true;
            }

            Script = null;
            return false;
        }

        private void Apply()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            // Open the QAUI scene
            var scene = EditorSceneManager.OpenScene(QAUIScene.AssetPath, OpenSceneMode.Single);

            var mainSceneGameObject = GameObject.Find(QAUIScene.MainScene.Name);
            var sceneManagerGameObject = GameObject.Find(QAUIScene.SceneManager.Name);
            if (!mainSceneGameObject || !sceneManagerGameObject) return;

            // Remove old scene component
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(mainSceneGameObject);
            foreach (var component in mainSceneGameObject.GetComponents<Component>())
            {
                if (component is Transform) continue;
                Undo.DestroyObjectImmediate(component);
            }

            // Add new scene component
            var newSceneComponent = (Scene)Undo.AddComponent(mainSceneGameObject, Type);

            var sceneNavigation = sceneManagerGameObject.GetComponent<SceneNavigation>();
            if (sceneNavigation) sceneNavigation.mainScene = newSceneComponent;

            // Save scene
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }
    }
}

#endif