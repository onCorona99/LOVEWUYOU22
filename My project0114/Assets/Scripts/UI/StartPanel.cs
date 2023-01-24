using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    public StartPanel() : base(new PanelType(PathManager.StartPanel)) { /*Debug.Log("XXXXXX"); */}

    public class Controls
    {
        public Button LoginButton;
        public Button QuitButton;
    }

    public Controls m_ctl;
    /// <summary>
    /// 执行OnEnter时 处于栈顶
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Serializable()
    {
        GameUtility.FindControls(PanelManager.Instance.panelDict[PanelStack.Instance.stack.Peek().PanelType], ref m_ctl);
    }
    public override void InitListener()
    {
        UIEventTriggerListener.Get(m_ctl.LoginButton.gameObject).OnClick = OpenSlotPanel;
        UIEventTriggerListener.Get(m_ctl.QuitButton.gameObject).OnClick = QuitGame;
    }

    private void OpenSlotPanel(GameObject go)
    {
        PanelStack.Instance.Push(new SlotPanel());
    }

    private void QuitGame(GameObject go)
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public override void OnExit()
    {
        PanelManager.Instance.DestroyPanel(this.PanelType);
    }

    public override void OnPause()
    {
        PanelExtensionTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public override void OnResume()
    {
        PanelExtensionTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
    }
}