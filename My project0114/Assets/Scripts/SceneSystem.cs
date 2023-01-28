using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class SceneBase
{
    public abstract void OnEnter();

    public abstract void OnExit();
}

public class SceneSystem
{
    private readonly static SceneSystem instance = new SceneSystem();

    private SceneSystem()
    {
        // todo something
    }

    public static SceneSystem GetInstance()
    {
        return instance;
    }

    private SceneBase sceneBase;

    public void SetScene(SceneBase scene)
    {
        sceneBase?.OnExit();
        sceneBase = scene;
        sceneBase?.OnEnter();
    }
}


public class StartScene : SceneBase
{
    public override void OnEnter()
    {
        if (SceneManager.GetActiveScene().name != PathManager.StartScene)
        {
            SceneManager.LoadScene(PathManager.StartScene);
            SceneManager.sceneLoaded += SceneLoaded;
            GameObject BG =  GameUtility.FindChild(MyCanvas.instance.gameObject, "BG");
            BG.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else PanelStack.Instance.Push(new StartPanel());
    }

    public override void OnExit()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    /// <summary>
    /// 场景加载完毕后执行的方法
    /// </summary>
    private void SceneLoaded(Scene scene, LoadSceneMode load)
    {
        PanelStack.Instance.Push(new StartPanel());
    }
}


public class MainScene : SceneBase
{
    public override void OnEnter()
    {
        if (SceneManager.GetActiveScene().name != PathManager.MainScene)
        {
            SceneManager.LoadScene(PathManager.MainScene);
            SceneManager.sceneLoaded += SceneLoaded;
            GameObject BG =  GameUtility.FindChild(MyCanvas.instance.gameObject, "BG");
            BG.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        else PanelStack.Instance.Push(new MainPanel());
    }

    public override void OnExit()
    {
        PlayerController.instance.Destroy();
        BattleManager.instance.Destroy();
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    void SceneLoaded(Scene scene, LoadSceneMode load)
    {
        PanelStack.Instance.Push(new MainPanel());
    }
}