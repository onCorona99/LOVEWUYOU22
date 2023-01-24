using UnityEngine;
using UnityEngine.UI;

public class SlotPanel : BasePanel
{
    public SlotPanel() : base(new PanelType(PathManager.SlotPanel)) { }

    public class Controls
    {
        //public InputField UsernameInput;
        //public InputField PasswordInput;
        //public Button LoginButton;
        public Button Btn_Decline;

        public Button Btn_Slot1;
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
        UIEventTriggerListener.Get(m_ctl.Btn_Decline.gameObject).OnClick = Decline;
        UIEventTriggerListener.Get(m_ctl.Btn_Slot1.gameObject).OnClick = ChooseSlot1;
    }

    private void Decline(GameObject go)
    {
        PanelStack.Instance.Pop();
       
    }

    private void ChooseSlot1(GameObject go)
    {
        if (true)
        {
            SceneSystem.GetInstance().SetScene(new MainScene());
            PanelStack.Instance.PopAll();
        }
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