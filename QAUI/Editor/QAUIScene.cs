#if UNITY_EDITOR

using System.IO;
using QAUI;

internal static class QAUIScene
{
    public static readonly string RelativePath = Path.Combine(
        FileUtil.GetRelativePackagePath(),
        "Scenes",
        "QAUIScene.unity"
    );

    public static class GameObject
    {
        public const string MainScene = "Main Scene";
        public const string SceneManager = "Scene Manager";
    }
}

#endif