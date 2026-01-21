#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace QAUI
{
    internal static class FileUtil
    {
        public static string GetRelativePackagePath()
        {
            var script = MonoScript.FromScriptableObject(ScriptableObject.CreateInstance<Locator>());
            return AssetDatabase.GetAssetPath(script).ParentDirectory(2);
        }

        public static string GetAbsolutePackagePath()
        {
            return Path.GetFullPath(GetRelativePackagePath());
        }

        private static string ParentDirectory(this string path, int depth = 0)
        {
            if (depth < 0) depth = 0;

            var dir = path;
            for (var i = 0; i < depth; i++)
            {
                var parentDir = Path.GetDirectoryName(dir);
                if (parentDir == null) break;
                dir = parentDir;
            }

            return dir;
        }
    }
}

#endif