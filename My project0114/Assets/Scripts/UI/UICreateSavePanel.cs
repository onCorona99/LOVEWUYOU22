using UnityEngine;
using UnityEngine.UI;

public class UICreateSavePanel : BasePanel
{
    public UICreateSavePanel() : base(new PanelType(PathManager.UICreateSavePanel)) { }

    public class Controls
    {
        public Button Btn_Decline;
        public Button Btn_Accept;

        public InputField InputField_PlayerName;
        public Button PlayerHead;
    }

    public Controls m_ctl;


    public override void OnEnter()
    {
        base.OnEnter();
        isVisibleWhenPause = true;
    }

    public override void Serializable()
    {
        GameUtility.FindControls(PanelManager.Instance.panelDict[PanelStack.Instance.stack.Peek().PanelType], ref m_ctl);
    }
    public override void InitListener()
    {
        UIEventTriggerListener.Get(m_ctl.Btn_Decline.gameObject).OnClick = Decline;
        UIEventTriggerListener.Get(m_ctl.Btn_Accept.gameObject).OnClick = Accept;
        UIEventTriggerListener.Get(m_ctl.PlayerHead.gameObject).OnClick = OpenHeadSelectPanel;
    }

    private void Decline(GameObject go)
    {
        PanelStack.Instance.Pop();

    }

    private void Accept(GameObject go)
    {
        // create player save data json
        var name = m_ctl.InputField_PlayerName.text;
        if(name.Equals("") || name.Trim().Equals("") || name == null)
        {
            UIManager.instance.ShowUICommonPopUp(DisstrManager.UICreateSlot_NameIsNull);
            return;
        }
        Debug.Log($"{name}");

        PlayData p = new PlayData(name, m_ctl.PlayerHead.image.sprite.name);
        GameManager.instance.PlayerInfo.datas.Add(p);
        JsonManager.Instance.SaveJsonDate(GameManager.instance.PlayerInfo, "/StreamingAssets/Json/PlayerInfo/", "PlayerInfo.json");

        foreach (var panel in PanelStack.Instance.stack)
        {
            if (panel.PanelType.Path.Equals(PathManager.SlotPanel))
            {
                (panel as SlotPanel).AddSaveSlot(p);
            }
            continue;
        }

        PanelStack.Instance.Pop();
    }

    private void OpenHeadSelectPanel(GameObject go)
    {
        //PanelStack.Instance.Push(new UICreateSavePanel());
        Debug.Log("打开选择人头界面...");
        PanelStack.Instance.Push(new UISelectHeadPanel());

    }

    public override void OnExit()
    {
        PanelManager.Instance.DestroyPanel(this.PanelType);
    }
}