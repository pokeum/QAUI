#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

internal static class QAUIScene
{
    public static void CopyToAssetPath()
    {
        var sourceFilePath = Path.Combine(QAUI.FileUtil.GetAbsolutePackagePath(), QAUI.Scene.SceneFile);
        if (sourceFilePath.Equals(QAUI.Scene.AssetPath)) return;

        var directory = Path.GetDirectoryName(QAUI.Scene.AssetPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.Copy(sourceFilePath, QAUI.Scene.AssetPath, overwrite: true);
        AssetDatabase.ImportAsset(QAUI.Scene.AssetPath);
    }

    public static class MainScene
    {
        public const string Name = "Main Scene";

        public static MonoScript Script
        {
            get
            {
                var scene = EditorSceneManager.OpenScene(QAUI.Scene.AssetPath, OpenSceneMode.Additive);

                try
                {
                    foreach (var gameObject in scene.GetRootGameObjects())
                    {
                        if (gameObject.name != Name) continue;
                        foreach (var monoBehaviour in gameObject.GetComponents<MonoBehaviour>())
                        {
                            if (monoBehaviour == null) continue;
                            if (monoBehaviour is QAUI.Scene) return MonoScript.FromMonoBehaviour(monoBehaviour);
                        }
                    }
                }
                finally
                {
                    if (scene.isLoaded &&
                        UnityEngine.SceneManagement.SceneManager.sceneCount > 1)
                    {
                        EditorSceneManager.CloseScene(scene, true);
                    }
                }

                return null;
            }
        }
    }

    public static class SceneManager
    {
        public const string Name = "Scene Manager";
    }
}

#endif