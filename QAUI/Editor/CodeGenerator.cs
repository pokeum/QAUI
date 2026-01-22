#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace QAUI
{
    internal static class CodeGenerator
    {
        private enum Mode
        {
            Scene,
            Dialog
        }

        private const string SceneFileName = "NewScene.cs";
        private const string DialogFileName = "NewDialog.cs";

        private const string ClassNameToken = "#CLASSNAME#";

        [MenuItem("Assets/Create/QAUI/Scene", false, 0)]
        public static void CreateQAUIScene() => CreateWithInlineRename(Mode.Scene, SceneFileName);

        [MenuItem("Assets/Create/QAUI/Dialog", false, 1)]
        public static void CreateQAUIDialog() => CreateWithInlineRename(Mode.Dialog, DialogFileName);

        private static void CreateWithInlineRename(Mode mode, string defaultFileName)
        {
            var icon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                instanceID: 0,
                endAction: ScriptableObject.CreateInstance<DoCreateModeScript>(),
                pathName: GetTargetPathWith(defaultFileName),
                icon: icon,
                resourceFile: mode.ToString()
            );
        }

        private static string GetTargetPathWith(string fileName)
        {
            string directory = null;
            var obj = Selection.activeObject;
            if (obj != null)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path))
                {
                    directory = Directory.Exists(path) ? path : Path.GetDirectoryName(path);
                }
            }

            if (string.IsNullOrEmpty(directory))
            {
                directory = "Assets";
            }

            return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(directory, fileName));
        }

        /** Handles the confirmed name */
        private class DoCreateModeScript : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                Enum.TryParse(resourceFile, out Mode mode);

                var fileName = Path.GetFileNameWithoutExtension(pathName);
                var className = GetValidClassName(mode, fileName);

                var code = GenerateCode(mode, className);

                var directory = Path.GetDirectoryName(pathName);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(pathName, code);

                AssetDatabase.ImportAsset(pathName);
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);

                ProjectWindowUtil.ShowCreatedAsset(script);
                if (script != null) AssetDatabase.OpenAsset(script);
            }
        }

        /** Codegen */
        private static string GenerateCode(Mode mode, string className)
        {
            switch (mode)
            {
                case Mode.Scene:
                    return LoadTemplateTextOrThrow($"{SceneFileName}.txt")
                        .Replace(ClassNameToken, className);
                case Mode.Dialog:
                    return LoadTemplateTextOrThrow($"{DialogFileName}.txt")
                        .Replace(ClassNameToken, className);
                default:
                    return string.Empty;
            }
        }

        private static string LoadTemplateTextOrThrow(string fileName)
        {
            var filePath = Path.Combine(
                FileUtil.GetAbsolutePackagePath(),
                "Resources",
                "ScriptTemplates",
                fileName
            );

            return File.Exists(filePath)
                ? File.ReadAllText(filePath)
                : throw new FileNotFoundException("Script template file not found.", filePath);
        }

        private static int _newClassIndex;

        private static string GetValidClassName(Mode mode, string fileName)
        {
            var reserved = string.Join("|", new List<string>()
            {
                "abstract", "as",
                "base", "bool", "break", "byte",
                "case", "catch", "char", "checked", "class", "const", "continue",
                "decimal", "default", "delegate", "do", "double",
                "else", "enum", "event", "explicit", "extern",
                "false", "finally", "fixed", "float", "for", "foreach",
                "goto",
                "if",
                "implicit", "in", "int", "interface", "internal", "is",
                "lock", "long",
                "namespace", "new", "null",
                "object", "operator", "out", "override",
                "params", "private", "protected", "public",
                "readonly", "ref", "return",
                "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
                "this", "throw", "true", "try", "typeof",
                "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
                "virtual", "void", "volatile",
                "while"
            });
            var pattern = $"^(?!(?:{reserved})$)@?[A-Za-z_][A-Za-z0-9_]*$";
            return !Regex.IsMatch(fileName, pattern)
                ? $"{mode.ToString()}Class{++_newClassIndex}"
                : fileName;
        }
    }
}

#endif