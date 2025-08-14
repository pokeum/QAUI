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

        [MenuItem("Assets/Create/QAUI/Scene", false, 0)]
        public static void CreateQAUIScene() =>
            CreateWithInlineRename(Mode.Scene, "NewScene.cs");

        [MenuItem("Assets/Create/QAUI/Dialog", false, 1)]
        public static void CreateQAUIDialog() =>
            CreateWithInlineRename(Mode.Dialog, "NewDialog.cs");

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
            string folder = "Assets";
            var obj = Selection.activeObject;
            if (obj != null)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path))
                {
                    if (Directory.Exists(path)) folder = path;
                    else folder = Path.GetDirectoryName(path);
                }
            }

            return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folder, fileName));
        }

        /** Handles the confirmed name */
        private class DoCreateModeScript : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                Enum.TryParse(resourceFile, out Mode mode);

                string fileName = Path.GetFileNameWithoutExtension(pathName);
                string className = GetValidClassName(mode, fileName);

                string code = GenerateCode(mode, className);

                Directory.CreateDirectory(Path.GetDirectoryName(pathName)!);
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
                    return
                        $@"using System;
using QAUI;
using UnityEngine;

public class {className} : Scene
{{
    public override string Title()
    {{
        throw new NotImplementedException();
    }}

    public override void Initialize(GameObject content)
    {{
        throw new NotImplementedException();
    }}
}}";
                case Mode.Dialog:
                    return
                        $@"using System;
using QAUI;
using UnityEngine;

public class {className} : IDialog
{{
    public void Initialize(GameObject content)
    {{
        throw new NotImplementedException();
    }}
}}";
                default: return string.Empty;
            }
        }


        private static int _newClassIndex;

        private static string GetValidClassName(Mode mode, string fileName)
        {
            string reserved = string.Join("|", new List<string>()
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
            string pattern = $"^(?!(?:{reserved})$)@?[A-Za-z_][A-Za-z0-9_]*$";
            if (!Regex.IsMatch(fileName, pattern)) return $"{mode.ToString()}Class{++_newClassIndex}";
            return fileName;
        }
    }
}

#endif