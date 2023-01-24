using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    public LoginPanel() : base(new PanelType(PathManager.LoginPanel)) { }

    public class Controls
    {
        public InputField UsernameInput;
        public InputField PasswordInput;
        public Button LoginButton;
        public Button BackButton;
    }

    public Controls m_ctl;

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
        UIEventTriggerListener.Get(m_ctl.LoginButton.gameObject).OnClick = Login;
        UIEventTriggerListener.Get(m_ctl.BackButton.gameObject).OnClick = OnExitToStartScene;
    }
    /// <summary>
    /// Ç°ÍùÐÂ³¡¾°
    /// </summary>
    private void Login(GameObject go)
    {
        if (true)
        {
            SceneSystem.GetInstance().SetScene(new MainScene());
            PanelStack.Instance.PopAll();
        }
    }

    private void OnExitToStartScene(GameObject go)
    {
        PanelStack.Instance.Pop();
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