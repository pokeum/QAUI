#if UNITY_EDITOR

using System.IO;
using UnityEditor;

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

    public static class GameObject
    {
        public const string MainScene = "Main Scene";
        public const string SceneManager = "Scene Manager";
    }
}

#endif