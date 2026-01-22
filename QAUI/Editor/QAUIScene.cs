#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

internal static class QAUIScene
{
    private const string SceneFile = "Scene.unity";

    public static readonly string AssetPath = Path.Combine("Assets", "QAUI", SceneFile);

    public static void CopyToAssetPath()
    {
        var sourceFilePath = Path.Combine(QAUI.FileUtil.GetAbsolutePackagePath(), SceneFile);
        if (sourceFilePath.Equals(AssetPath)) return;

        var directory = Path.GetDirectoryName(AssetPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.Copy(sourceFilePath, AssetPath, overwrite: true);
        AssetDatabase.ImportAsset(AssetPath);
    }

    public static class MainScene
    {
        public const string Name = "Main Scene";

        public static MonoScript Script
        {
            get
            {
                var scene = EditorSceneManager.OpenScene(AssetPath, OpenSceneMode.Additive);

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