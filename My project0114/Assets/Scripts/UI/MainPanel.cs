using System;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public MainPanel() : base(new PanelType(PathManager.MainPanel)) { }

    public class Controls
    {
        public Button Btn_Info;
        public Button Btn_Quit;

        public GameObject Btn_Skill1;
        public GameObject Btn_Skill2;
        public GameObject Btn_Skill3;

        public Image HeadIcon;
        public Text LevelText;
        public Text NameText;
        public Text GoldCountText;
        public Text IDText;
    }

    public static Controls m_ctl;

    public static void PressBtnSk(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Alpha1:
                m_ctl.Btn_Skill1.GetComponent<SkillBtnPressHint>().TurnPressCol();
                break;
            case KeyCode.Alpha2:
                m_ctl.Btn_Skill2.GetComponent<SkillBtnPressHint>().TurnPressCol();
                break;
            case KeyCode.Alpha3:
                m_ctl.Btn_Skill3.GetComponent<SkillBtnPressHint>().TurnPressCol();
                break;
        }
    }

    public static void UpBtnSk(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Alpha1:
                m_ctl.Btn_Skill1.GetComponent<SkillBtnPressHint>().TurnNormalCol();
                break;
            case KeyCode.Alpha2:
                m_ctl.Btn_Skill2.GetComponent<SkillBtnPressHint>().TurnNormalCol();
                break;
            case KeyCode.Alpha3:
                m_ctl.Btn_Skill3.GetComponent<SkillBtnPressHint>().TurnNormalCol();
                break;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        var Info = GameManager.instance.CurrentPlayerInfo;
        m_ctl.NameText.text = Info.Name;
        m_ctl.GoldCountText.text = $"{Info.GoldCount}G";
        m_ctl.LevelText.text = Info.Level.ToString();
        m_ctl.IDText.text = $"ID:{Info.ID}";
        m_ctl.HeadIcon.sprite = Resources.Load<Sprite>($"Pic/HeadPortrait/{Info.HeadImgPath}");
    }

    public override void Serializable()
    {
        GameUtility.FindControls(PanelManager.Instance.panelDict[PanelStack.Instance.stack.Peek().PanelType], ref m_ctl);
    }
    public override void InitListener()
    {
        UIEventTriggerListener.Get(m_ctl.Btn_Info.gameObject).OnClick = OpenInfoPanel;
        UIEventTriggerListener.Get(m_ctl.Btn_Quit.gameObject).OnClick = OpenQuitPanel;
        UIEventTriggerListener.Get(m_ctl.Btn_Skill1.gameObject).OnClick = OnCastSkill1;
    }

    private void OnCastSkill1(GameObject go)
    {
        Debug.Log("Click OnCastSkill1");
        PlayerController.instance.DoAttackAnim();
    }

    private void OpenInfoPanel(GameObject go)
    {
        Debug.Log("Click OpenInfoPanel");
    }

    private void OpenQuitPanel(GameObject go)
    {
        Debug.Log("Click OpenQuitPanel");

        OnExitToStartScene();
    }

    /// <summary>
    /// 返回开始场景
    /// </summary>
    private void OnExitToStartScene()
    {
        SceneSystem.GetInstance().SetScene(new StartScene());
        PanelStack.Instance.PopAll(); // 这行和上一行是否应该呼唤 ？ 我认为可以
        //GameObject.Destroy(SimpleCharacterController.instance.gameObject, 0.034f);
    }

    private void OpenShopPanelByLua(GameObject go)
    {
        ////PanelStack.Instance.Push(new ShopPanel());
        //Debug.Log("load Resources/OpenShop.lua.txt  [call lua][byRes]");

        //LuaEnv env = new LuaEnv();

        //TextAsset luaAsset = Resources.Load<TextAsset>("OpenShop.lua");
        //if (luaAsset is null)
        //{
        //    Debug.LogError("specified lua file doesn't exist!");
        //}
        //else
        //{
        //    env.DoString(luaAsset.text);
        //}
    }

    /// <summary>
    /// 返回开始场景
    /// </summary>
    private void OnExitToStartScene(GameObject go)
    {
        SceneSystem.GetInstance().SetScene(new StartScene());
        PanelStack.Instance.PopAll();
        //GameObject.Destroy(SimpleCharacterController.instance.gameObject, 0.034f);
        // todo something
    }

    private void DoBearPlank(GameObject go)
    {
        //SimpleCharacterController.instance.DoBearPlank();
    }

    private void DoBackStretch(GameObject go)
    {
        //SimpleCharacterController.instance.DoBackStretch();
    }

    private void DoBelly(GameObject go)
    {
        //SimpleCharacterController.instance.DoBelly();
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