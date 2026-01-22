#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace QAUI
{
    internal class MainSceneSelector : EditorWindow
    {
        private const string Key = "QAUI.MainSceneSelector";

        private MonoScript Script { get; set; }

        private Type Type => Script?.GetClass();

        private void OnEnable()
        {
            var guid = EditorPrefs.GetString(Key, string.Empty);
            if (string.IsNullOrEmpty(guid)) return;

            var path = AssetDatabase.GUIDToAssetPath(guid);
            Script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
        }

        private void OnGUI()
        {
            minSize = new Vector2(400, 100);

            EditorGUILayout.LabelField(QAUIScene.GameObject.MainScene, EditorStyles.boldLabel);
            Script = (MonoScript)EditorGUILayout.ObjectField($"{typeof(Scene).FullName} Script", Script,
                typeof(MonoScript), false);

            EditorGUILayout.Space();
            using (new EditorGUI.DisabledScope(!Script))
            {
                if (GUILayout.Button("Select") && IsValidType())
                {
                    Apply();
                    SaveScript();
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
            var scene = EditorSceneManager.OpenScene(QAUIScene.RelativePath, OpenSceneMode.Single);

            var mainSceneGameObject = GameObject.Find(QAUIScene.GameObject.MainScene);
            var sceneManagerGameObject = GameObject.Find(QAUIScene.GameObject.SceneManager);
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

        private void SaveScript()
        {
            if (Script == null)
            {
                EditorPrefs.DeleteKey(Key);
                return;
            }

            var path = AssetDatabase.GetAssetPath(Script);
            var guid = AssetDatabase.AssetPathToGUID(path);
            EditorPrefs.SetString(Key, guid);
        }
    }
}

#endif