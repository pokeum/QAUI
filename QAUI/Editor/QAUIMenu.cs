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
        var scenes = EditorBuildSettings.scenes.ToList();
        scenes.RemoveAll(scene => scene.path == QAUIScene.RelativePath);
        scenes.Insert(0, new EditorBuildSettingsScene(QAUIScene.RelativePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }

    [MenuItem("QAUI/Select Main Scene")]
    public static void SelectMainScene()
    {
        EditorWindow.GetWindow<MainSceneSelector>(false, "Main Scene Selector", true).Show();
    }
}

#endif