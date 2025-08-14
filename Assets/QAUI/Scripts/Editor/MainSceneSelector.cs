#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace QAUI
{
    internal class MainSceneSelector : EditorWindow
    {
        private MonoScript _mainSceneScriptAsset;

        private void OnGUI()
        {
            minSize = new Vector2(400, 100);

            EditorGUILayout.LabelField(QAUIScene.GameObject.MainScene, EditorStyles.boldLabel);
            _mainSceneScriptAsset =
                (MonoScript)EditorGUILayout.ObjectField(
                    $"{typeof(Scene).FullName} Script",
                    _mainSceneScriptAsset,
                    typeof(MonoScript),
                    false
                );

            EditorGUILayout.Space(10);
            using (new EditorGUI.DisabledScope(!_mainSceneScriptAsset))
            {
                if (GUILayout.Button("Select")) SelectMainScene();
            }
        }

        private void SelectMainScene()
        {
            Type newSceneType = _mainSceneScriptAsset?.GetClass();
            if (newSceneType == null ||
                !typeof(MonoBehaviour).IsAssignableFrom(newSceneType) ||
                !typeof(Scene).IsAssignableFrom(newSceneType))
            {
                _mainSceneScriptAsset = null;
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            // Open the QAUI scene
            var scene = EditorSceneManager.OpenScene(QAUIScene.Path, OpenSceneMode.Single);

            GameObject mainSceneGameObject = GameObject.Find(QAUIScene.GameObject.MainScene);
            GameObject sceneManagerGameObject = GameObject.Find(QAUIScene.GameObject.SceneManager);
            if (!mainSceneGameObject || !sceneManagerGameObject) return;

            // Remove old scene component
            Scene oldSceneComponent = mainSceneGameObject.GetComponent<Scene>();
            if (oldSceneComponent) Undo.DestroyObjectImmediate(oldSceneComponent);

            // Add new scene component
            Scene newSceneComponent = (Scene)Undo.AddComponent(mainSceneGameObject, newSceneType);

            SceneNavigation sceneNavigation = sceneManagerGameObject.GetComponent<SceneNavigation>();
            if (sceneNavigation) sceneNavigation.mainScene = newSceneComponent;

            // Save scene
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            // close window
            Close();
        }
    }
}

#endif