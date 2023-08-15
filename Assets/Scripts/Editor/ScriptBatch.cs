using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Drawing;
using UnityEditor.Build;

public class ScriptBatch
{
    [MenuItem("Tools/Android Build")]
    public static void BuildGame()
    {
        PlayerSettings.productName = "CarSprayRun";
        PlayerSettings.SetIcons(NamedBuildTarget.Android, new[] { Resources.Load<Texture2D>("Final/Icon") }, IconKind.Application);

        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/Scenes/Main.unity" };
        BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.Android, BuildOptions.None);
    }
}
