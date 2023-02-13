using System;
using UnityEngine;
using UnityEngine.UI;

public class SlotPanel : BasePanel
{
    public SlotPanel() : base(new PanelType(PathManager.SlotPanel)) { }

    public class Controls
    {
        public Button Btn_Decline;
        public Transform SaveSlotList;
        public Button UICreateSaveSlot;
    }

    public Controls m_ctl;

    public GameObject UISaveSlot;

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("<color=#ffff00>读取玩家存档...</color>");
        UISaveSlot = Resources.Load<GameObject>("Prefabs/UI/UISaveSlot");

        ResetSaveSlotList();
        
    }

    /// <summary>
    /// 根据存档数 实例化对应数量的 UI存档条Prefab 为对应的存档条附上Index
    /// Index暂时没有用
    /// </summary>
    public void ResetSaveSlotList()
    {
        var PlayDatas = GameManager.instance.PlayerInfo;
        for (int i = 0; i < PlayDatas.datas.Count; i++)
        {
            var go = UnityEngine.GameObject.Instantiate(UISaveSlot, m_ctl.SaveSlotList);
            var saveSlot = go.GetComponent<UISaveSlot>();
            saveSlot.SetText(PlayDatas.datas[i].Name, PlayDatas.datas[i].Level, PlayDatas.datas[i].GoldCount);
            saveSlot.SetImage(PlayDatas.datas[i].HeadImgPath);
            saveSlot.saveIndex = i;
        }

        m_ctl.UICreateSaveSlot.transform.SetAsLastSibling();
    }

    public void AddSaveSlot(PlayData pData)
    {
        var PlayDatas = GameManager.instance.PlayerInfo;
        var go = UnityEngine.GameObject.Instantiate(UISaveSlot, m_ctl.SaveSlotList);
        var saveSlot = go.GetComponent<UISaveSlot>();
        saveSlot.SetText(pData.Name, pData.Level, pData.GoldCount);
        saveSlot.SetImage(pData.HeadImgPath);
        saveSlot.saveIndex = GameManager.instance.PlayerInfo.datas.Count - 1;
        m_ctl.UICreateSaveSlot.transform.SetAsLastSibling();
    }

    public override void Serializable()
    {
        GameUtility.FindControls(PanelManager.Instance.panelDict[PanelStack.Instance.stack.Peek().PanelType], ref m_ctl);
    }
    public override void InitListener()
    {
        UIEventTriggerListener.Get(m_ctl.Btn_Decline.gameObject).OnClick = Decline;

        UIEventTriggerListener.Get(m_ctl.UICreateSaveSlot.gameObject).OnClick = CreateSave;
    }

    private void CreateSave(GameObject go)
    {
        PanelStack.Instance.Push(new UICreateSavePanel());

    }

    private void Decline(GameObject go)
    {
        Debug.Log("Decline SlotPanel");
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
        Debug.Log("OnExit SlotPanel");
        PanelManager.Instance.DestroyPanel(this.PanelType);
    }
}