using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class QuickOpenScene
{
    [MenuItem("Tools/OpenStartScene_AndPlay &1",priority = 1)]
    private static void OpenStartScene()
    {
        if (EditorApplication.isPlaying) return;

        var startScene = "Assets/Scenes/Presistense.unity";


        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(startScene);
        }

        Debug.Log("进入开始场景");

        EditorApplication.isPlaying = true;
    }

    [MenuItem(itemName:"Tools/OpenMainScene_AndEdit &2",priority = 2)]
    private static void OpenMainScene()
    {
        if (EditorApplication.isPlaying) return;
        var mainScene = "Assets/Fantasy Root Forest/Demo 1.unity";

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(mainScene);
        }

        Debug.Log("进入主场景");
    }

}
