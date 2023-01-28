using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class QuickOpenScene
{
    [MenuItem("Tools/OpenStartScene_AndPlay &1", priority = 1)]
    private static void PlayStartScene()
    {
        var startScene = "Assets/Scenes/Presistense.unity";
        OpenScene(startScene, true);
    }

    private static void OpenScene(string scenePath, bool isPlay)
    {
        if (EditorApplication.isPlaying) return;

        var sceneName = scenePath.Substring(scenePath.LastIndexOf('/') + 1);
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
        Debug.Log($"进入场景:[{sceneName}]");
        EditorApplication.isPlaying = isPlay;
    }

    [MenuItem(itemName: "Tools/OpenMainScene_AndPlay &2", priority = 2)]
    private static void PlayMainScene()
    {
        var mainScene = "Assets/Fantasy Root Forest/Demo 1.unity";
        OpenScene(mainScene, true);
    }

    [MenuItem(itemName: "Tools/OpenStartScene_AndEdit &3", priority = 3)]
    private static void OpenStartScene()
    {
        var startScene = "Assets/Scenes/Presistense.unity";
        OpenScene(startScene, false);
    }

    [MenuItem(itemName: "Tools/OpenMainScene_AndEdit &4", priority = 4)]
    private static void OpenMainScene()
    {
        var mainScene = "Assets/Fantasy Root Forest/Demo 1.unity";
        OpenScene(mainScene, false);
    }

    [MenuItem(itemName: "Tools/ExitPlayMode &Q", priority = 5)]
    private static void ExitPlayMode()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.Log("退出播放模式...");
            EditorApplication.isPlaying = false;
        }
    }

}
