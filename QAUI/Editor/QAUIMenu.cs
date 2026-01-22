#if UNITY_EDITOR

using System.Linq;
using QAUI;
using UnityEditor;

/// <summary>
/// This script adds the QAUI menu options to the Unity Editor.
/// </summary>
public static class QAUIMenu
{
    [MenuItem("QAUI/Add Scene In Build")]
    public static void AddSceneInBuild()
    {
        QAUIScene.CopyToAssetPath();

        var scenes = EditorBuildSettings.scenes.ToList();
        scenes.RemoveAll(scene => scene.path == QAUIScene.AssetPath);
        scenes.Insert(0, new EditorBuildSettingsScene(QAUIScene.AssetPath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }

    [MenuItem("QAUI/Select Main Scene")]
    public static void SelectMainScene()
    {
        EditorWindow.GetWindow<MainSceneSelector>(false, "Main Scene Selector", true).Show();
    }
}

#endif